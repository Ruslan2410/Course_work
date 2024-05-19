using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_work.Items
{
    public class Supply
    {
        public int SupplyID { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public int Quantity { get; set; }
        public DateTime SupplyDate { get; set; }
        public decimal SupplyAmount { get; set; }
    }
}
