using System.Collections.Generic;

namespace SuppliesPriceLister;

public interface ISupplyRepository
{
    IEnumerable<SupplyItem> GetAllSupplies();
}