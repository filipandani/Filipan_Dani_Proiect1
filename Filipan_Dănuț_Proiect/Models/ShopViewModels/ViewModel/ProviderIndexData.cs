using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filipan_Dănuț_Proiect.Models.ShopViewModels.ViewModel
{
    public class ProviderIndexData
    {
        public IEnumerable<Provider> Providers { get; set; }
        public IEnumerable<Drink> Drinks { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
