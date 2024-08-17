using CodingTracker;

using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .Build();

Console.WriteLine("ConnectionString: {0}", config.GetConnectionString("SQLite"));

DapperHelper dapper = new DapperHelper(config);

dapper.InitializeDb();