namespace SzyCo.Garage.Data.Models;

public class Event
{
    private const string SoldVehicleEventTypeName = "Sold Vehicle";

    private static readonly JsonSerializerOptions JsonDataSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public int Id { get; set; }

    [ForeignKey(nameof(Car))]
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    [ForeignKey(nameof(EventTypeDefinition))]
    public int EventTypeId { get; set; }
    public EventTypeDefinition EventTypeDefinition { get; set; } = null!;

    public string JsonData { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    [NotMapped]
    [Read]
    public decimal Cost => GetCostFromJsonData(JsonData);

    public static decimal GetCostFromJsonData(string? jsonData)
    {
        if (string.IsNullOrWhiteSpace(jsonData)) return 0m;

        try
        {
            var eventData = JsonSerializer.Deserialize<EventJsonData>(jsonData, JsonDataSerializerOptions);
            return eventData?.GetCost() ?? 0m;
        }
        catch (JsonException)
        {
            return 0m;
        }
    }

    private sealed class EventJsonData
    {
        public JsonElement? Cost { get; set; }

        public decimal GetCost()
        {
            if (Cost is not { } cost) return 0m;

            return cost.ValueKind switch
            {
                JsonValueKind.Number when cost.TryGetDecimal(out var value) => value,
                JsonValueKind.String => ParseCost(cost.GetString()),
                _ => 0m,
            };
        }

        private static decimal ParseCost(string? cost)
        {
            if (string.IsNullOrWhiteSpace(cost)) return 0m;

            var normalized = cost.Replace("$", string.Empty).Replace(",", string.Empty);
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
                ? value
                : 0m;
        }
    }

    private static string? ValidateJsonData(string? jsonData, EventTypeDefinition eventTypeDefinition)
    {
        JsonElement root;
        try
        {
            using var document = JsonDocument.Parse(string.IsNullOrWhiteSpace(jsonData) ? "{}" : jsonData);
            root = document.RootElement.Clone();
        }
        catch (JsonException)
        {
            return "Event data must be valid JSON.";
        }

        if (root.ValueKind != JsonValueKind.Object) return "Event data must be a JSON object.";

        foreach (var field in GetEventDefinitionFields(eventTypeDefinition.JsonDefinition))
        {
            if (string.IsNullOrWhiteSpace(field.Name)) continue;

            var hasValue = TryGetProperty(root, field.Name, out var value) && !IsEmptyJsonValue(value);
            if (field.Required && !hasValue)
            {
                return $"{field.DisplayName} is required.";
            }

            if (!hasValue) continue;

            if (field.Options is { Count: > 0 } && !field.Options.Any(o => string.Equals(o, GetJsonValueAsString(value), StringComparison.OrdinalIgnoreCase)))
            {
                return $"{field.DisplayName} must be one of: {string.Join(", ", field.Options)}.";
            }

            var normalizedType = field.Type?.Trim().ToLowerInvariant();
            if ((normalizedType == "number" || normalizedType == "currency") && !TryGetDecimalValue(value, out _))
            {
                return $"{field.DisplayName} must be a number.";
            }

            if (normalizedType == "date" && !TryGetDateValue(value, out _))
            {
                return $"{field.DisplayName} must be a valid date.";
            }
        }

        return null;
    }

    private static IReadOnlyList<EventDefinitionField> GetEventDefinitionFields(string? jsonDefinition)
    {
        if (string.IsNullOrWhiteSpace(jsonDefinition)) return [];

        try
        {
            var definition = JsonSerializer.Deserialize<EventJsonDefinition>(jsonDefinition, JsonDataSerializerOptions);
            return definition?.Fields ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase))
            {
                value = property.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static bool IsEmptyJsonValue(JsonElement value)
        => value.ValueKind switch
        {
            JsonValueKind.Null or JsonValueKind.Undefined => true,
            JsonValueKind.String => string.IsNullOrWhiteSpace(value.GetString()),
            _ => false,
        };

    private static string? GetJsonValueAsString(JsonElement value)
        => value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.Number => value.GetRawText(),
            JsonValueKind.True => bool.TrueString,
            JsonValueKind.False => bool.FalseString,
            _ => value.ToString(),
        };

    private static bool TryGetDecimalValue(JsonElement value, out decimal result)
    {
        if (value.ValueKind == JsonValueKind.Number) return value.TryGetDecimal(out result);

        if (value.ValueKind == JsonValueKind.String)
        {
            var normalized = value.GetString()?.Replace("$", string.Empty).Replace(",", string.Empty);
            return decimal.TryParse(normalized, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
        }

        result = 0m;
        return false;
    }

    private static bool TryGetDateValue(JsonElement value, out DateTime result)
    {
        if (value.ValueKind != JsonValueKind.String)
        {
            result = default;
            return false;
        }

        return DateTime.TryParse(value.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }

    private static bool IsSoldVehicleEventType(EventTypeDefinition eventTypeDefinition)
        => string.Equals(eventTypeDefinition.Name, SoldVehicleEventTypeName, StringComparison.OrdinalIgnoreCase);

    private sealed class EventJsonDefinition
    {
        public List<EventDefinitionField> Fields { get; set; } = [];
    }

    private sealed class EventDefinitionField
    {
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? Type { get; set; }
        public bool Required { get; set; }
        public List<string>? Options { get; set; }

        public string DisplayName => string.IsNullOrWhiteSpace(Label) ? Name ?? "Field" : Label;
    }

    [DefaultDataSource]
    public class MyEvents : AppDataSource<Event>
    {
        public MyEvents(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Event> GetQuery(IDataSourceParameters parameters)
            => Db.Events
                .Include(e => e.EventTypeDefinition)
                .Include(e => e.Car)
                .Where(e => e.Car.UserId == User.GetUserId());
    }

    public class EventBehaviors : AppBehaviors<Event>
    {
        public EventBehaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override ItemResult BeforeSave(SaveKind kind, Event? oldItem, Event item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";

            var car = Db.Cars.FirstOrDefault(c => c.CarId == item.CarId);
            if (car == null) return "Car not found.";
            if (car.UserId != userId) return "You can only add events to your own cars.";
            if (car.IsArchived) return "Sold vehicles are read-only.";

            if (kind == SaveKind.Update && oldItem != null && oldItem.CarId != item.CarId)
            {
                var oldCar = Db.Cars.FirstOrDefault(c => c.CarId == oldItem.CarId);
                if (oldCar?.IsArchived == true) return "Sold vehicles are read-only.";
            }

            var eventType = Db.EventTypeDefinitions.FirstOrDefault(et => et.EventTypeDefinitionId == item.EventTypeId);
            if (eventType == null) return "Event type not found.";
            if (kind == SaveKind.Create && !eventType.IsActive) return "Event type is not active.";

            var validationError = ValidateJsonData(item.JsonData, eventType);
            if (validationError != null) return validationError;

            if (kind == SaveKind.Create)
            {
                item.CreateDate = DateTime.UtcNow;
            }
            item.ModifiedDate = DateTime.UtcNow;

            if (IsSoldVehicleEventType(eventType))
            {
                car.IsArchived = true;
            }

            return base.BeforeSave(kind, oldItem, item);
        }

        public override ItemResult BeforeDelete(Event item)
        {
            var userId = User.GetUserId();
            if (string.IsNullOrEmpty(userId)) return "Authentication required.";

            var car = Db.Cars.FirstOrDefault(c => c.CarId == item.CarId);
            if (car == null || car.UserId != userId) return "You can only delete events for your own cars.";
            if (car.IsArchived) return "Sold vehicles are read-only.";

            return base.BeforeDelete(item);
        }
    }
}
