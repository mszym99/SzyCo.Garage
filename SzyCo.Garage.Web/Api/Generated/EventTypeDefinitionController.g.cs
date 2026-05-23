
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
    [Route("api/EventTypeDefinition")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class EventTypeDefinitionController
        : BaseApiController<SzyCo.Garage.Data.Models.EventTypeDefinition, EventTypeDefinitionParameter, EventTypeDefinitionResponse, SzyCo.Garage.Data.AppDbContext>
    {
        public EventTypeDefinitionController(CrudContext<SzyCo.Garage.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<SzyCo.Garage.Data.Models.EventTypeDefinition>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<EventTypeDefinitionResponse>> Get(
            int id,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<EventTypeDefinitionResponse>> List(
            [FromQuery] ListParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            [FromQuery] FilterParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        [Authorize]
        public virtual Task<ItemResult<EventTypeDefinitionResponse>> Save(
            [FromForm] EventTypeDefinitionParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource,
            IBehaviors<SzyCo.Garage.Data.Models.EventTypeDefinition> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("save")]
        [Consumes("application/json")]
        [Authorize]
        public virtual Task<ItemResult<EventTypeDefinitionResponse>> SaveFromJson(
            [FromBody] EventTypeDefinitionParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource,
            IBehaviors<SzyCo.Garage.Data.Models.EventTypeDefinition> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<EventTypeDefinitionResponse>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSource, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<EventTypeDefinitionResponse>> Delete(
            int id,
            IBehaviors<SzyCo.Garage.Data.Models.EventTypeDefinition> behaviors,
            IDataSource<SzyCo.Garage.Data.Models.EventTypeDefinition> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
