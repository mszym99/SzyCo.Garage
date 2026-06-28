import * as metadata from './metadata.g'
import { convertToModel, mapToModel, reactiveDataSource } from 'coalesce-vue/lib/model'
import type { Model, DataSource } from 'coalesce-vue/lib/model'

export enum AuditEntryState {
  EntityAdded = 0,
  EntityDeleted = 1,
  EntityModified = 2,
}


export enum Permission {
  
  /** Modify application configuration and other administrative functions excluding user/role management. */
  Admin = 1,
  
  /** Add and modify users accounts and their assigned roles. Edit roles and their permissions. */
  UserAdmin = 2,
  ViewAuditLogs = 3,
}


export interface AuditLog extends Model<typeof metadata.AuditLog> {
  userId: string | null
  user: User | null
  id: number | null
  type: string | null
  keyValue: string | null
  description: string | null
  state: AuditEntryState | null
  date: Date | null
  properties: AuditLogProperty[] | null
  clientIp: string | null
  referrer: string | null
  endpoint: string | null
}
export class AuditLog {
  
  /** Mutates the input object and its descendents into a valid AuditLog implementation. */
  static convert(data?: Partial<AuditLog>): AuditLog {
    return convertToModel(data || {}, metadata.AuditLog) 
  }
  
  /** Maps the input object and its descendents to a new, valid AuditLog implementation. */
  static map(data?: Partial<AuditLog>): AuditLog {
    return mapToModel(data || {}, metadata.AuditLog) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.AuditLog; }
  
  /** Instantiate a new AuditLog, optionally basing it on the given data. */
  constructor(data?: Partial<AuditLog> | {[k: string]: any}) {
    Object.assign(this, AuditLog.map(data || {}));
  }
}


export interface AuditLogProperty extends Model<typeof metadata.AuditLogProperty> {
  id: number | null
  parentId: number | null
  propertyName: string | null
  oldValue: string | null
  oldValueDescription: string | null
  newValue: string | null
  newValueDescription: string | null
}
export class AuditLogProperty {
  
  /** Mutates the input object and its descendents into a valid AuditLogProperty implementation. */
  static convert(data?: Partial<AuditLogProperty>): AuditLogProperty {
    return convertToModel(data || {}, metadata.AuditLogProperty) 
  }
  
  /** Maps the input object and its descendents to a new, valid AuditLogProperty implementation. */
  static map(data?: Partial<AuditLogProperty>): AuditLogProperty {
    return mapToModel(data || {}, metadata.AuditLogProperty) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.AuditLogProperty; }
  
  /** Instantiate a new AuditLogProperty, optionally basing it on the given data. */
  constructor(data?: Partial<AuditLogProperty> | {[k: string]: any}) {
    Object.assign(this, AuditLogProperty.map(data || {}));
  }
}


export interface Car extends Model<typeof metadata.Car> {
  carId: number | null
  userId: string | null
  user: User | null
  year: number | null
  make: string | null
  model: string | null
  color: string | null
  isArchived: boolean | null
  events: Event[] | null
  totalEventHistoryCost: number | null
}
export class Car {
  
  /** Mutates the input object and its descendents into a valid Car implementation. */
  static convert(data?: Partial<Car>): Car {
    return convertToModel(data || {}, metadata.Car) 
  }
  
  /** Maps the input object and its descendents to a new, valid Car implementation. */
  static map(data?: Partial<Car>): Car {
    return mapToModel(data || {}, metadata.Car) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.Car; }
  
  /** Instantiate a new Car, optionally basing it on the given data. */
  constructor(data?: Partial<Car> | {[k: string]: any}) {
    Object.assign(this, Car.map(data || {}));
  }
}
export namespace Car {
  export namespace DataSources {
    
    export class MyGarage implements DataSource<typeof metadata.Car.dataSources.myGarage> {
      readonly $metadata = metadata.Car.dataSources.myGarage
    }
  }
}


export interface Event extends Model<typeof metadata.Event> {
  id: number | null
  carId: number | null
  car: Car | null
  eventTypeId: number | null
  eventTypeDefinition: EventTypeDefinition | null
  jsonData: string | null
  createDate: Date | null
  modifiedDate: Date | null
  cost: number | null
}
export class Event {
  
  /** Mutates the input object and its descendents into a valid Event implementation. */
  static convert(data?: Partial<Event>): Event {
    return convertToModel(data || {}, metadata.Event) 
  }
  
  /** Maps the input object and its descendents to a new, valid Event implementation. */
  static map(data?: Partial<Event>): Event {
    return mapToModel(data || {}, metadata.Event) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.Event; }
  
  /** Instantiate a new Event, optionally basing it on the given data. */
  constructor(data?: Partial<Event> | {[k: string]: any}) {
    Object.assign(this, Event.map(data || {}));
  }
}
export namespace Event {
  export namespace DataSources {
    
    export class MyEvents implements DataSource<typeof metadata.Event.dataSources.myEvents> {
      readonly $metadata = metadata.Event.dataSources.myEvents
    }
  }
}


export interface EventTypeDefinition extends Model<typeof metadata.EventTypeDefinition> {
  eventTypeDefinitionId: number | null
  name: string | null
  description: string | null
  jsonDefinition: string | null
  isActive: boolean | null
}
export class EventTypeDefinition {
  
