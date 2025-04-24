<template>
  <v-avatar color="white">
    <span v-if="!imageLoaded && initials" class="initials">
      {{ initials }}
    </span>
    <v-icon
      v-if="!imageLoaded && !initials"
      icon="fa fa-user text-grey-darken-2"
    ></v-icon>
  </v-avatar>
</template>

<script setup lang="ts">
import { User, UserInfo } from "@/models.g";

const props = defineProps<{
  user: User | UserInfo;
}>();

const initials = computed(() => {
  const fullName = props.user.fullName;
  if (!fullName) return "";

  const firstInitialMatch = /, (.)/.exec(fullName);
  if (firstInitialMatch) {
    return firstInitialMatch[1] + fullName.charAt(0);
  } else {
    const lastInitialMatch = / ([^ ])[^ ]+$/.exec(fullName);
    if (lastInitialMatch) {
      return fullName.charAt(0) + lastInitialMatch[1];
    } else {
      return fullName.substr(0, 2);
    }
  }
});

const imageLoaded = ref(false);
</script>
