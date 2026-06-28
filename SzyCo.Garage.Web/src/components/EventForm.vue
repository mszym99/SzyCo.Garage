<template>
  <v-dialog v-model="dialog" max-width="600px">
    <template #activator="{ props: activatorProps }">
      <slot name="activator" :props="activatorProps">
        <v-btn
          v-bind="activatorProps"
          color="surface-variant"
          prepend-icon="fa fa-plus"
          :text="activatorText"
          variant="flat"
        />
      </slot>
    </template>

    <v-card>
      <v-card-title>{{ dialogTitle }}</v-card-title>

      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-select
            v-model="eventForm.carId"
            :items="carOptions"
            item-title="display"
            item-value="carId"
            label="Select Car"
            :rules="requiredRule"
            required
          />

          <v-select
            v-model="eventForm.eventTypeId"
            :items="eventTypeOptions"
            item-title="name"
            item-value="eventTypeDefinitionId"
            label="Event Type"
            :loading="eventTypeList.$load.isLoading"
            :rules="requiredRule"
            required
          />

          <template v-if="selectedEventDefinitionFields.length">
            <template
              v-for="field in selectedEventDefinitionFields"
              :key="field.name"
            >
              <v-select
                v-if="isSelectField(field)"
                v-model="eventFieldValues[field.name]"
                :items="field.options"
                :label="field.label"
                :rules="rulesForField(field)"
                :required="field.required"
              />
              <v-textarea
                v-else-if="isTextareaField(field)"
                v-model="eventFieldValues[field.name]"
                :label="field.label"
                :rules="rulesForField(field)"
                :required="field.required"
                auto-grow
                rows="2"
              />
              <v-text-field
                v-else
                v-model="eventFieldValues[field.name]"
                :label="field.label"
                :prefix="prefixForField(field)"
                :rules="rulesForField(field)"
                :type="inputTypeForField(field)"
                :required="field.required"
              />
            </template>
          </template>

          <v-alert
            v-if="saveError"
            class="mt-2"
            type="error"
            variant="tonal"
            density="compact"
          >
            {{ saveError }}
          </v-alert>
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn @click="dialog = false">Cancel</v-btn>
        <v-btn
          color="primary"
          :disabled="!canSave"
          :loading="saving"
          @click="saveEvent"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script setup lang="ts">
import { ref, onMounted, computed, nextTick, watch } from "vue";
import {
  EventViewModel,
  CarListViewModel,
  EventTypeDefinitionListViewModel,
} from "@/viewmodels.g";
import type { Event as EventModel } from "@/models.g";

const emit = defineEmits<{ saved: [] }>();

type FormRef = {
  validate: () => Promise<{ valid: boolean }>;
  resetValidation?: () => void;
};

type EventFieldDefinition = {
  name: string;
  label: string;
  type: string;
  required: boolean;
  options: string[];
};

const props = withDefaults(
  defineProps<{
    carList: CarListViewModel;
    event?: EventModel | null;
    defaultCarId?: number | null;
    activatorText?: string;
  }>(),
  {
    event: null,
    defaultCarId: null,
    activatorText: "Add New Event",
  },
);

const form = ref<FormRef | null>(null);
const dialog = ref(false);
const valid = ref(false);
const saving = ref(false);
const saveError = ref("");

const eventForm = ref({
  carId: null as number | null,
  eventTypeId: null as number | null,
});

const eventFieldValues = ref<Record<string, string>>({});
const fallbackJsonData = ref("{}");

const isEditing = computed(() => !!props.event?.id);
const dialogTitle = computed(() =>
  isEditing.value ? "Edit Event" : "Add New Event",
);

function getDefaultCarId() {
  if (props.defaultCarId != null) return props.defaultCarId;
  return carOptions.value.length === 1 ? carOptions.value[0].carId : null;
}

