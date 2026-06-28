<template>
  <v-data-table
    :headers="headers"
    :items="cars"
    item-value="carId"
    class="elevation-1"
    hover
    @click:row="goToCar"
  >
    <template #[`item.make`]="{ item }">
      {{ item.make }}
    </template>
    <template #[`item.model`]="{ item }">
      {{ item.model }}
    </template>
    <template #[`item.year`]="{ item }">
      {{ item.year }}
    </template>
    <template #[`item.color`]="{ item }">
      {{ item.color || "No description provided." }}
    </template>
    <template #[`item.status`]="{ item }">
      <v-chip v-if="item.isArchived" color="info" size="small" variant="tonal">
        Archived
      </v-chip>
      <span v-else>Active</span>
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { defineProps } from "vue";
import { useRouter } from "vue-router";
import { CarViewModel } from "@/viewmodels.g";

const props = defineProps<{
  cars: CarViewModel[];
}>();

const router = useRouter();

const headers = [
  { title: "Make", key: "make" },
  { title: "Model", key: "model" },
  { title: "Year", key: "year" },
  { title: "Color", key: "color" },
  { title: "Status", key: "status", sortable: false },
];

function goToCar(_: MouseEvent, row: { item: CarViewModel }) {
  const car = row.item;
  if (!car?.carId) {
    console.error("No car ID found:", car);
    return;
  }

  router.push({ name: "car", params: { id: car.carId.toString() } });
}
</script>
