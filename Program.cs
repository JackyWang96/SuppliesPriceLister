using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace SuppliesPriceLister
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        
            //Create a service container and register the services required by the application
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration) 
                .AddTransient<ISupplyRepository, SupplyRepository>() 
                .AddTransient<ISupplyService, SupplyService>() 
                .BuildServiceProvider(); 
        
            //use GetSortedSupplies method get data
            ISupplyService supplyService = serviceProvider.GetService<ISupplyService>();
            IEnumerable<SupplyItem> sortedSupplies = supplyService.GetSortedSupplies();
        
            foreach (SupplyItem supply in sortedSupplies)
            {
                Console.WriteLine($"ID: {supply.Id}, Description: {supply.Description}, Price: {supply.Price.ToString("F2")} AUD");
            }
        }
    }
}
