<template>
  <v-container class="pa-6">
    <v-btn @click="goBack" prepend-icon="mdi-arrow-left" class="mb-4" color="primary" variant="outlined">
      Back to Car List
    </v-btn>

    <h2 class="text-h5 mb-4">Car Details</h2>
    <CarHero :carId="carId" />
  </v-container>
</template>

<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router'
import CarHero from '@/components/CarHero.vue'

const route = useRoute()
const router = useRouter()
const carId = Number(route.params.id)

function goBack() {
  router.back()
}
</script>
