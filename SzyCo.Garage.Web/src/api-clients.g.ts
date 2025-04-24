import * as $metadata from './metadata.g'
import * as $models from './models.g'
import { ModelApiClient, ServiceApiClient } from 'coalesce-vue/lib/api-client'
import type { AxiosPromise, AxiosRequestConfig, ItemResult, ListResult } from 'coalesce-vue/lib/api-client'

export class AuditLogApiClient extends ModelApiClient<$models.AuditLog> {
  constructor() { super($metadata.AuditLog) }
}


export class AuditLogPropertyApiClient extends ModelApiClient<$models.AuditLogProperty> {
  constructor() { super($metadata.AuditLogProperty) }
}


export class CarApiClient extends ModelApiClient<$models.Car> {
  constructor() { super($metadata.Car) }
}


export class RoleApiClient extends ModelApiClient<$models.Role> {
  constructor() { super($metadata.Role) }
}


export class UserApiClient extends ModelApiClient<$models.User> {
  constructor() { super($metadata.User) }
  public setEmail(id: string | null, newEmail: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.setEmail
    const $params =  {
      id,
      newEmail,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public sendEmailConfirmation(id: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.sendEmailConfirmation
    const $params =  {
      id,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public setPassword(id: string | null, currentPassword: string | null, newPassword: string | null, confirmNewPassword: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.setPassword
    const $params =  {
      id,
      currentPassword,
      newPassword,
      confirmNewPassword,
    }
    return this.$invoke($method, $params, $config)
  }
  
}


export class UserRoleApiClient extends ModelApiClient<$models.UserRole> {
  constructor() { super($metadata.UserRole) }
}


export class WidgetApiClient extends ModelApiClient<$models.Widget> {
  constructor() { super($metadata.Widget) }
}


export class CarServiceApiClient extends ServiceApiClient<typeof $metadata.CarService> {
  constructor() { super($metadata.CarService) }
}


export class SecurityServiceApiClient extends ServiceApiClient<typeof $metadata.SecurityService> {
  constructor() { super($metadata.SecurityService) }
  public whoAmI($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.UserInfo>> {
    const $method = this.$metadata.methods.whoAmI
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


