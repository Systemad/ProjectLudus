// See https://aka.ms/new-console-template for more information

using Seed;

Console.WriteLine("Hello, World!");

var task = new SeedService();
await task.FetchAndInsertDataAsync();
