<template>
  <v-container class="pa-6">
    <div class="d-flex flex-wrap align-center ga-2 mb-4">
      <v-btn
        prepend-icon="mdi-arrow-left"
        color="primary"
        variant="outlined"
        @click="goBack"
      >
        Back to Car List
      </v-btn>

      <v-btn color="red" @click="removeCar">Remove Car</v-btn>
      <v-btn color="secondary" @click="editDialog = true">Edit Car</v-btn>
      <EventForm :car-list="carList" @saved="refreshEvents" />
    </div>
    <h2 class="text-h5 mb-4">Car Details</h2>
    <CarHero
      :car-id="carId"
      :refresh-key="carRefreshKey"
      :total-event-history-cost="totalEventHistoryCost"
    />

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
          <v-text-field v-model="editCar.year" label="Year" type="number" />
          <v-text-field v-model="editCar.make" label="Make" />
          <v-text-field v-model="editCar.model" label="Model" />
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
    <v-snackbar v-model="snackbar.show" :timeout="3000" :color="snackbar.color">
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
import { computed, ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import CarHero from "@/components/CarHero.vue";
import EventForm from "@/components/EventForm.vue";
import {
  CarListViewModel,
  CarViewModel,
  EventViewModel,
  EventListViewModel,
} from "@/viewmodels.g";

const route = useRoute();
const router = useRouter();
const carId = Number(route.params.id);

const editDialog = ref(false);
const editCar = ref<CarViewModel>(new CarViewModel());
const carRefreshKey = ref(0);
const snackbar = ref({
  show: false,
  message: "",
  color: "success",
});

const carList = new CarListViewModel();
const eventList = new EventListViewModel();
const car = ref<CarViewModel>(new CarViewModel());
let parsedEventDataCache = new WeakMap<object, Record<string, string> | null>();

const totalEventHistoryCost = computed(() =>
  formatCurrency(car.value.totalEventHistoryCost ?? 0),
);

function goBack() {
  router.back();
}

onMounted(async () => {
  car.value.carId = carId;
  carList.$params.filter = { carId };

  eventList.$params.filter = { carId: carId };

  await Promise.all([car.value.$load(), carList.$load(), eventList.$load()]);
  editCar.value = car.value;
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

function parseJsonData(
  jsonData: string | null | undefined,
): Record<string, string> | null {
  if (!jsonData) return null;

  try {
    const parsed = JSON.parse(jsonData) as Record<string, unknown>;
    const nonEmpty: Record<string, string> = {};

    for (const [key, value] of Object.entries(parsed)) {
      if (value !== "" && value != null) nonEmpty[key] = String(value);
    }

    return Object.keys(nonEmpty).length > 0 ? nonEmpty : null;
  } catch {
    return null;
  }
}

function formatCurrency(value: number): string {
  return new Intl.NumberFormat(undefined, {
    style: "currency",
    currency: "USD",
  }).format(value);
}

function formatLabel(key: string): string {
  return key.replace(/([A-Z])/g, " $1").replace(/^./, (s) => s.toUpperCase());
}

async function submitEdit() {
  await editCar.value.$save();
  await editCar.value.$load();
  carRefreshKey.value += 1;
  editDialog.value = false;
  snackbar.value.message = "Car updated!";
  snackbar.value.color = "success";
  snackbar.value.show = true;
}

async function refreshEvents() {
  parsedEventDataCache = new WeakMap<object, Record<string, string> | null>();
  await Promise.all([eventList.$load(), car.value.$load()]);
}

const confirmDeleteDialog = ref(false);

function removeCar() {
  confirmDeleteDialog.value = true;
}

async function confirmRemoveCar() {
  try {
    const eventIds = eventList.$items
      .map((e) => e.id)
      .filter((id) => id != null);

    // Delete dependent events first to avoid FK constraint failures when deleting a car.
    await Promise.all(
      eventIds.map(async (id) => {
        const event = new EventViewModel();
        event.id = id;
        await event.$delete();
      }),
    );

    const car = new CarViewModel();
    car.carId = carId;
    await car.$delete();

    confirmDeleteDialog.value = false;
    snackbar.value.message = "Car removed.";
    snackbar.value.color = "success";
    snackbar.value.show = true;
    router.back();
  } catch (error) {
    console.error("Error deleting car:", error);
    snackbar.value.message = "Failed to remove car. Please try again.";
    snackbar.value.color = "error";
    snackbar.value.show = true;
  }
}
</script>