function resetForm({ resetValidation = true } = {}) {
  eventForm.value = {
    carId: getDefaultCarId(),
    eventTypeId: null,
  };
  eventFieldValues.value = {};
  fallbackJsonData.value = "{}";
  saveError.value = "";
  valid.value = Boolean(eventForm.value.carId && eventForm.value.eventTypeId);
  if (resetValidation) form.value?.resetValidation?.();
}

const requiredRule = [(v: unknown) => !!v || "This field is required"];

// Load event types from backend
const eventTypeList = new EventTypeDefinitionListViewModel();

onMounted(() => {
  eventTypeList.$load();
});

const eventTypeOptions = computed(() => {
  return eventTypeList.$items
    .filter(
      (et) =>
        et.isActive || et.eventTypeDefinitionId === eventForm.value.eventTypeId,
    )
    .map((et) => ({
      eventTypeDefinitionId: et.eventTypeDefinitionId!,
      name: et.name!,
    }));
});

const selectedEventTypeDefinition = computed(() => {
  if (!eventForm.value.eventTypeId) return null;
  const found = eventTypeList.$items.find(
    (et) => et.eventTypeDefinitionId === eventForm.value.eventTypeId,
  );
  return found ?? props.event?.eventTypeDefinition ?? null;
});

const selectedEventDefinitionFields = computed(() =>
  parseEventDefinition(selectedEventTypeDefinition.value?.jsonDefinition),
);

const carOptions = computed(() => {
  if (!props.carList || !props.carList.$items?.length) return [];
  return props.carList.$items
    .filter((car) => !car.isArchived || car.carId === eventForm.value.carId)
    .map((car) => ({
      carId: car.carId!,
      display: `${car.year} ${car.make} ${car.model} (${car.color})`,
    }));
});

const canSave = computed(
  () =>
    !saving.value &&
    Boolean(eventForm.value.carId && eventForm.value.eventTypeId),
);

watch(dialog, async (isOpen) => {
  if (!isOpen) return;

  hydrateForm();
  await nextTick();
  valid.value = canSave.value;
});

watch(
  () => props.event,
  () => {
    if (dialog.value) hydrateForm();
  },
);

watch(
  () => eventForm.value.eventTypeId,
  (newEventTypeId, oldEventTypeId) => {
    if (!dialog.value || newEventTypeId === oldEventTypeId) return;

    const parsedJsonData =
      props.event?.eventTypeId === newEventTypeId
        ? parseJsonData(props.event.jsonData)
        : {};
    eventFieldValues.value = getInitialEventFieldValues(parsedJsonData);
  },
);

watch(selectedEventDefinitionFields, () => {
  hydrateDefinitionFields();
});

watch(carOptions, () => {
  if (!dialog.value || eventForm.value.carId) return;
  eventForm.value.carId = getDefaultCarId();
});

function hydrateForm() {
  if (!props.event) {
    resetForm({ resetValidation: false });
    return;
  }

  const parsedJsonData = parseJsonData(props.event.jsonData);
  eventForm.value = {
    carId: props.event.carId ?? getDefaultCarId(),
    eventTypeId: props.event.eventTypeId,
  };
  eventFieldValues.value = getInitialEventFieldValues(parsedJsonData);
  fallbackJsonData.value = props.event.jsonData?.trim() || "{}";
  hydrateDefinitionFields(parsedJsonData);
  saveError.value = "";
  valid.value = Boolean(eventForm.value.carId && eventForm.value.eventTypeId);
}

function parseJsonData(
  jsonData: string | null | undefined,
): Record<string, unknown> {
  if (!jsonData) return {};

  try {
    const parsed = JSON.parse(jsonData) as unknown;
    if (!parsed || typeof parsed !== "object" || Array.isArray(parsed)) {
      return {};
    }

    return parsed as Record<string, unknown>;
  } catch {
    return {};
  }
}

function getStringField(data: Record<string, unknown>, ...keys: string[]) {
  for (const key of keys) {
    const value = data[key];
    if (value != null) return String(value);
  }

  return "";
}

