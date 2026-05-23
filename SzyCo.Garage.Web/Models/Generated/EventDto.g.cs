using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SzyCo.Garage.Web.Models
{
    public partial class EventParameter : GeneratedParameterDto<SzyCo.Garage.Data.Models.Event>
    {
        public EventParameter() { }

        private int? _Id;
        private int? _CarId;
        private int? _EventTypeId;
        private string _JsonData;
        private System.DateTime? _CreateDate;
        private System.DateTime? _ModifiedDate;

        public int? Id
        {
            get => _Id;
            set { _Id = value; Changed(nameof(Id)); }
        }
        public int? CarId
        {
            get => _CarId;
            set { _CarId = value; Changed(nameof(CarId)); }
        }
        public int? EventTypeId
        {
            get => _EventTypeId;
            set { _EventTypeId = value; Changed(nameof(EventTypeId)); }
        }
        public string JsonData
        {
            get => _JsonData;
            set { _JsonData = value; Changed(nameof(JsonData)); }
        }
        public System.DateTime? CreateDate
        {
            get => _CreateDate;
            set { _CreateDate = value; Changed(nameof(CreateDate)); }
        }
        public System.DateTime? ModifiedDate
        {
            get => _ModifiedDate;
            set { _ModifiedDate = value; Changed(nameof(ModifiedDate)); }
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(SzyCo.Garage.Data.Models.Event entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(Id))) entity.Id = (Id ?? entity.Id);
            if (ShouldMapTo(nameof(CarId))) entity.CarId = (CarId ?? entity.CarId);
            if (ShouldMapTo(nameof(EventTypeId))) entity.EventTypeId = (EventTypeId ?? entity.EventTypeId);
            if (ShouldMapTo(nameof(JsonData))) entity.JsonData = JsonData;
            if (ShouldMapTo(nameof(CreateDate))) entity.CreateDate = (CreateDate ?? entity.CreateDate);
            if (ShouldMapTo(nameof(ModifiedDate))) entity.ModifiedDate = (ModifiedDate ?? entity.ModifiedDate);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override SzyCo.Garage.Data.Models.Event MapToNew(IMappingContext context)
        {
            var entity = new SzyCo.Garage.Data.Models.Event();
            MapTo(entity, context);
            return entity;
        }
    }

    public partial class EventResponse : GeneratedResponseDto<SzyCo.Garage.Data.Models.Event>
    {
        public EventResponse() { }

        public int? Id { get; set; }
        public int? CarId { get; set; }
        public int? EventTypeId { get; set; }
        public string JsonData { get; set; }
        public System.DateTime? CreateDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public SzyCo.Garage.Web.Models.CarResponse Car { get; set; }
        public SzyCo.Garage.Web.Models.EventTypeDefinitionResponse EventTypeDefinition { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(SzyCo.Garage.Data.Models.Event obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.Id = obj.Id;
            this.CarId = obj.CarId;
            this.EventTypeId = obj.EventTypeId;
            this.JsonData = obj.JsonData;
            this.CreateDate = obj.CreateDate;
            this.ModifiedDate = obj.ModifiedDate;
            if (tree == null || tree[nameof(this.Car)] != null)
                this.Car = obj.Car.MapToDto<SzyCo.Garage.Data.Models.Car, CarResponse>(context, tree?[nameof(this.Car)]);

            if (tree == null || tree[nameof(this.EventTypeDefinition)] != null)
                this.EventTypeDefinition = obj.EventTypeDefinition.MapToDto<SzyCo.Garage.Data.Models.EventTypeDefinition, EventTypeDefinitionResponse>(context, tree?[nameof(this.EventTypeDefinition)]);

        }
    }
}
