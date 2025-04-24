<template>
  <v-container max-width="1000px">
    <v-card>
      <v-card-title>User Profile</v-card-title>
      <c-loader-status
        :loaders="{
          'no-initial-content no-error-content': [user.$load],
          //'': [user.$bulkSave],
        }"
      >
        <v-card-text>
          <c-input :model="user" for="userName"></c-input>
          <c-input :model="user" for="fullName"></c-input>

          <div class="d-flex">
            <c-input
              :model="user"
              for="email"
              readonly
              :hint="
                user.emailConfirmed ? 'Email Verified' : 'Email not verified'
              "
              persistent-hint
              class="mb-3"
            >
              <template #append-inner>
                <v-icon v-if="user.emailConfirmed" color="success">
                  fa fa-check-circle
                </v-icon>
                <v-icon v-else color="warning">
                  fa fa-exclamation-triangle
                </v-icon>
              </template>
            </c-input>

            <div v-if="!user.emailConfirmed">
              <v-btn
                color="success"
                :loading="user.sendEmailConfirmation.isLoading"
                prepend-icon="fa fa-paper-plane"
                class="ml-3"
                @click="user.sendEmailConfirmation()"
              >
                Send Email Confirmation
              </v-btn>
              <c-loader-status
                :loaders="user.sendEmailConfirmation"
                no-progress
              />
            </div>

            <v-dialog v-else width="400px">
              <template #activator="{ props }">
                <v-btn
                  color="primary"
                  variant="text"
                  class="ml-3"
                  v-bind="props"
                >
                  Change Email...
                </v-btn>
              </template>
              <template #default="{ isActive }">
                <v-card title="Change Email">
                  <v-card-text>
                    <c-input
                      :model="user.setEmail"
                      for="newEmail"
                      hide-details="auto"
                    ></c-input>
                    <c-loader-status :loaders="user.setEmail" />
                    <v-alert
                      v-if="user.setEmail.wasSuccessful"
                      :text="user.setEmail.message ?? ''"
                      type="success"
                    ></v-alert>
                  </v-card-text>
                  <v-card-actions>
                    <v-btn
                      v-if="!user.setEmail.wasSuccessful"
                      variant="text"
                      @click="isActive.value = false"
                    >
                      Cancel
                    </v-btn>
                    <v-spacer></v-spacer>
                    <v-btn
                      v-if="user.setEmail.wasSuccessful"
                      @click="
                        isActive.value = false;
                        user.$load();
                      "
                    >
                      Done
                    </v-btn>
                    <v-btn
                      v-else
                      :loading="user.setEmail.isLoading"
                      color="primary"
                      variant="elevated"
                      @click="user.setEmail.invokeWithArgs()"
                    >
                      Update Email
                    </v-btn>
                  </v-card-actions>
                </v-card>
              </template>
            </v-dialog>
          </div>
        </v-card-text>

        <template v-if="isMe">
          <v-card-title> Password </v-card-title>
          <v-card-text>
            <v-dialog width="400px">
              <template #activator="{ props }">
                <v-btn
                  color="primary"
                  variant="text"
                  class="ml-3"
                  v-bind="props"
                >
                  Change Password...
                </v-btn>
              </template>
              <template #default="{ isActive }">
                <v-card title="Change Password">
                  <v-card-text>
                    <c-input
                      :model="user.setPassword"
                      for="currentPassword"
                    ></c-input>
                    <c-input
                      :model="user.setPassword"
                      for="newPassword"
                    ></c-input>
                    <c-input
                      :model="user.setPassword"
                      for="confirmNewPassword"
                    ></c-input>
                    <c-loader-status :loaders="user.setPassword" />
                  </v-card-text>
                  <v-card-actions>
                    <v-btn variant="text" @click="isActive.value = false">
                      Cancel
                    </v-btn>
                    <v-spacer></v-spacer>
                    <v-btn
                      :loading="user.setPassword.isLoading"
                      color="primary"
                      variant="elevated"
                      @click="
                        user.setPassword
                          .invokeWithArgs()
                          .then(() => (isActive.value = false))
                      "
                    >
                      Update Password
                    </v-btn>
                  </v-card-actions>
                </v-card>
              </template>
            </v-dialog>
          </v-card-text>
        </template>

        <template v-if="isUserAdmin">
          <v-card-title> Roles & permissions </v-card-title>
          <v-card-text>
            <v-row>
              <v-col>
                <c-input :model="user" for="userRoles"></c-input>
              </v-col>
              <v-col>
                <h3>Effective Permissions:</h3>
                <div style="max-height: 300px; overflow-y: auto">
                  <div
                    v-for="permission in PermissionMeta.values.map((p) => ({
                      meta: p,
                      roles: user.roles.filter((r) =>
                        r.permissions?.includes(p.value),
                      ),
                    }))"
                    :key="permission.meta.strValue"
                  >
                    <span v-if="permission.roles.length">
                      <v-icon icon="fa fa-check text-success"></v-icon>
                      {{ permission.meta.displayName }}
                      <span class="text-caption text-grey pl-1">
                        (via
                        {{ permission.roles.map((r) => r.name).join(",") }})
                      </span>
                    </span>
                    <span v-else>
                      <v-icon icon="fa fa-times text-error"></v-icon>
                      {{ permission.meta.displayName }}
                    </span>
                  </div>
                </div>
              </v-col>
            </v-row>
          </v-card-text>
        </template>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="success"
            prepend-icon="fa fa-save"
            :loading="user.$bulkSave.isLoading"
            :disabled="!user.$bulkSavePreview().isDirty"
            :variant="!user.$bulkSavePreview().isDirty ? 'text' : 'elevated'"
            @click="user.$bulkSave()"
          >
            Save
          </v-btn>
        </v-card-actions>
      </c-loader-status>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { UserViewModel } from "@/viewmodels.g";
import { Permission } from "@/models.g";
import { Permission as PermissionMeta } from "@/metadata.g";

const router = useRouter();
const { can, userInfo } = useUser();

const props = defineProps<{ id: string }>();
const user = new UserViewModel();

const isUserAdmin = computed(() => can(Permission.UserAdmin));

const isMe = computed(() => props.id == userInfo.value.id);

if (!isUserAdmin.value && !isMe.value) {
  // Non-admins can only view themselves
  router.replace({ name: "error-404" });
} else {
  user.$load(props.id);
}
</script>
