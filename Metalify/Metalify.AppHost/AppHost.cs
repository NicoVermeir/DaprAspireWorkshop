var builder = DistributedApplication.CreateBuilder(args);

//int fixedPort = 1433;
//var sql = builder
//    .AddSqlServer("BeCertDevDB", builder.AddParameter("sqlServerDevPass", true), fixedPort)
//    .WithLifetime(ContainerLifetime.Persistent)
//    .WithVolume("VolumeMount.sqlserver.data", "/var/opt/mssql")
//    .AddDatabase("BeCertDB", "BeCertNT.TemplateConfigurationDb");

var sql = builder
    .AddSqlServer("metalify-sql")
    .AddDatabase("CatalogDb");

builder.AddProject<Projects.Metalify_BandCenter>("metalify-bandcenter");
builder.AddProject<Projects.Metalify_Bandcenter_Api>("metalify-bandcenter-api")
    .WithReference(sql)
    .WaitFor(sql);

builder.AddProject<Projects.Metalify>("metalify");
builder.AddProject<Projects.Metalify_Playlist_Api>("metalify-playlist-api");
builder.AddProject<Projects.Metalify_Catalog_Api>("metalify-catalog-api");


builder.Build().Run();
