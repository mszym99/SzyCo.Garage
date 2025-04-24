<template>
  <v-dialog v-model="dialog" max-width="500px">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn
        v-bind="activatorProps"
        color="surface-variant"
        text="Add New Car"
        variant="flat"
      ></v-btn>
    </template>
    <v-card>
      <v-card-title>Add New Car</v-card-title>

      <v-card-text>
        <v-form v-model="valid" ref="form" lazy-validation>
          <v-text-field
            v-model="car.Year"
            label="Year"
            type="number"
            required
          />
          <v-text-field v-model="car.Make" label="Make" required />
          <v-text-field v-model="car.Model" label="Model" required />
          <v-text-field v-model="car.Color" label="Color" required />
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-spacer />
        <v-btn @click="dialog = false">Cancel</v-btn>
        <v-btn :disabled="!valid" @click="saveCar">Save</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { CarViewModel, SecurityServiceViewModel } from "@/viewmodels.g";

const dialog = ref(false); // or control from parent
const valid = ref(false);
const ssvm = new SecurityServiceViewModel();
const UserInfoId = ssvm.whoAmI();

const car = ref({
  CarId: null as number | null,
  UserInfoId: "",
  Year: "",
  Make: "",
  Model: "",
  Color: "",
});

const saveCar = async () => {
  const carVM = new CarViewModel();
  carVM.carId = car.value.CarId;
  carVM.userId = (await UserInfoId).id;
  carVM.year = parseInt(car.value.Year);
  carVM.make = car.value.Make;
  carVM.model = car.value.Model;
  carVM.color = car.value.Color;

  try {
    await carVM.$save();
    console.log("Car saved!");
    dialog.value = false;
  } catch (err) {
    console.error("Error saving car:", err);
  }
};
</script>
