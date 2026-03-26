var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Finlo_Api>("finlo-api");

builder.Build().Run();
