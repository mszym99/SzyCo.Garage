<template>
  <v-data-table
    :headers="headers"
    :items="cars"
    item-value="carId"
    class="elevation-1"
    hover
    @click:row="(event: MouseEvent, row: CarViewModel) => goToCar(row)"
    >
    <template #item.make="{ item }">
      {{ item.make }}
    </template>
    <template #item.model="{ item }">
      {{ item.model }}
    </template>
    <template #item.year="{ item }">
      {{ item.year }}
    </template>
    <template #item.color="{ item }">
      {{ item.color || 'No description provided.' }}
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { defineProps } from 'vue'
import { useRouter } from 'vue-router'
import { CarViewModel } from '@/viewmodels.g'
import Car from '@/views/Car.vue';

const props = defineProps<{
  cars: CarViewModel[]
}>()

const router = useRouter()

const headers = [
  { title: 'Make', key: 'make' },
  { title: 'Model', key: 'model' },
  { title: 'Year', key: 'year' },
  { title: 'Color', key: 'color' }
]

function goToCar(event: { item: CarViewModel }) {
  const id = event.item?.carId
  console.log(id);
  if (id == null) {
    console.error('No car ID found:', event.item)
    return
  }

  router.push({ name: 'car', params: { id: id.toString() } })
}


</script>