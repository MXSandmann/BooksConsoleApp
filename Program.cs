using BooksConsoleApp.Context;
using BooksConsoleApp.Entities;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("SqlServer")!;
Console.WriteLine("--> " + connectionString);

using (var context = new DataContext(connectionString))
{
    context.Authors.Add(new Author { Name = "Will" });
    context.SaveChanges();
}

