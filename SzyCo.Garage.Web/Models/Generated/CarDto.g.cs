using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SzyCo.Garage.Web.Models
{
    public partial class CarParameter : GeneratedParameterDto<SzyCo.Cars.Data.Models.Car>
    {
        public CarParameter() { }

        private int? _CarId;
        private string _UserId;
        private int? _Year;
        private string _Make;
        private string _Model;
        private string _Color;

        public int? CarId
        {
            get => _CarId;
            set { _CarId = value; Changed(nameof(CarId)); }
        }
        public string UserId
        {
            get => _UserId;
            set { _UserId = value; Changed(nameof(UserId)); }
        }
        public int? Year
        {
            get => _Year;
            set { _Year = value; Changed(nameof(Year)); }
        }
        public string Make
        {
            get => _Make;
            set { _Make = value; Changed(nameof(Make)); }
        }
        public string Model
        {
            get => _Model;
            set { _Model = value; Changed(nameof(Model)); }
        }
        public string Color
        {
            get => _Color;
            set { _Color = value; Changed(nameof(Color)); }
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(SzyCo.Cars.Data.Models.Car entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(CarId))) entity.CarId = (CarId ?? entity.CarId);
            if (ShouldMapTo(nameof(UserId))) entity.UserId = UserId;
            if (ShouldMapTo(nameof(Year))) entity.Year = (Year ?? entity.Year);
            if (ShouldMapTo(nameof(Make))) entity.Make = Make;
            if (ShouldMapTo(nameof(Model))) entity.Model = Model;
            if (ShouldMapTo(nameof(Color))) entity.Color = Color;
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override SzyCo.Cars.Data.Models.Car MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new SzyCo.Cars.Data.Models.Car()
            {
                Year = (Year ?? default),
                Make = Make,
                Model = Model,
                Color = Color,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(CarId))) entity.CarId = (CarId ?? entity.CarId);
            if (ShouldMapTo(nameof(UserId))) entity.UserId = UserId;

            return entity;
        }
    }

    public partial class CarResponse : GeneratedResponseDto<SzyCo.Cars.Data.Models.Car>
    {
        public CarResponse() { }

        public int? CarId { get; set; }
        public string UserId { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public SzyCo.Garage.Web.Models.UserResponse User { get; set; }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(SzyCo.Cars.Data.Models.Car obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.CarId = obj.CarId;
            this.UserId = obj.UserId;
            this.Year = obj.Year;
            this.Make = obj.Make;
            this.Model = obj.Model;
            this.Color = obj.Color;
            if (tree == null || tree[nameof(this.User)] != null)
                this.User = obj.User.MapToDto<SzyCo.Garage.Data.Models.User, UserResponse>(context, tree?[nameof(this.User)]);

        }
    }
}