function getInitialEventFieldValues(data: Record<string, unknown>) {
  const values: Record<string, string> = {};

  for (const [key, value] of Object.entries(data)) {
    if (value != null && typeof value !== "object") {
      values[key] = String(value);
    }
  }

  return values;
}

function hydrateDefinitionFields(data = parseJsonData(fallbackJsonData.value)) {
  const values = { ...eventFieldValues.value };

  for (const field of selectedEventDefinitionFields.value) {
    if (values[field.name] == null || values[field.name] === "") {
      values[field.name] = getStringField(
        data,
        field.name,
        toPascalCase(field.name),
      );
    }
  }

  eventFieldValues.value = values;
}

function parseEventDefinition(
  jsonDefinition: string | null | undefined,
): EventFieldDefinition[] {
  if (!jsonDefinition) return [];

  try {
    const parsed = JSON.parse(jsonDefinition) as {
      fields?: unknown;
    };

    if (!Array.isArray(parsed.fields)) return [];

    return parsed.fields
      .map(normalizeEventFieldDefinition)
      .filter((field): field is EventFieldDefinition => field != null);
  } catch {
    return [];
  }
}

function normalizeEventFieldDefinition(
  field: unknown,
): EventFieldDefinition | null {
  if (!field || typeof field !== "object") return null;

  const rawField = field as Record<string, unknown>;
  const name = typeof rawField.name === "string" ? rawField.name.trim() : "";
  if (!name) return null;

  const label =
    typeof rawField.label === "string" && rawField.label.trim()
      ? rawField.label.trim()
      : formatFieldLabel(name);

  const type =
    typeof rawField.type === "string" && rawField.type.trim()
      ? rawField.type.trim().toLowerCase()
      : "text";

  const options = Array.isArray(rawField.options)
    ? rawField.options
        .filter((option) => option != null)
        .map((option) => String(option))
    : [];

  return {
    name,
    label,
    type,
    required: rawField.required === true,
    options,
  };
}

function formatFieldLabel(name: string) {
  return name
    .replace(/([A-Z])/g, " $1")
    .replace(/^./, (first) => first.toUpperCase());
}

function toPascalCase(value: string) {
  return value.charAt(0).toUpperCase() + value.slice(1);
}

function isSelectField(field: EventFieldDefinition) {
  return field.type === "select" || field.type === "enum";
}

function isTextareaField(field: EventFieldDefinition) {
  return field.type === "textarea";
}

function inputTypeForField(field: EventFieldDefinition) {
  if (field.type === "date") return "date";
  if (field.type === "number" || field.type === "currency") return "number";
  return "text";
}

function prefixForField(field: EventFieldDefinition) {
  return field.type === "currency" ? "$" : undefined;
}

function rulesForField(field: EventFieldDefinition) {
  return field.required ? requiredRule : [];
}

const saveEvent = async () => {
  const validation = await form.value?.validate();
  if (validation && !validation.valid) return;

  const eventVM = new EventViewModel(props.event ?? undefined);
  eventVM.carId = eventForm.value.carId!;
  eventVM.eventTypeId = eventForm.value.eventTypeId!;

  let jsonData: Record<string, unknown> = {};

  if (selectedEventDefinitionFields.value.length) {
    for (const field of selectedEventDefinitionFields.value) {
      jsonData[field.name] = eventFieldValues.value[field.name] ?? "";
    }
  } else if (fallbackJsonData.value.trim()) {
    jsonData = parseJsonData(fallbackJsonData.value);
  }

  eventVM.jsonData = JSON.stringify(jsonData);

  try {
    saving.value = true;
    saveError.value = "";
    await eventVM.$save();
    dialog.value = false;
    resetForm();
    emit("saved");
  } catch (err) {
    console.error("Error saving event:", err);
    saveError.value = "Failed to save event.";
  } finally {
    saving.value = false;
  }
};
</script>
