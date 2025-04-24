
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Api;
using IntelliTect.Coalesce.Api.Behaviors;
using IntelliTect.Coalesce.Api.Controllers;
using IntelliTect.Coalesce.Api.DataSources;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Mapping.IncludeTrees;
using IntelliTect.Coalesce.Models;
using IntelliTect.Coalesce.TypeDefinition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SzyCo.Garage.Web.Models;

namespace SzyCo.Garage.Web.Api
{
    [Route("api/User")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class UserController
        : BaseApiController<SzyCo.Garage.Data.Models.User, UserParameter, UserResponse, SzyCo.Garage.Data.AppDbContext>
    {
        public UserController(CrudContext<SzyCo.Garage.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<SzyCo.Garage.Data.Models.User>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<UserResponse>> Get(
            string id,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<UserResponse>> List(
            [FromQuery] ListParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            [FromQuery] FilterParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        [Authorize]
        public virtual Task<ItemResult<UserResponse>> Save(
            [FromForm] UserParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource,
            IBehaviors<SzyCo.Garage.Data.Models.User> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("save")]
        [Consumes("application/json")]
        [Authorize]
        public virtual Task<ItemResult<UserResponse>> SaveFromJson(
            [FromBody] UserParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource,
            IBehaviors<SzyCo.Garage.Data.Models.User> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<UserResponse>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.User> dataSource,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSource, dataSourceFactory, behaviorsFactory);

        // Methods from data class exposed through API Controller.

        /// <summary>
        /// Method: SetEmail
        /// </summary>
        [HttpPost("SetEmail")]
        [Authorize]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public virtual async Task<ItemResult> SetEmail(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] SzyCo.Garage.Data.Auth.UserManagementService userService,
            [FromForm(Name = "id")] string id,
            [FromForm(Name = "newEmail")] string newEmail)
        {
            var _params = new
            {
                Id = id,
                NewEmail = newEmail
            };

            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("SetEmail"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return _validationResult;
            }

            var _methodResult = await item.SetEmail(
                userService,
                User,
                _params.NewEmail
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        public class SetEmailParameters
        {
            public string Id { get; set; }
            public string NewEmail { get; set; }
        }

        /// <summary>
        /// Method: SetEmail
        /// </summary>
        [HttpPost("SetEmail")]
        [Authorize]
        [Consumes("application/json")]
        public virtual async Task<ItemResult> SetEmail(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] SzyCo.Garage.Data.Auth.UserManagementService userService,
            [FromBody] SetEmailParameters _params
        )
        {
            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("SetEmail"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return _validationResult;
            }

            var _methodResult = await item.SetEmail(
                userService,
                User,
                _params.NewEmail
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: SendEmailConfirmation
        /// </summary>
        [HttpPost("SendEmailConfirmation")]
        [Authorize]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public virtual async Task<ItemResult> SendEmailConfirmation(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] SzyCo.Garage.Data.Auth.UserManagementService userService,
            [FromForm(Name = "id")] string id)
        {
            var _params = new
            {
                Id = id
            };

            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            var _methodResult = await item.SendEmailConfirmation(
                userService,
                User
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        public class SendEmailConfirmationParameters
        {
            public string Id { get; set; }
        }

        /// <summary>
        /// Method: SendEmailConfirmation
        /// </summary>
        [HttpPost("SendEmailConfirmation")]
        [Authorize]
        [Consumes("application/json")]
        public virtual async Task<ItemResult> SendEmailConfirmation(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] SzyCo.Garage.Data.Auth.UserManagementService userService,
            [FromBody] SendEmailConfirmationParameters _params
        )
        {
            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            var _methodResult = await item.SendEmailConfirmation(
                userService,
                User
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        /// <summary>
        /// Method: SetPassword
        /// </summary>
        [HttpPost("SetPassword")]
        [Authorize]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public virtual async Task<ItemResult> SetPassword(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] Microsoft.AspNetCore.Identity.UserManager<SzyCo.Garage.Data.Models.User> userManager,
            [FromServices] Microsoft.AspNetCore.Identity.SignInManager<SzyCo.Garage.Data.Models.User> signInManager,
            [FromForm(Name = "id")] string id,
            [FromForm(Name = "currentPassword")] string currentPassword,
            [FromForm(Name = "newPassword")] string newPassword,
            [FromForm(Name = "confirmNewPassword")] string confirmNewPassword)
        {
            var _params = new
            {
                Id = id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = confirmNewPassword
            };

            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("SetPassword"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return _validationResult;
            }

            var _methodResult = await item.SetPassword(
                userManager,
                signInManager,
                User,
                _params.CurrentPassword,
                _params.NewPassword,
                _params.ConfirmNewPassword
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }

        public class SetPasswordParameters
        {
            public string Id { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }

        /// <summary>
        /// Method: SetPassword
        /// </summary>
        [HttpPost("SetPassword")]
        [Authorize]
        [Consumes("application/json")]
        public virtual async Task<ItemResult> SetPassword(
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] Microsoft.AspNetCore.Identity.UserManager<SzyCo.Garage.Data.Models.User> userManager,
            [FromServices] Microsoft.AspNetCore.Identity.SignInManager<SzyCo.Garage.Data.Models.User> signInManager,
            [FromBody] SetPasswordParameters _params
        )
        {
            var dataSource = dataSourceFactory.GetDataSource<SzyCo.Garage.Data.Models.User, SzyCo.Garage.Data.Models.User>("Default");
            var itemResult = await dataSource.GetItemAsync(_params.Id, new DataSourceParameters());
            if (!itemResult.WasSuccessful)
            {
                return new ItemResult(itemResult);
            }
            var item = itemResult.Object;
            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("SetPassword"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return _validationResult;
            }

            var _methodResult = await item.SetPassword(
                userManager,
                signInManager,
                User,
                _params.CurrentPassword,
                _params.NewPassword,
                _params.ConfirmNewPassword
            );
            var _result = new ItemResult(_methodResult);
            return _result;
        }
    }
}
