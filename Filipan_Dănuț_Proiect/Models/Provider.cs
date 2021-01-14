using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Filipan_Dănuț_Proiect.Models
{
    public class Provider
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Provider Name")]
        [StringLength(50)]
        public string ProviderName { get; set; }

        [StringLength(70)]
        public string Site { get; set; }
        public ICollection<ProvidedDrink> ProvidedDrinks { get; set; }
    }
}
