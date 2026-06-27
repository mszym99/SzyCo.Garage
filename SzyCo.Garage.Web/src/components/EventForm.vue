<template>
  <v-dialog v-model="dialog" max-width="600px">
    <template #activator="{ props: activatorProps }">
      <v-btn
        v-bind="activatorProps"
        color="surface-variant"
        text="Add New Event"
        variant="flat"
      />
    </template>

    <v-card>
      <v-card-title>Add New Event</v-card-title>

      <v-card-text>
        <v-form ref="form" v-model="valid" lazy-validation>
          <v-select
            v-model="eventForm.CarId"
            :items="carOptions"
            item-title="display"
            item-value="carId"
            label="Select Car"
            :rules="requiredRule"
            required
          />

          <v-select
            v-model="eventForm.EventTypeId"
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
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn @click="dialog = false">Cancel</v-btn>
        <v-btn :disabled="!valid" @click="saveEvent">Save</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import {
  EventViewModel,
  CarListViewModel,
  EventTypeDefinitionListViewModel,
} from "@/viewmodels.g";

const emit = defineEmits<{ saved: [] }>();

const dialog = ref(false);
const valid = ref(false);

const eventForm = ref({
  CarId: null as number | null,
  EventTypeId: null as number | null,
});

const replacementFields = ref({
  PartName: "",
  Reason: "",
  Cost: "",
});

function resetForm() {
  eventForm.value = {
    CarId: null,
    EventTypeId: null,
  };
  replacementFields.value = {
    PartName: "",
    Reason: "",
    Cost: "",
  };
  valid.value = false;
}

const requiredRule = [(v: unknown) => !!v || "This field is required"];

// Load event types from backend
const eventTypeList = new EventTypeDefinitionListViewModel();

onMounted(() => {
  eventTypeList.$load();
});

const eventTypeOptions = computed(() => {
  return eventTypeList.$items
    .filter((et) => et.isActive)
    .map((et) => ({
      eventTypeDefinitionId: et.eventTypeDefinitionId!,
      name: et.name!,
    }));
});

const selectedEventTypeName = computed(() => {
  if (!eventForm.value.EventTypeId) return null;
  const found = eventTypeList.$items.find(
    (et) => et.eventTypeDefinitionId === eventForm.value.EventTypeId,
  );
  return found?.name ?? null;
});

const isReplacementEventType = computed(
  () => selectedEventTypeName.value?.toLowerCase() === "replacement",
);

const props = defineProps<{
  carList: CarListViewModel;
}>();

const carOptions = computed(() => {
  if (!props.carList || !props.carList.$items?.length) return [];
  return props.carList.$items.map((car) => ({
    carId: car.carId!,
    display: `${car.year} ${car.make} ${car.model} (${car.color})`,
  }));
});

const saveEvent = async () => {
  const eventVM = new EventViewModel();
  eventVM.carId = eventForm.value.CarId!;
  eventVM.eventTypeId = eventForm.value.EventTypeId!;

  let jsonData: Record<string, string> = {};

  if (isReplacementEventType.value) {
    jsonData = {
      partName: replacementFields.value.PartName,
      reason: replacementFields.value.Reason,
      cost: replacementFields.value.Cost,
    };
  }

  eventVM.jsonData = JSON.stringify(jsonData);

  try {
    await eventVM.$save();
    dialog.value = false;
    resetForm();
    emit("saved");
  } catch (err) {
    console.error("Error saving event:", err);
  }
};
</script>
