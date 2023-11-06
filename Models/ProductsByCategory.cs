using System;
using System.Collections.Generic;

namespace Anropa_databasen__SQL___ORM_.Models
{
    public partial class ProductsByCategory
    {
        public string CategoryName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public short? UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
