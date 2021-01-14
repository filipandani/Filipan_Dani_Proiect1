using Filipan_Dănuț_Proiect.Data;
using Filipan_Dănuț_Proiect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filipan_Dănuț_Proiect.Models
{
    public class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();
            if (context.Drinks.Any())
            {
                return; // BD a fost creata anterior
            }
            var drinks = new Drink[]
            {
             new Drink{Name="Black",Brand="Johnnie Walker",Price=Decimal.Parse("80"), Liters=Decimal.Parse("1")},
             new Drink{Name="Rare",Brand="J&B",Price=Decimal.Parse("75"), Liters=Decimal.Parse("1")},
             new Drink{Name="12 YO",Brand="Jack Ryan",Price=Decimal.Parse("250"), Liters=Decimal.Parse("1")},
            };
            foreach (Drink s in drinks)
            {
                context.Drinks.Add(s);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {
             new Customer{CustomerID=1050,Name="Farcaș Marcela",BirthDate=DateTime.Parse("1997-09-22")},
             new Customer{CustomerID=1045,Name="Roi Cornel",BirthDate=DateTime.Parse("1982-07-24")},
             new Customer{ CustomerID=1046, Name="Filip Lorand",BirthDate=DateTime.Parse("1998-05-17")},
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
             new Order{DrinkID=1,CustomerID=1050},
             new Order{DrinkID=3,CustomerID=1045},
             new Order{DrinkID=1,CustomerID=1045},
             new Order{DrinkID=2,CustomerID=1046},
             new Order{DrinkID=3,CustomerID=1046},
             new Order{DrinkID=3,CustomerID=1050},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
        }
    }
}