using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SzyCo.Garage.Web.Models
{
    public partial class EventTypeDefinitionParameter : GeneratedParameterDto<SzyCo.Garage.Data.Models.EventTypeDefinition>
    {
        public EventTypeDefinitionParameter() { }

        private int? _EventTypeDefinitionId;
        private string _Name;
        private string _Description;
        private string _JsonDefinition;
        private bool? _IsActive;

        public int? EventTypeDefinitionId
        {
            get => _EventTypeDefinitionId;
            set { _EventTypeDefinitionId = value; Changed(nameof(EventTypeDefinitionId)); }
        }
        public string Name
        {
            get => _Name;
            set { _Name = value; Changed(nameof(Name)); }
        }
        public string Description
        {
            get => _Description;
            set { _Description = value; Changed(nameof(Description)); }
        }
        public string JsonDefinition
        {
            get => _JsonDefinition;
            set { _JsonDefinition = value; Changed(nameof(JsonDefinition)); }
        }
        public bool? IsActive
        {
            get => _IsActive;
            set { _IsActive = value; Changed(nameof(IsActive)); }
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(SzyCo.Garage.Data.Models.EventTypeDefinition entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(EventTypeDefinitionId))) entity.EventTypeDefinitionId = (EventTypeDefinitionId ?? entity.EventTypeDefinitionId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(JsonDefinition))) entity.JsonDefinition = JsonDefinition;
            if (ShouldMapTo(nameof(IsActive))) entity.IsActive = (IsActive ?? entity.IsActive);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override SzyCo.Garage.Data.Models.EventTypeDefinition MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new SzyCo.Garage.Data.Models.EventTypeDefinition()
            {
                Name = Name,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(EventTypeDefinitionId))) entity.EventTypeDefinitionId = (EventTypeDefinitionId ?? entity.EventTypeDefinitionId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(JsonDefinition))) entity.JsonDefinition = JsonDefinition;
            if (ShouldMapTo(nameof(IsActive))) entity.IsActive = (IsActive ?? entity.IsActive);

            return entity;
        }
    }

    public partial class EventTypeDefinitionResponse : GeneratedResponseDto<SzyCo.Garage.Data.Models.EventTypeDefinition>
    {
        public EventTypeDefinitionResponse() { }

        public int? EventTypeDefinitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string JsonDefinition { get; set; }
        public bool? IsActive { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(SzyCo.Garage.Data.Models.EventTypeDefinition obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.EventTypeDefinitionId = obj.EventTypeDefinitionId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            this.JsonDefinition = obj.JsonDefinition;
            this.IsActive = obj.IsActive;
        }
    }
}
