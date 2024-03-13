using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SuppliesPriceLister;

public class SupplyItem
{
    public string Id { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}