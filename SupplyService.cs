using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SuppliesPriceLister;

public class SupplyService : ISupplyService
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly double  _exchangeRate;

    //Receive instances of ISupplyRepository and IConfiguration via dependency injection.
    public SupplyService(ISupplyRepository supplyRepository, IConfiguration configuration)
    {
        _supplyRepository = supplyRepository;
        _exchangeRate = double.Parse(configuration["audUsdExchangeRate"]);
    }

    //return a sorted collection of data
    public IEnumerable<SupplyItem> GetSortedSupplies()
    {
        IEnumerable<SupplyItem> supplies = _supplyRepository.GetAllSupplies();
        
        IEnumerable<SupplyItem> sortedSupplies = supplies.OrderByDescending(item => item.Price);

        return sortedSupplies;
    }
}
