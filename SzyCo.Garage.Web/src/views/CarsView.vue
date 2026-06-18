<script setup lang="ts">
import { onMounted } from "vue";
import { useTitle } from "@vueuse/core";
import CarForm from "@/components/CarForm.vue";
import CarCard from "@/components/CarTable.vue";
import { CarListViewModel } from "@/viewmodels.g";
import EventForm from "@/components/EventForm.vue";

useTitle("Home");

const carList = new CarListViewModel();

onMounted(() => {
  carList.$load();
});
</script>

<template>
  <div class="home">
    <h1>My Garage</h1>
    <div class="d-flex flex-wrap align-center ga-2 mb-4">
      <CarForm @saved="carList.$load" />
      <EventForm :car-list="carList" />
    </div>

    <div class="car-list mt-6" style="align-items: flex-start">
      <CarCard :cars="carList.$items" />
    </div>
  </div>
</template>
