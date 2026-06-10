var builder = DistributedApplication.CreateBuilder(args);

// MySQL local (sem Docker) — connection string em appsettings da API
var api = builder.AddProject<Projects.SpaceConnect_ApiService>("apiservice");

builder.AddProject<Projects.SpaceConnect_Web>("webfrontend")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
