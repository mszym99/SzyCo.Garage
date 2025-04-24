<template>
  <div class="home">
    <h1>My Garage</h1>
    <CarForm @car-added="carList.$load" />

    <div style="align-items: start">
      <div class="car-list mt-6" style="align-items: flex-start">
        <CarCard :cars="carList.$items"></CarCard>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { useTitle } from "@vueuse/core";
import CarForm from "@/components/CarForm.vue";
import CarCard from "@/components/CarTable.vue";
import { CarListViewModel } from "@/viewmodels.g";

useTitle("Home");

const carList = new CarListViewModel();

onMounted(() => {
  carList.$load();
});
</script>
