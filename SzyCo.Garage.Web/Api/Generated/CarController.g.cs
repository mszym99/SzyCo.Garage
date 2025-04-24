
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
    [Route("api/Car")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class CarController
        : BaseApiController<SzyCo.Cars.Data.Models.Car, CarParameter, CarResponse, SzyCo.Garage.Data.AppDbContext>
    {
        public CarController(CrudContext<SzyCo.Garage.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<SzyCo.Cars.Data.Models.Car>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CarResponse>> Get(
            int id,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<CarResponse>> List(
            [FromQuery] ListParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            [FromQuery] FilterParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Consumes("application/x-www-form-urlencoded", "multipart/form-data")]
        [Authorize]
        public virtual Task<ItemResult<CarResponse>> Save(
            [FromForm] CarParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource,
            IBehaviors<SzyCo.Cars.Data.Models.Car> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("save")]
        [Consumes("application/json")]
        [Authorize]
        public virtual Task<ItemResult<CarResponse>> SaveFromJson(
            [FromBody] CarParameter dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource,
            IBehaviors<SzyCo.Cars.Data.Models.Car> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<CarResponse>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSource, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CarResponse>> Delete(
            int id,
            IBehaviors<SzyCo.Cars.Data.Models.Car> behaviors,
            IDataSource<SzyCo.Cars.Data.Models.Car> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
