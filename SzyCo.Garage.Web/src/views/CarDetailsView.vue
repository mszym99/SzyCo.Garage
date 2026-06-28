<template>
  <v-container class="pa-6">
    <div class="d-flex flex-wrap align-center ga-2 mb-4">
      <v-btn
        prepend-icon="fa fa-arrow-left"
        color="primary"
        variant="outlined"
        @click="goBack"
      >
        Back to Car List
      </v-btn>

      <v-btn
        prepend-icon="fa fa-trash"
        color="error"
        variant="tonal"
        @click="removeCar"
      >
        Remove Car
      </v-btn>
      <v-btn
        v-if="!isArchived"
        prepend-icon="fa fa-pencil"
        color="secondary"
        variant="tonal"
        @click="editDialog = true"
      >
        Edit Car
      </v-btn>
      <EventForm
        v-if="!isArchived"
        :car-list="carList"
        :default-car-id="carId"
        @saved="handleEventSaved('Event added.')"
      />
    </div>
    <h2 class="text-h5 mb-4">Car Details</h2>
    <CarHero
      :car-id="carId"
      :refresh-key="carRefreshKey"
      :total-event-history-cost="totalEventHistoryCost"
    />
    <v-alert
      v-if="isArchived"
      class="mt-4"
      type="info"
      variant="tonal"
      density="comfortable"
    >
      This vehicle has been sold and archived. Its history is read-only.
    </v-alert>

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
        v-for="event in sortedEvents"
        :key="event.id!"
        size="small"
        dot-color="primary"
      >
        <v-card variant="outlined" class="mb-2">
          <v-card-title class="event-card-header text-subtitle-1">
            <span class="event-card-name">
              {{ event.eventTypeDefinition?.name || "Event" }}
            </span>
            <v-menu v-if="!isArchived" location="bottom end">
              <template #activator="{ props: menuProps }">
                <v-btn
                  v-bind="menuProps"
                  class="event-action-menu-button"
                  icon="fa fa-ellipsis-v"
                  size="small"
                  variant="text"
                  density="comfortable"
                  title="Event actions"
                  aria-label="Event actions"
                />
              </template>
              <v-list
                class="event-action-menu"
                density="compact"
                min-width="190"
              >
                <EventForm
                  :car-list="carList"
                  :event="event"
                  :default-car-id="carId"
                  @saved="handleEventSaved('Event updated.')"
                >
                  <template #activator="{ props: activatorProps }">
                    <v-list-item
                      v-bind="activatorProps"
                      prepend-icon="fa fa-pencil"
                      title="Edit event"
                    />
                  </template>
                </EventForm>
                <v-list-item
                  prepend-icon="fa fa-copy"
                  :title="
                    copyingEventId === event.id ? 'Copying...' : 'Copy to today'
                  "
                  :disabled="copyingEventId === event.id"
                  @click="copyEvent(event)"
                />
                <v-list-item
                  class="event-action-menu-item-delete"
                  prepend-icon="fa fa-trash"
                  title="Delete event"
                  @click="removeEvent(event)"
                />
              </v-list>
            </v-menu>
          </v-card-title>
          <v-card-subtitle class="event-card-date">
            {{ formatDate(event.createDate) }}
          </v-card-subtitle>
          <v-card-text v-if="getParsedEventData(event)">
            <div
              v-for="(value, key) in getParsedEventData(event)"
              :key="String(key)"
            >
              <strong>{{ formatLabel(String(key)) }}:</strong>
              {{ formatEventValue(String(key), value) }}
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
          <v-btn color="error" @click="confirmRemoveCar">Yes, Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-dialog v-model="confirmEventDeleteDialog" width="400">
      <v-card>
        <v-card-title class="text-h6">Confirm Deletion</v-card-title>
        <v-card-text>
          Are you sure you want to remove {{ eventPendingDeleteName }}?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn variant="text" @click="confirmEventDeleteDialog = false">
            Cancel
          </v-btn>
          <v-btn color="error" @click="confirmRemoveEvent">Yes, Delete</v-btn>
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
  EventServiceViewModel,
} from "@/viewmodels.g";

const route = useRoute();
const router = useRouter();
const carId = Number(route.params.id);

const editDialog = ref(false);
const editCar = ref<CarViewModel>(new CarViewModel());
const carRefreshKey = ref(0);
const copyingEventId = ref<number | null>(null);
const confirmEventDeleteDialog = ref(false);
const eventPendingDelete = ref<EventViewModel | null>(null);
const snackbar = ref({
  show: false,
  message: "",
  color: "success",
});