  /** Mutates the input object and its descendents into a valid EventTypeDefinition implementation. */
  static convert(data?: Partial<EventTypeDefinition>): EventTypeDefinition {
    return convertToModel(data || {}, metadata.EventTypeDefinition) 
  }
  
  /** Maps the input object and its descendents to a new, valid EventTypeDefinition implementation. */
  static map(data?: Partial<EventTypeDefinition>): EventTypeDefinition {
    return mapToModel(data || {}, metadata.EventTypeDefinition) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.EventTypeDefinition; }
  
  /** Instantiate a new EventTypeDefinition, optionally basing it on the given data. */
  constructor(data?: Partial<EventTypeDefinition> | {[k: string]: any}) {
    Object.assign(this, EventTypeDefinition.map(data || {}));
  }
}


export interface Role extends Model<typeof metadata.Role> {
  name: string | null
  permissions: Permission[] | null
  id: string | null
}
export class Role {
  
  /** Mutates the input object and its descendents into a valid Role implementation. */
  static convert(data?: Partial<Role>): Role {
    return convertToModel(data || {}, metadata.Role) 
  }
  
  /** Maps the input object and its descendents to a new, valid Role implementation. */
  static map(data?: Partial<Role>): Role {
    return mapToModel(data || {}, metadata.Role) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.Role; }
  
  /** Instantiate a new Role, optionally basing it on the given data. */
  constructor(data?: Partial<Role> | {[k: string]: any}) {
    Object.assign(this, Role.map(data || {}));
  }
}


export interface User extends Model<typeof metadata.User> {
  fullName: string | null
  userName: string | null
  email: string | null
  emailConfirmed: boolean | null
  
  /** If set, the user will be blocked from signing in until this date. */
  lockoutEnd: Date | null
  
  /** If enabled, the user can be locked out. */
  lockoutEnabled: boolean | null
  userRoles: UserRole[] | null
  roleNames: string[] | null
  id: string | null
}
export class User {
  
  /** Mutates the input object and its descendents into a valid User implementation. */
  static convert(data?: Partial<User>): User {
    return convertToModel(data || {}, metadata.User) 
  }
  
  /** Maps the input object and its descendents to a new, valid User implementation. */
  static map(data?: Partial<User>): User {
    return mapToModel(data || {}, metadata.User) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.User; }
  
  /** Instantiate a new User, optionally basing it on the given data. */
  constructor(data?: Partial<User> | {[k: string]: any}) {
    Object.assign(this, User.map(data || {}));
  }
}
export namespace User {
  export namespace DataSources {
    
    export class DefaultSource implements DataSource<typeof metadata.User.dataSources.defaultSource> {
      readonly $metadata = metadata.User.dataSources.defaultSource
    }
  }
}


export interface UserRole extends Model<typeof metadata.UserRole> {
  id: string | null
  user: User | null
  role: Role | null
  userId: string | null
  roleId: string | null
}
export class UserRole {
  
  /** Mutates the input object and its descendents into a valid UserRole implementation. */
  static convert(data?: Partial<UserRole>): UserRole {
    return convertToModel(data || {}, metadata.UserRole) 
  }
  
  /** Maps the input object and its descendents to a new, valid UserRole implementation. */
  static map(data?: Partial<UserRole>): UserRole {
    return mapToModel(data || {}, metadata.UserRole) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.UserRole; }
  
  /** Instantiate a new UserRole, optionally basing it on the given data. */
  constructor(data?: Partial<UserRole> | {[k: string]: any}) {
    Object.assign(this, UserRole.map(data || {}));
  }
}
export namespace UserRole {
  export namespace DataSources {
    
    export class DefaultSource implements DataSource<typeof metadata.UserRole.dataSources.defaultSource> {
      readonly $metadata = metadata.UserRole.dataSources.defaultSource
    }
  }
}


export interface UserInfo extends Model<typeof metadata.UserInfo> {
  id: string | null
  userName: string | null
  email: string | null
  fullName: string | null
  roles: string[] | null
  permissions: string[] | null
}
export class UserInfo {
  
  /** Mutates the input object and its descendents into a valid UserInfo implementation. */
  static convert(data?: Partial<UserInfo>): UserInfo {
    return convertToModel(data || {}, metadata.UserInfo) 
  }
  
  /** Maps the input object and its descendents to a new, valid UserInfo implementation. */
  static map(data?: Partial<UserInfo>): UserInfo {
    return mapToModel(data || {}, metadata.UserInfo) 
  }
  
  static [Symbol.hasInstance](x: any) { return x?.$metadata === metadata.UserInfo; }
  
  /** Instantiate a new UserInfo, optionally basing it on the given data. */
  constructor(data?: Partial<UserInfo> | {[k: string]: any}) {
    Object.assign(this, UserInfo.map(data || {}));
  }
}


declare module "coalesce-vue/lib/model" {
  interface EnumTypeLookup {
    AuditEntryState: AuditEntryState
    Permission: Permission
  }
  interface ModelTypeLookup {
    AuditLog: AuditLog
    AuditLogProperty: AuditLogProperty
    Car: Car
    Event: Event
    EventTypeDefinition: EventTypeDefinition
    Role: Role
    User: User
    UserInfo: UserInfo
    UserRole: UserRole
  }
}
