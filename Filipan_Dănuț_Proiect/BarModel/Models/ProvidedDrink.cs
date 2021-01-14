using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarModel.Models
{
    public class ProvidedDrink
    {
        public int ProviderID { get; set; }
        public int DrinkID { get; set; }
        public Provider Provider { get; set; }
        public Drink Drink { get; set; }
    }
}