const carList = new CarListViewModel();
const eventList = new EventListViewModel();
const eventService = new EventServiceViewModel();
const car = ref<CarViewModel>(new CarViewModel());
const parsedEventDataCache = ref(
  new WeakMap<object, Record<string, string> | null>(),
);

const totalEventHistoryCost = computed(() =>
  formatCurrency(car.value.totalEventHistoryCost ?? 0),
);

const isArchived = computed(() => car.value.isArchived === true);

const sortedEvents = computed(() =>
  [...eventList.$items].sort((a, b) => {
    const bTime = b.createDate ? new Date(b.createDate).getTime() : 0;
    const aTime = a.createDate ? new Date(a.createDate).getTime() : 0;
    return bTime - aTime;
  }),
);

const eventPendingDeleteName = computed(
  () => eventPendingDelete.value?.eventTypeDefinition?.name ?? "this event",
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
  if (parsedEventDataCache.value.has(event)) {
    return parsedEventDataCache.value.get(event) ?? null;
  }

  const parsedData = parseJsonData(event.jsonData);
  parsedEventDataCache.value.set(event, parsedData);
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

function formatEventValue(key: string, value: string): string {
  const normalizedKey = key.toLowerCase();
  return normalizedKey.includes("cost") || normalizedKey.includes("price")
    ? `$${value}`
    : value;
}

async function submitEdit() {
  if (isArchived.value) {
    showSnackbar("Sold vehicles are read-only.", "error");
    return;
  }

  await editCar.value.$save();
  await editCar.value.$load();
  carRefreshKey.value += 1;
  editDialog.value = false;
  showSnackbar("Car updated!");
}

async function refreshEvents() {
  parsedEventDataCache.value = new WeakMap<
    object,
    Record<string, string> | null
  >();
  await Promise.all([eventList.$load(), car.value.$load()]);
  carRefreshKey.value += 1;
}

async function handleEventSaved(message: string) {
  await refreshEvents();
  showSnackbar(message);
}

async function copyEvent(event: EventViewModel) {
  if (isArchived.value) {
    showSnackbar("Sold vehicles are read-only.", "error");
    return;
  }

  if (event.id == null) {
    showSnackbar("Event is missing.", "error");
    return;
  }

  try {
    copyingEventId.value = event.id;
    await eventService.copyEventToToday(event.id);
    await refreshEvents();
    showSnackbar("Event copied to today.");
  } catch (error) {
    console.error("Error copying event:", error);
    showSnackbar("Failed to copy event. Please try again.", "error");
  } finally {
    copyingEventId.value = null;
  }
}

function removeEvent(event: EventViewModel) {
  if (isArchived.value) {
    showSnackbar("Sold vehicles are read-only.", "error");
    return;
  }

  eventPendingDelete.value = event;
  confirmEventDeleteDialog.value = true;
}

async function confirmRemoveEvent() {
  if (isArchived.value) {
    showSnackbar("Sold vehicles are read-only.", "error");
    return;
  }

  const eventId = eventPendingDelete.value?.id;
  if (eventId == null) return;

  try {
    const event = new EventViewModel();
    event.id = eventId;
    await event.$delete();

    confirmEventDeleteDialog.value = false;
    eventPendingDelete.value = null;
    await refreshEvents();
    showSnackbar("Event removed.");
  } catch (error) {
    console.error("Error deleting event:", error);
    showSnackbar("Failed to remove event. Please try again.", "error");
  }
}

function showSnackbar(message: string, color = "success") {
  snackbar.value.message = message;
  snackbar.value.color = color;
  snackbar.value.show = true;
}

const confirmDeleteDialog = ref(false);

function removeCar() {
  confirmDeleteDialog.value = true;
}

async function confirmRemoveCar() {
  try {
    const car = new CarViewModel();
    car.carId = carId;
    await car.$delete();

    confirmDeleteDialog.value = false;
    showSnackbar("Car removed.");
    router.back();
  } catch (error) {
    console.error("Error deleting car:", error);
    showSnackbar("Failed to remove car. Please try again.", "error");
  }
}
</script>

<style scoped>
.event-card-header {
  display: grid;
  grid-template-columns: minmax(0, 1fr) auto;
  gap: 8px;
  align-items: center;
  padding-bottom: 0;
  white-space: normal;
}

.event-card-name {
  display: block;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
}

.event-card-date {
  padding-top: 2px;
}

.event-action-menu-button {
  flex: 0 0 auto;
}

.event-action-menu-item-delete {
  color: rgb(var(--v-theme-error));
}
</style>
