namespace SzyCo.Garage.Data;

public class DatabaseSeeder(AppDbContext db)
{
    private static readonly EventTypeDefinitionSeed[] DefaultEventTypeDefinitions =
    [
        new(
            "Replacement",
            "A part replacement event",
            """
            {
              "fields": [
                { "name": "partName", "label": "Part Name", "type": "text", "required": true },
                { "name": "reason", "label": "Reason for Replacement", "type": "textarea" },
                { "name": "cost", "label": "Cost", "type": "currency" }
              ]
            }
            """),
        new(
            "Purchased Vehicle",
            "The purchase record for a vehicle",
            """
            {
              "fields": [
                { "name": "purchaseDate", "label": "Purchase Date", "type": "date", "required": true },
                { "name": "seller", "label": "Seller", "type": "text" },
                { "name": "odometer", "label": "Odometer", "type": "number" },
                { "name": "cost", "label": "Purchase Price", "type": "currency" },
                { "name": "notes", "label": "Notes", "type": "textarea" }
              ]
            }
            """),
        new(
            "Sold Vehicle",
            "The sale record that archives a vehicle",
            """
            {
              "fields": [
                { "name": "saleDate", "label": "Sale Date", "type": "date", "required": true },
                { "name": "buyer", "label": "Buyer", "type": "text" },
                { "name": "odometer", "label": "Odometer", "type": "number" },
                { "name": "salePrice", "label": "Sale Price", "type": "currency" },
                { "name": "notes", "label": "Notes", "type": "textarea" }
              ]
            }
            """),
        new(
            "Fluid Change",
            "A fluid change service event",
            """
            {
              "fields": [
                {
                  "name": "fluidType",
                  "label": "Fluid Type",
                  "type": "select",
                  "required": true,
                  "options": ["Oil", "Coolant", "Brake Fluid", "Transmission Fluid", "Power Steering Fluid", "Differential Fluid", "Washer Fluid"]
                },
                { "name": "brand", "label": "Brand", "type": "text" },
                { "name": "quantity", "label": "Quantity", "type": "number" },
                { "name": "cost", "label": "Cost", "type": "currency" },
                { "name": "notes", "label": "Notes", "type": "textarea" }
              ]
            }
            """),
        new(
            "Car Wash",
            "A car wash event",
            """
            {
              "fields": [
                {
                  "name": "washType",
                  "label": "Wash Type",
                  "type": "select",
                  "options": ["Self-Service", "Automatic", "Hand Wash", "Touchless"]
                },
                { "name": "location", "label": "Location", "type": "text" },
                { "name": "cost", "label": "Cost", "type": "currency" }
              ]
            }
            """),
        new(
            "Car Detail",
            "A car detail event",
            """
            {
              "fields": [
                {
                  "name": "detailType",
                  "label": "Detail Type",
                  "type": "select",
                  "options": ["Interior", "Exterior", "Full Detail", "Paint Correction", "Ceramic Coating"]
                },
                { "name": "provider", "label": "Provider", "type": "text" },
                { "name": "cost", "label": "Cost", "type": "currency" },
                { "name": "notes", "label": "Notes", "type": "textarea" }
              ]
            }
            """),
        new(
            "Fuel Fill Up",
            "A fuel fill-up event",
            """
            {
              "fields": [
                {
                  "name": "fuelType",
                  "label": "Fuel Type",
                  "type": "select",
                  "required": true,
                  "options": ["Gasoline", "Diesel"]
                },
                { "name": "gallons", "label": "Gallons", "type": "number" },
                { "name": "pricePerGallon", "label": "Price Per Gallon", "type": "currency" },
                { "name": "cost", "label": "Total Cost", "type": "currency" },
                { "name": "odometer", "label": "Odometer", "type": "number" },
                { "name": "station", "label": "Station", "type": "text" }
              ]
            }
            """),
    ];

    public void Seed()
    {
        SeedRoles();
        SeedEventTypeDefinitions();
    }

    private void SeedRoles()
    {
        if (!db.Roles.Any())
        {
            db.Roles.Add(new()
            {
                Permissions = Enum.GetValues<Permission>().ToList(),
                Name = "Admin",
                NormalizedName = "ADMIN",
            });

            // NOTE: In this application's permissions-based authorization system,
            // additional roles can freely be created by administrators.
            // You don't have to seed every possible role.

            db.SaveChanges();
        }
    }

    private void SeedEventTypeDefinitions()
    {
        var existingByName = db.EventTypeDefinitions
            .ToList()
            .GroupBy(e => e.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

        foreach (var seed in DefaultEventTypeDefinitions)
        {
            if (existingByName.TryGetValue(seed.Name, out var existing))
            {
                if (string.IsNullOrWhiteSpace(existing.JsonDefinition) || existing.JsonDefinition.Trim() == "{}")
                {
                    existing.JsonDefinition = seed.JsonDefinition;
                }

                if (string.IsNullOrWhiteSpace(existing.Description))
                {
                    existing.Description = seed.Description;
                }

                continue;
            }

            db.EventTypeDefinitions.Add(new EventTypeDefinition
            {
                Name = seed.Name,
                Description = seed.Description,
                JsonDefinition = seed.JsonDefinition,
                IsActive = true,
            });
        }

        db.SaveChanges();
    }

    /// <summary>
    /// Grant administrative permissions to the very first user in the application.
    /// </summary>
    public void InitializeFirstUser(User user)
    {
        if (db.Users.Any()) return;

        // If this user is the first user, give them all roles so there is an initial admin.
        user.UserRoles = db.Roles.Select(r => new UserRole { Role = r, User = user }).ToList();
    }

    private sealed record EventTypeDefinitionSeed(string Name, string Description, string JsonDefinition);
}
