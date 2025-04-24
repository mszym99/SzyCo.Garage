<template>
  <v-container class="pa-6">
    <v-card v-if="car">
      <v-card-title> {{ car.make }} {{ car.model }} </v-card-title>
      <v-card-subtitle> Year: {{ car.year }} </v-card-subtitle>
      <v-card-text>
        <p>
          <strong>Color:</strong> {{ car.color || "No description provided." }}
        </p>
      </v-card-text>
    </v-card>
    <v-alert v-else type="info">Loading car data...</v-alert>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { CarListViewModel, CarViewModel } from "@/viewmodels.g";

const car = ref<CarViewModel | null>(null);
const route = useRoute();

onMounted(async () => {
  const carId = Number(route.params.id);
  if (isNaN(carId)) return;

  const carList = new CarListViewModel();
  await carList.$load();

  car.value = carList.$items.find((c) => c.carId === carId) || null;
});
</script>
