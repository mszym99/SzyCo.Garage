import { getEnumMeta, solidify } from 'coalesce-vue/lib/metadata'
import type {
  Domain, ModelType, ObjectType, HiddenAreas, BehaviorFlags, 
  PrimitiveProperty, ForeignKeyProperty, PrimaryKeyProperty,
  ModelCollectionNavigationProperty, ModelReferenceNavigationProperty
} from 'coalesce-vue/lib/metadata'


const domain: Domain = { enums: {}, types: {}, services: {} }
export const AuditEntryState = domain.enums.AuditEntryState = {
  name: "AuditEntryState" as const,
  displayName: "Audit Entry State",
  type: "enum",
  ...getEnumMeta<"EntityAdded"|"EntityDeleted"|"EntityModified">([
  {
    value: 0,
    strValue: "EntityAdded",
    displayName: "Entity Added",
  },
  {
    value: 1,
    strValue: "EntityDeleted",
    displayName: "Entity Deleted",
  },
  {
    value: 2,
    strValue: "EntityModified",
    displayName: "Entity Modified",
  },
  ]),
}
export const Permission = domain.enums.Permission = {
  name: "Permission" as const,
  displayName: "Permission",
  type: "enum",
  ...getEnumMeta<"Admin"|"UserAdmin"|"ViewAuditLogs">([
  {
    value: 1,
    strValue: "Admin",
    displayName: "Admin - General",
    description: "Modify application configuration and other administrative functions excluding user/role management.",
  },
  {
    value: 2,
    strValue: "UserAdmin",
    displayName: "Admin - Users",
    description: "Add and modify users accounts and their assigned roles. Edit roles and their permissions.",
  },
  {
    value: 3,
    strValue: "ViewAuditLogs",
    displayName: "View Audit Logs",
  },
  ]),
}
export const WidgetCategory = domain.enums.WidgetCategory = {
  name: "WidgetCategory" as const,
  displayName: "Widget Category",
  type: "enum",
  ...getEnumMeta<"Whizbangs"|"Sprecklesprockets"|"Discombobulators">([
  {
    value: 0,
    strValue: "Whizbangs",
    displayName: "Whizbangs",
  },
  {
    value: 1,
    strValue: "Sprecklesprockets",
    displayName: "Sprecklesprockets",
  },
  {
    value: 2,
    strValue: "Discombobulators",
    displayName: "Discombobulators",
  },
  ]),
}
export const AuditLog = domain.types.AuditLog = {
  name: "AuditLog" as const,
  displayName: "Audit Log",
  get displayProp() { return this.props.type }, 
  type: "model",
  controllerRoute: "AuditLog",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 0 as BehaviorFlags,
  props: {
    userId: {
      name: "userId",
      displayName: "User Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.User as ModelType & { name: "User" }) },
      get navigationProp() { return (domain.types.AuditLog as ModelType & { name: "AuditLog" }).props.user as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
    },
    user: {
      name: "user",
      displayName: "Changed By",
      type: "model",
      get typeDef() { return (domain.types.User as ModelType & { name: "User" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.AuditLog as ModelType & { name: "AuditLog" }).props.userId as ForeignKeyProperty },
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      dontSerialize: true,
    },
    id: {
      name: "id",
      displayName: "Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    type: {
      name: "type",
      displayName: "Type",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Type is required.",
        maxLength: val => !val || val.length <= 100 || "Type may not be more than 100 characters.",
      }
    },
    keyValue: {
      name: "keyValue",
      displayName: "Key Value",
      type: "string",
      role: "value",
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    state: {
      name: "state",
      displayName: "Change Type",
      type: "enum",
      get typeDef() { return AuditEntryState },
      role: "value",
    },
    date: {
      name: "date",
      displayName: "Date",
      type: "date",
      dateKind: "datetime",
      role: "value",
    },
    properties: {
      name: "properties",
      displayName: "Properties",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.AuditLogProperty as ModelType & { name: "AuditLogProperty" }) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.AuditLogProperty as ModelType & { name: "AuditLogProperty" }).props.parentId as ForeignKeyProperty },
      dontSerialize: true,
    },
    clientIp: {
      name: "clientIp",
      displayName: "Client IP",
      type: "string",
      role: "value",
    },
    referrer: {
      name: "referrer",
      displayName: "Referrer",
      type: "string",
      role: "value",
    },
    endpoint: {
      name: "endpoint",
      displayName: "Endpoint",
      type: "string",
      role: "value",
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const AuditLogProperty = domain.types.AuditLogProperty = {
  name: "AuditLogProperty" as const,
  displayName: "Audit Log Property",
  get displayProp() { return this.props.propertyName }, 
  type: "model",
  controllerRoute: "AuditLogProperty",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 0 as BehaviorFlags,
  props: {
    id: {
      name: "id",
      displayName: "Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    parentId: {
      name: "parentId",
      displayName: "Parent Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.AuditLog as ModelType & { name: "AuditLog" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.AuditLog as ModelType & { name: "AuditLog" }) },
      rules: {
        required: val => val != null || "Parent Id is required.",
      }
    },
    propertyName: {
      name: "propertyName",
      displayName: "Property Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Property Name is required.",
        maxLength: val => !val || val.length <= 100 || "Property Name may not be more than 100 characters.",
      }
    },
    oldValue: {
      name: "oldValue",
      displayName: "Old Value",
      type: "string",
      role: "value",
    },
    oldValueDescription: {
      name: "oldValueDescription",
      displayName: "Old Value Description",
      type: "string",
      role: "value",
    },
    newValue: {
      name: "newValue",
      displayName: "New Value",
      type: "string",
      role: "value",
    },
    newValueDescription: {
      name: "newValueDescription",
      displayName: "New Value Description",
      type: "string",
      role: "value",
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Car = domain.types.Car = {
  name: "Car" as const,
  displayName: "Car",
  get displayProp() { return this.props.carId }, 
  type: "model",
  controllerRoute: "Car",
  get keyProp() { return this.props.carId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    carId: {
      name: "carId",
      displayName: "Car Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    userId: {
      name: "userId",
      displayName: "User Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.User as ModelType & { name: "User" }) },
      get navigationProp() { return (domain.types.Car as ModelType & { name: "Car" }).props.user as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => (val != null && val !== '') || "User is required.",
      }
    },
    user: {
      name: "user",
      displayName: "User",
      type: "model",
      get typeDef() { return (domain.types.User as ModelType & { name: "User" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Car as ModelType & { name: "Car" }).props.userId as ForeignKeyProperty },
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      dontSerialize: true,
    },
    year: {
      name: "year",
      displayName: "Year",
      type: "number",
      role: "value",
      rules: {
        required: val => val != null || "Year is required.",
      }
    },
    make: {
      name: "make",
      displayName: "Make",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Make is required.",
      }
    },
    model: {
      name: "model",
      displayName: "Model",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Model is required.",
      }
    },
    color: {
      name: "color",
      displayName: "Color",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Color is required.",
      }
    },
  },
  methods: {
  },
  dataSources: {
    myGarage: {
      type: "dataSource",
      name: "MyGarage" as const,
      displayName: "My Garage",
      isDefault: true,
      props: {
      },
    },
  },
}
export const Role = domain.types.Role = {
  name: "Role" as const,
  displayName: "Role",
  description: "Roles are groups of permissions, analogous to job titles or functions.",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Role",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    permissions: {
      name: "permissions",
      displayName: "Permissions",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "enum",
        get typeDef() { return Permission },
      },
      role: "value",
    },
    id: {
      name: "id",
      displayName: "Id",
      type: "string",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const User = domain.types.User = {
  name: "User" as const,
  displayName: "User",
  description: "A user profile within the application.",
  get displayProp() { return this.props.fullName }, 
  type: "model",
  controllerRoute: "User",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 2 as BehaviorFlags,
  props: {
    fullName: {
      name: "fullName",
      displayName: "Full Name",
      type: "string",
      role: "value",
    },
    userName: {
      name: "userName",
      displayName: "User Name",
      type: "string",
      role: "value",
    },
    email: {
      name: "email",
      displayName: "Email",
      type: "string",
      role: "value",
      dontSerialize: true,
    },
    emailConfirmed: {
      name: "emailConfirmed",
      displayName: "Email Confirmed",
      type: "boolean",
      role: "value",
      dontSerialize: true,
    },
    lockoutEnd: {
      name: "lockoutEnd",
      displayName: "Lockout End",
      description: "If set, the user will be blocked from signing in until this date.",
      type: "date",
      dateKind: "datetime",
      role: "value",
    },
    lockoutEnabled: {
      name: "lockoutEnabled",
      displayName: "Lockout Enabled",
      description: "If enabled, the user can be locked out.",
      type: "boolean",
      role: "value",
    },
    userRoles: {
      name: "userRoles",
      displayName: "User Roles",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.UserRole as ModelType & { name: "UserRole" }) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.userId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.user as ModelReferenceNavigationProperty },
      manyToMany: {
        name: "roles",
        displayName: "Roles",
        get typeDef() { return (domain.types.Role as ModelType & { name: "Role" }) },
        get farForeignKey() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.roleId as ForeignKeyProperty },
        get farNavigationProp() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.role as ModelReferenceNavigationProperty },
        get nearForeignKey() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.userId as ForeignKeyProperty },
        get nearNavigationProp() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.user as ModelReferenceNavigationProperty },
      },
      hidden: 3 as HiddenAreas,
      dontSerialize: true,
    },
    roleNames: {
      name: "roleNames",
      displayName: "Roles",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "string",
      },
      role: "value",
      dontSerialize: true,
    },
    id: {
      name: "id",
      displayName: "Id",
      type: "string",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
  },
  methods: {
    setEmail: {
      name: "setEmail",
      displayName: "Set Email",
      transportType: "item",
      httpMethod: "POST",
      params: {
        id: {
          name: "id",
          displayName: "Primary Key",
          type: "string",
          role: "value",
          get source() { return (domain.types.User as ModelType & { name: "User" }).props.id },
          rules: {
            required: val => (val != null && val !== '') || "Primary Key is required.",
          }
        },
        newEmail: {
          name: "newEmail",
          displayName: "New Email",
          type: "string",
          subtype: "email",
          role: "value",
          rules: {
            required: val => (val != null && val !== '') || "New Email is required.",
          }
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    sendEmailConfirmation: {
      name: "sendEmailConfirmation",
      displayName: "Send Email Confirmation",
      transportType: "item",
      httpMethod: "POST",
      params: {
        id: {
          name: "id",
          displayName: "Primary Key",
          type: "string",
          role: "value",
          get source() { return (domain.types.User as ModelType & { name: "User" }).props.id },
          rules: {
            required: val => (val != null && val !== '') || "Primary Key is required.",
          }
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    setPassword: {
      name: "setPassword",
      displayName: "Set Password",
      transportType: "item",
      httpMethod: "POST",
      params: {
        id: {
          name: "id",
          displayName: "Primary Key",
          type: "string",
          role: "value",
          get source() { return (domain.types.User as ModelType & { name: "User" }).props.id },
          rules: {
            required: val => (val != null && val !== '') || "Primary Key is required.",
          }
        },
        currentPassword: {
          name: "currentPassword",
          displayName: "Current Password",
          type: "string",
          subtype: "password",
          role: "value",
        },
        newPassword: {
          name: "newPassword",
          displayName: "New Password",
          type: "string",
          subtype: "password",
          role: "value",
          rules: {
            required: val => (val != null && val !== '') || "New Password is required.",
          }
        },
        confirmNewPassword: {
          name: "confirmNewPassword",
          displayName: "Confirm New Password",
          type: "string",
          subtype: "password",
          role: "value",
          rules: {
            required: val => (val != null && val !== '') || "Confirm New Password is required.",
          }
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
  },
  dataSources: {
    defaultSource: {
      type: "dataSource",
      name: "DefaultSource" as const,
      displayName: "Default Source",
      isDefault: true,
      props: {
      },
    },
  },
}
export const UserRole = domain.types.UserRole = {
  name: "UserRole" as const,
  displayName: "User Role",
  get displayProp() { return this.props.id }, 
  type: "model",
  controllerRoute: "UserRole",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 5 as BehaviorFlags,
  props: {
    id: {
      name: "id",
      displayName: "Id",
      type: "string",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    user: {
      name: "user",
      displayName: "User",
      type: "model",
      get typeDef() { return (domain.types.User as ModelType & { name: "User" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.userId as ForeignKeyProperty },
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.User as ModelType & { name: "User" }).props.userRoles as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    role: {
      name: "role",
      displayName: "Role",
      type: "model",
      get typeDef() { return (domain.types.Role as ModelType & { name: "Role" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.roleId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Role as ModelType & { name: "Role" }).props.id as PrimaryKeyProperty },
      dontSerialize: true,
    },
    userId: {
      name: "userId",
      displayName: "User Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.User as ModelType & { name: "User" }) },
      get navigationProp() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.user as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => (val != null && val !== '') || "User is required.",
      }
    },
    roleId: {
      name: "roleId",
      displayName: "Role Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.Role as ModelType & { name: "Role" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.Role as ModelType & { name: "Role" }) },
      get navigationProp() { return (domain.types.UserRole as ModelType & { name: "UserRole" }).props.role as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      rules: {
        required: val => (val != null && val !== '') || "Role is required.",
      }
    },
  },
  methods: {
  },
  dataSources: {
    defaultSource: {
      type: "dataSource",
      name: "DefaultSource" as const,
      displayName: "Default Source",
      isDefault: true,
      props: {
      },
    },
  },
}
export const Widget = domain.types.Widget = {
  name: "Widget" as const,
  displayName: "Widget",
  description: "A sample model provided by the Coalesce template. Remove this when you start building your real data model.",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Widget",
  get keyProp() { return this.props.widgetId }, 
  behaviorFlags: 7 as BehaviorFlags,
  props: {
    widgetId: {
      name: "widgetId",
      displayName: "Widget Id",
      type: "number",
      role: "primaryKey",
      hidden: 3 as HiddenAreas,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
      rules: {
        required: val => (val != null && val !== '') || "Name is required.",
      }
    },
    category: {
      name: "category",
      displayName: "Category",
      type: "enum",
      get typeDef() { return WidgetCategory },
      role: "value",
      rules: {
        required: val => val != null || "Category is required.",
      }
    },
    inventedOn: {
      name: "inventedOn",
      displayName: "Invented On",
      type: "date",
      dateKind: "datetime",
      role: "value",
    },
    modifiedById: {
      name: "modifiedById",
      displayName: "Modified By Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.User as ModelType & { name: "User" }) },
      get navigationProp() { return (domain.types.Widget as ModelType & { name: "Widget" }).props.modifiedBy as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      dontSerialize: true,
    },
    createdById: {
      name: "createdById",
      displayName: "Created By Id",
      type: "string",
      role: "foreignKey",
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      get principalType() { return (domain.types.User as ModelType & { name: "User" }) },
      get navigationProp() { return (domain.types.Widget as ModelType & { name: "Widget" }).props.createdBy as ModelReferenceNavigationProperty },
      hidden: 3 as HiddenAreas,
      dontSerialize: true,
    },
    createdBy: {
      name: "createdBy",
      displayName: "Created By",
      type: "model",
      get typeDef() { return (domain.types.User as ModelType & { name: "User" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Widget as ModelType & { name: "Widget" }).props.createdById as ForeignKeyProperty },
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      dontSerialize: true,
    },
    createdOn: {
      name: "createdOn",
      displayName: "Created On",
      type: "date",
      dateKind: "datetime",
      role: "value",
      dontSerialize: true,
    },
    modifiedBy: {
      name: "modifiedBy",
      displayName: "Modified By",
      type: "model",
      get typeDef() { return (domain.types.User as ModelType & { name: "User" }) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Widget as ModelType & { name: "Widget" }).props.modifiedById as ForeignKeyProperty },
      get principalKey() { return (domain.types.User as ModelType & { name: "User" }).props.id as PrimaryKeyProperty },
      dontSerialize: true,
    },
    modifiedOn: {
      name: "modifiedOn",
      displayName: "Modified On",
      type: "date",
      dateKind: "datetime",
      role: "value",
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const UserInfo = domain.types.UserInfo = {
  name: "UserInfo" as const,
  displayName: "User Info",
  type: "object",
  props: {
    id: {
      name: "id",
      displayName: "Id",
      type: "string",
      role: "value",
    },
    userName: {
      name: "userName",
      displayName: "User Name",
      type: "string",
      role: "value",
    },
    email: {
      name: "email",
      displayName: "Email",
      type: "string",
      role: "value",
    },
    fullName: {
      name: "fullName",
      displayName: "Full Name",
      type: "string",
      role: "value",
    },
    roles: {
      name: "roles",
      displayName: "Roles",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "string",
      },
      role: "value",
    },
    permissions: {
      name: "permissions",
      displayName: "Permissions",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "string",
      },
      role: "value",
    },
  },
}
export const CarService = domain.services.CarService = {
  name: "CarService",
  displayName: "Car Service",
  type: "service",
  controllerRoute: "CarService",
  methods: {
  },
}
export const SecurityService = domain.services.SecurityService = {
  name: "SecurityService",
  displayName: "Security Service",
  type: "service",
  controllerRoute: "SecurityService",
  methods: {
    whoAmI: {
      name: "whoAmI",
      displayName: "Who AmI",
      transportType: "item",
      httpMethod: "GET",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "object",
        get typeDef() { return (domain.types.UserInfo as ObjectType & { name: "UserInfo" }) },
        role: "value",
      },
    },
  },
}

interface AppDomain extends Domain {
  enums: {
    AuditEntryState: typeof AuditEntryState
    Permission: typeof Permission
    WidgetCategory: typeof WidgetCategory
  }
  types: {
    AuditLog: typeof AuditLog
    AuditLogProperty: typeof AuditLogProperty
    Car: typeof Car
    Role: typeof Role
    User: typeof User
    UserInfo: typeof UserInfo
    UserRole: typeof UserRole
    Widget: typeof Widget
  }
  services: {
    CarService: typeof CarService
    SecurityService: typeof SecurityService
  }
}

solidify(domain)

export default domain as unknown as AppDomain
