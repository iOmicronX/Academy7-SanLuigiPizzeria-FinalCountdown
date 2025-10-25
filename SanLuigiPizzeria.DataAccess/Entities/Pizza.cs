using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.DataAccess.Entities
{
    public class Pizza
    {
        public Pizza(string name, double price)
        {
            Name = name;
            Price = price;
        }
        public Pizza()
        {
            
        }

        public string Name { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Price} €";
        }
    }
}
