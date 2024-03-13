using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SuppliesPriceLister;

public class PartnerList
{
    public List<Partner> Partners { get; set; }
}

public class Partner
{
    public List<Supply> Supplies { get; set; }
}

public class Supply
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int PriceInCents { get; set; }
}


public class SupplyRepository : ISupplyRepository
{
    private readonly string _csvFilePath;
    private readonly string _jsonFilePath;
    private readonly double _exchangeRate;

    //Receive an IConfiguration instance from dependency injection.
    public SupplyRepository(IConfiguration configuration)
    {
        var projectRootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        _csvFilePath = Path.Combine(projectRootPath, "humphries.csv");
        _jsonFilePath =Path.Combine(projectRootPath, "megacorp.json");
        _exchangeRate = double.Parse(configuration["audUsdExchangeRate"]);
    }

    //Get all supplyItems data
    public IEnumerable<SupplyItem> GetAllSupplies()
    {
        List<SupplyItem> supplies = new List<SupplyItem>();
        supplies.AddRange(ReadCsvFile());
        supplies.AddRange(ReadJsonFile());
        return supplies;
    }

    private IEnumerable<SupplyItem> ReadCsvFile()
    {
        List<SupplyItem> items = new List<SupplyItem>();
        string[] lines = File.ReadAllLines(_csvFilePath);

        //Iterate through each line in the file, skipping the header line
        foreach (var line in lines.Skip(1))
        {
            var columns = line.Split(',');
            items.Add(new SupplyItem
            {
                Id = columns[0],
                Description = columns[1],
                Price = Math.Round(double.Parse(columns[3]), 2)
            });
        }

        return items;
    }
    
    private IEnumerable<SupplyItem> ReadJsonFile()
    {
        string jsonString = File.ReadAllText(_jsonFilePath);
        // var partnerList = System.Text.Json.JsonSerializer.Deserialize<PartnerList>(jsonString);
        List<SupplyItem> items = new List<SupplyItem>();
        //Converts the jsonString to an instance of the PartnerList
        var partnerList = Newtonsoft.Json.JsonConvert.DeserializeObject<PartnerList>(jsonString);
        
        if (partnerList == null || partnerList.Partners == null)
        {
            Console.WriteLine("Warning: JSON data is null or in an unexpected format.");
            return items; 
        }
           
        // Iterate through each partner and its supplies, adding the supplies to the list
        foreach (var partner in partnerList.Partners)
        {
            foreach (var supply in partner.Supplies)
            {
                items.Add(new SupplyItem
                {
                    Id = supply.Id.ToString(),
                    Description = supply.Description,
                    Price = Math.Round(supply.PriceInCents / 100.0 * (1 + _exchangeRate), 2)
                });
            }
        }

        return items;
    }
}
