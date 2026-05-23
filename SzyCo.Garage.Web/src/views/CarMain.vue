<template>
  <v-container class="pa-6">
    <v-btn
      prepend-icon="mdi-arrow-left"
      class="mb-4"
      color="primary"
      variant="outlined"
      @click="goBack"
    >
      Back to Car List
    </v-btn>

    <v-btn @click="removeCar" class="mb-4" color="red">Remove Car</v-btn>
    <v-btn @click="editDialog = true" class="mb-4" color="secondary"
      >Edit Car</v-btn
    >

    <h2 class="text-h5 mb-4">Car Details</h2>
    <CarHero :car-id="carId" />

    <!-- Event History -->
    <h3 class="text-h6 mt-6 mb-3">Event History</h3>
    <v-card v-if="eventList.$load.isLoading" class="pa-4">
      <v-progress-circular indeterminate size="24" class="mr-2" />
      Loading events...
    </v-card>
    <v-card
      v-else-if="!eventList.$items.length"
      class="pa-4"
      variant="outlined"
    >
      <v-card-text>No events recorded for this car.</v-card-text>
    </v-card>
    <v-timeline v-else side="end" density="compact">
      <v-timeline-item
        v-for="event in eventList.$items"
        :key="event.id!"
        size="small"
        dot-color="primary"
      >
        <v-card variant="outlined" class="mb-2">
          <v-card-title class="text-subtitle-1">
            {{ event.eventTypeDefinition?.name || "Event" }}
          </v-card-title>
          <v-card-subtitle>
            {{ formatDate(event.createDate) }}
          </v-card-subtitle>
          <v-card-text v-if="getParsedEventData(event)">
            <div
              v-for="(value, key) in getParsedEventData(event)"
              :key="String(key)"
            >
              <strong>{{ formatLabel(String(key)) }}:</strong>
              {{ key === "cost" ? `$${value}` : value }}
            </div>
          </v-card-text>
        </v-card>
      </v-timeline-item>
    </v-timeline>

    <!-- Edit Car Dialog -->
    <v-dialog v-model="editDialog" width="500">
      <v-card>
        <v-card-title>Edit Car</v-card-title>
        <v-card-text>
          <v-text-field v-model="editCar.make" label="Make" />
          <v-text-field v-model="editCar.model" label="Model" />
          <v-text-field v-model="editCar.year" label="Year" type="number" />
          <v-text-field v-model="editCar.color" label="Color" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="editDialog = false">Cancel</v-btn>
          <v-btn color="primary" @click="submitEdit">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar -->
    <v-snackbar v-model="snackbar.show" :timeout="3000" color="success">
      {{ snackbar.message }}
    </v-snackbar>
    <!-- Confirm Delete Dialog -->
    <v-dialog v-model="confirmDeleteDialog" width="400">
      <v-card>
        <v-card-title class="text-h6">Confirm Deletion</v-card-title>
        <v-card-text>Are you sure you want to remove this car?</v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="confirmDeleteDialog = false"
            >Cancel</v-btn
          >
          <v-btn color="red" @click="confirmRemoveCar">Yes, Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import CarHero from "@/components/CarHero.vue";
import { CarViewModel, EventListViewModel } from "@/viewmodels.g";

const route = useRoute();
const router = useRouter();
const carId = Number(route.params.id);

const editDialog = ref(false);
const editCar = ref<CarViewModel>(new CarViewModel());
const snackbar = ref({ show: false, message: "" });

const eventList = new EventListViewModel();
const parsedEventDataCache = new WeakMap<
  object,
  Record<string, string> | null
>();

function goBack() {
  router.back();
}

onMounted(async () => {
  const car = new CarViewModel();
  car.carId = carId;
  await car.$load();
  editCar.value = car;

  eventList.$params.filter = { carId: carId };
  await eventList.$load();
});

function formatDate(date: Date | string | null | undefined): string {
  if (!date) return "";
  const d = new Date(date);
  return d.toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function parseJsonData(
  jsonData: string | null | undefined,
): Record<string, string> | null {
  if (!jsonData) return null;
  try {
    const parsed = JSON.parse(jsonData) as Record<string, string>;
    const nonEmpty: Record<string, string> = {};
    for (const [k, v] of Object.entries(parsed)) {
      if (v !== "" && v != null) nonEmpty[k] = String(v);
    }
    return Object.keys(nonEmpty).length > 0 ? nonEmpty : null;
  } catch {
    return null;
  }
}

function getParsedEventData(event: {
  jsonData: string | null | undefined;
}): Record<string, string> | null {
  if (parsedEventDataCache.has(event)) {
    return parsedEventDataCache.get(event) ?? null;
  }

  const parsedData = parseJsonData(event.jsonData);
  parsedEventDataCache.set(event, parsedData);
  return parsedData;
}

function formatLabel(key: string): string {
  return key.replace(/([A-Z])/g, " $1").replace(/^./, (s) => s.toUpperCase());
}

async function submitEdit() {
  await editCar.value.$save();
  editDialog.value = false;
  snackbar.value.message = "Car updated!";
  snackbar.value.show = true;
  await editCar.value.$load();
}

const confirmDeleteDialog = ref(false);

function removeCar() {
  confirmDeleteDialog.value = true;
}

function confirmRemoveCar() {
  const car = new CarViewModel();
  car.carId = carId;
  car.$delete().then(() => {
    confirmDeleteDialog.value = false;
    router.back();
  });
}
</script>
