using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "OrchardCore.UserGroups",
    Author = "The Orchard Team",
    Website = "http://orchardproject.net",
    Version = "0.0.1",
    Description = "Provides the ability to create a hierarchical structure of organizational units that can contain users or other groups(organizational units).",
    Category = "Security"
)]


[assembly: Feature(
    Id = "OrchardCore.UserGroups",
    Name = "User Groups",
    Description = "Provides the ability to create a hierarchical structure of organizational units that can contain users or other groups(organizational units).",
    Category = "Security"
)]