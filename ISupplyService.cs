using System.Collections.Generic;

namespace SuppliesPriceLister;

public interface ISupplyService
{
    IEnumerable<SupplyItem> GetSortedSupplies();
}