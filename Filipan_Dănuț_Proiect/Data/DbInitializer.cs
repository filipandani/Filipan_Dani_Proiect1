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
             new Drink{Name="Blue",Brand="Johnnie Walker",Price=Decimal.Parse("120"), Liters=Decimal.Parse("1")},
             new Drink{Name="Gold 21 Yo",Brand="Johnnie Walker",Price=Decimal.Parse("210"), Liters=Decimal.Parse("1")},
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
             new Order{DrinkID=1,CustomerID=1050,OrderDate=DateTime.Parse("2021-01-01") },
             new Order{DrinkID=3,CustomerID=1045,OrderDate=DateTime.Parse("2020-10-28") },
             new Order{DrinkID=1,CustomerID=1045,OrderDate=DateTime.Parse("2020-07-20") },
             new Order{DrinkID=2,CustomerID=1046,OrderDate=DateTime.Parse("2020-11-25") },
             new Order{DrinkID=3,CustomerID=1046,OrderDate=DateTime.Parse("2020-12-23") },
             new Order{DrinkID=3,CustomerID=1050,OrderDate=DateTime.Parse("2020-12-31") },
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
            var providers = new Provider[]
            {
                new Provider{ProviderName="Fodor Lorant", Site="www.Fodor_Lorant.com"},
                new Provider{ProviderName="Filip Lorand", Site="www.Filip_Lorand.com"},
                new Provider{ProviderName="Filipan Dani", Site="www.Filipan_Dani.com"},
                new Provider{ProviderName="Domnu Ionel", Site="www.Domnu_Ionel.com"},
                new Provider{ProviderName="Fodor Lucian", Site="Fodor_Lucian.com"},
            };
            foreach (Provider p in providers)
            {
                context.Providers.Add(p);
            }
            context.SaveChanges();
            var provideddrinks = new ProvidedDrink[]
            {
                new ProvidedDrink{
                    DrinkID= drinks.Single(c => c.Name=="Black").ID,
                    ProviderID=providers.Single(i => i.ProviderName ==" Fodor Lorant").ID
                },
                new ProvidedDrink
                {
                    DrinkID = drinks.Single(c => c.Name == "Blue").ID,
                    ProviderID = providers.Single(i => i.ProviderName == " Filip Lorand").ID
                },
                new ProvidedDrink
                {
                    DrinkID = drinks.Single(c => c.Name == "Rare").ID,
                    ProviderID = providers.Single(i => i.ProviderName == " Domnu Ionel").ID
                },
                new ProvidedDrink
                {
                    DrinkID = drinks.Single(c => c.Name == "12 YO").ID,
                    ProviderID = providers.Single(i => i.ProviderName == " Fodor Lucian").ID
                },
                new ProvidedDrink
                {
                    DrinkID = drinks.Single(c => c.Name == "Gold 21 Yo").ID,
                    ProviderID = providers.Single(i => i.ProviderName == " Filipan Dani").ID
                 },
                 };
            foreach (ProvidedDrink pd in provideddrinks)
            {
                context.ProvidedDrinks.Add(pd);
            }
            context.SaveChanges();
        }
    }
}