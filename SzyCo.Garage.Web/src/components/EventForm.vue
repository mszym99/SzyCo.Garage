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

          <div v-if="isReplacementEventType">
            <v-text-field
              v-model="replacementFields.PartName"
              label="Part Name"
              required
            />
            <v-text-field
              v-model="replacementFields.Reason"
              label="Reason for Replacement"
            />
            <v-text-field
              v-model="replacementFields.Cost"
              label="Cost"
              prefix="$"
              type="number"
            />
          </div>

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

const replacementFields = ref({
  PartName: "",
  Reason: "",
  Cost: "",
});

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
  replacementFields.value = {
    PartName: "",
    Reason: "",
    Cost: "",
  };
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

const selectedEventTypeName = computed(() => {
  if (!eventForm.value.eventTypeId) return null;
  const found = eventTypeList.$items.find(
    (et) => et.eventTypeDefinitionId === eventForm.value.eventTypeId,
  );
  return found?.name ?? props.event?.eventTypeDefinition?.name ?? null;
});

const isReplacementEventType = computed(
  () => selectedEventTypeName.value?.toLowerCase() === "replacement",
);

const carOptions = computed(() => {
  if (!props.carList || !props.carList.$items?.length) return [];
  return props.carList.$items.map((car) => ({
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
  replacementFields.value = {
    PartName: getStringField(parsedJsonData, "partName", "PartName"),
    Reason: getStringField(parsedJsonData, "reason", "Reason"),
    Cost: getStringField(parsedJsonData, "cost", "Cost"),
  };
  fallbackJsonData.value = props.event.jsonData?.trim() || "{}";
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

const saveEvent = async () => {
  const validation = await form.value?.validate();
  if (validation && !validation.valid) return;

  const eventVM = new EventViewModel(props.event ?? undefined);
  eventVM.carId = eventForm.value.carId!;
  eventVM.eventTypeId = eventForm.value.eventTypeId!;

  let jsonData: Record<string, unknown> = {};

  if (isReplacementEventType.value) {
    jsonData = {
      partName: replacementFields.value.PartName,
      reason: replacementFields.value.Reason,
      cost: replacementFields.value.Cost,
    };
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
