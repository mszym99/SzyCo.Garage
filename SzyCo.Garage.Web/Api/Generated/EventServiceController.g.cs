
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
    [Route("api/EventService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class EventServiceController : BaseApiController
    {
        protected SzyCo.Garage.Data.Services.IEventService Service { get; }

        public EventServiceController(CrudContext context, SzyCo.Garage.Data.Services.IEventService service) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<SzyCo.Garage.Data.Services.IEventService>();
            Service = service;
        }

        /// <summary>
        /// Method: CopyEventToTodayAsync
        /// </summary>
        [HttpPost("CopyEventToToday")]
        [HttpPost("CopyEventToTodayAsync")]
        [Authorize]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        public virtual async Task<ItemResult<EventResponse>> CopyEventToToday(
            [FromForm(Name = "eventId")] int eventId)
        {
            var _params = new
            {
                EventId = eventId
            };

            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("CopyEventToTodayAsync"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return new ItemResult<EventResponse>(_validationResult);
            }

            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(Context);
            var _methodResult = await Service.CopyEventToTodayAsync(
                User,
                _params.EventId
            );
            var _result = new ItemResult<EventResponse>(_methodResult);
            _result.Object = Mapper.MapToDto<SzyCo.Garage.Data.Models.Event, EventResponse>(_methodResult.Object, _mappingContext, includeTree ?? _methodResult.IncludeTree);
            return _result;
        }

        public class CopyEventToTodayParameters
        {
            public int EventId { get; set; }
        }

        /// <summary>
        /// Method: CopyEventToTodayAsync
        /// </summary>
        [HttpPost("CopyEventToToday")]
        [HttpPost("CopyEventToTodayAsync")]
        [Authorize]
        [Consumes("application/json")]
        public virtual async Task<ItemResult<EventResponse>> CopyEventToToday(
            [FromBody] CopyEventToTodayParameters _params
        )
        {
            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("CopyEventToTodayAsync"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return new ItemResult<EventResponse>(_validationResult);
            }

            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(Context);
            var _methodResult = await Service.CopyEventToTodayAsync(
                User,
                _params.EventId
            );
            var _result = new ItemResult<EventResponse>(_methodResult);
            _result.Object = Mapper.MapToDto<SzyCo.Garage.Data.Models.Event, EventResponse>(_methodResult.Object, _mappingContext, includeTree ?? _methodResult.IncludeTree);
            return _result;
        }
    }
}
