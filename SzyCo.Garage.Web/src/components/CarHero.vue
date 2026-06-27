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
import { ref, watch } from "vue";
import { CarViewModel } from "@/viewmodels.g";

const props = withDefaults(
  defineProps<{
    carId: number;
    refreshKey?: number;
  }>(),
  {
    refreshKey: 0,
  },
);

const car = ref<CarViewModel | null>(null);

async function loadCar() {
  if (isNaN(props.carId)) return;

  const carVM = new CarViewModel();
  carVM.carId = props.carId;
  await carVM.$load();
  car.value = carVM;
}

watch(() => [props.carId, props.refreshKey], loadCar, { immediate: true });
</script>
