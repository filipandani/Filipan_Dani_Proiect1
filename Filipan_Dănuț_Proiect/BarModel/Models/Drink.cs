using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarModel.Models
{
    public class Drink
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Liters { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ProvidedDrink> ProvidedDrinks { get; set; }

    }
}