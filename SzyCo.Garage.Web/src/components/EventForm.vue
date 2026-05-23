<template>
  <v-dialog v-model="dialog" max-width="600px">
    <template v-slot:activator="{ props: activatorProps }">
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
        <v-form v-model="valid" ref="form" lazy-validation>
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
            :items="eventTypes"
            item-title="name"
            item-value="id"
            label="Event Type"
            required
          />

          <div v-if="eventForm.EventTypeId === 1">
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
import { ref, onMounted, computed, defineProps } from "vue";
import {
  EventViewModel,
  CarViewModel,
  SecurityServiceViewModel,
  CarListViewModel,
} from "@/viewmodels.g";

const emit = defineEmits<{ saved: [] }>();

const dialog = ref(false);
const valid = ref(false);

// Form state
const eventForm = ref({
  CarId: null as number | null,
  EventTypeId: null as number | null,
});

const replacementFields = ref({
  PartName: "",
  Reason: "",
  Cost: "",
});

// Rules
const requiredRule = [(v: unknown) => !!v || "This field is required"];

// Event types
const eventTypes = ref([{ id: 1, name: "Replacement" }]);

// Cars
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

// Save
const saveEvent = async () => {
  const eventVM = new EventViewModel();
  eventVM.carId = eventForm.value.CarId!;
  eventVM.eventTypeId = eventForm.value.EventTypeId!;

  let jsonData: Record<string, string> = {};

  if (eventForm.value.EventTypeId === 1) {
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
    emit("saved");
  } catch (err) {
    console.error("Error saving event:", err);
  }
};
</script>
