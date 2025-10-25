using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.DataAccess.Entities
{
    public class Ordine
    {
        public Ordine(List<Pizza> pizzas)
        {
            Pizzas = pizzas;
        }

        public int OrderId { get; set; }
        public List<Pizza> Pizzas { get; set; }
        public double TotalCost => CalculateCost();

        private double CalculateCost()
        {
            return Pizzas.Sum(pizza => pizza.Price);
        }
    }
}
