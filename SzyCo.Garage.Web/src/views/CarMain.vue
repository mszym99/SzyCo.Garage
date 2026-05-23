<template>
  <v-container class="pa-6">
    <v-btn @click="goBack" class="mb-4" color="primary" variant="outlined">
      Back to Car List
    </v-btn>

    <v-btn @click="removeCar" class="mb-4" color="red">Remove Car</v-btn>
    <v-btn @click="editDialog = true" class="mb-4" color="secondary"
      >Edit Car</v-btn
    >

    <h2 class="text-h5 mb-4">Car Details</h2>
    <CarHero :carId="carId" />

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
import { CarViewModel } from "@/viewmodels.g";

const route = useRoute();
const router = useRouter();
const carId = Number(route.params.id);

const editDialog = ref(false);
const editCar = ref<CarViewModel>(new CarViewModel());
const snackbar = ref({ show: false, message: "" });

function goBack() {
  router.back();
}

onMounted(async () => {
  const car = new CarViewModel();
  car.carId = carId;
  await car.$load();
  editCar.value = car;
});

async function submitEdit() {
  await editCar.value.$save();
  editDialog.value = false;
  snackbar.value.message = "Car updated!";
  snackbar.value.show = true;
  // Reload car data instead of full page reload
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
