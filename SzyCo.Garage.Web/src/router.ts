import { createRouter, createWebHistory } from "vue-router";
import {
  CAdminEditorPage,
  CAdminTablePage,
  CAdminAuditLogPage,
} from "coalesce-vue-vuetify3";
import { Permission } from "./models.g";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: "/",
      component: () => import("./views/CarsView.vue"),
    },
    {
      path: "/car/:id",
      name: "car",
      component: () => import("@/views/CarDetailsView.vue"),
    },
    {
      path: "/admin",
      component: () => import("./views/AdminView.vue"),
    },
    {
      path: "/user/:id",
      alias: "/admin/User/edit/:id", // Override coalesce admin page
      props: true,
      component: () => import("./views/UserProfileView.vue"),
    },
    {
      path: "/openapi",
      component: () => import("./views/OpenApiView.vue"),
    },

    // Coalesce admin routes
    {
      path: "/admin/:type",
      name: "coalesce-admin-list",
      component: titledAdminPage(CAdminTablePage),
      props: true,
    },
    {
      path: "/admin/:type/edit/:id?",
      name: "coalesce-admin-item",
      component: titledAdminPage(CAdminEditorPage),
      props: true,
    },
    {
      path: "/admin/audit",
      component: titledAdminPage(CAdminAuditLogPage),
      meta: { permissions: [Permission.ViewAuditLogs] },
      props: { type: "AuditLog" },
    },
    {
      name: "error-404",
      path: "/:pathMatch(.*)*",
      component: () => import("@/views/errors/NotFound.vue"),
    },
  ],
});

/** Creates a wrapper component that will pull page title from the
 *  coalesce admin page component and pass it to `useTitle`.
 */
function titledAdminPage<
  T extends
    | typeof CAdminTablePage
    | typeof CAdminEditorPage
    | typeof CAdminAuditLogPage,
>(component: T) {
  return defineComponent({
    setup() {
      const pageRef = ref<InstanceType<T>>();
      useTitle(() => pageRef.value?.pageTitle);
      return { pageRef };
    },
    render() {
      return h(component as any, {
        color: "primary",
        ref: "pageRef",
        ...this.$attrs,
      });
    },
  });
}

export default router;
