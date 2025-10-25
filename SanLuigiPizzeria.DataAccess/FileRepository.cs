using SanLuigiPizzeria.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.DataAccess
{
    public class FileRepository
    {
        /*
         * serializzatore, stream, using, PATH, 
         */

        private readonly string _pizzaFilePath = "..\\..\\..\\pizze.json";
        private readonly string _ordiniFilePath = "..\\..\\..\\ordini.json";

        private List<Pizza> _pizzaAppo = new();
        private List<Ordine> _ordiniAppo = new();

        public FileRepository()
        {
            LoadDataAsync().Wait();
        }

        private async Task LoadDataAsync()
        {
            if (File.Exists(_pizzaFilePath))
            {
                using var reader = new StreamReader(_pizzaFilePath);
                string json = await reader.ReadToEndAsync();
                _pizzaAppo = JsonSerializer.Deserialize<List<Pizza>>(json) ?? new();
            }
            if (File.Exists(_ordiniFilePath))
            {
                using var reader = new StreamReader(_ordiniFilePath);
                string json = await reader.ReadToEndAsync();
                _ordiniAppo = JsonSerializer.Deserialize<List<Ordine>>(json) ?? new();
            }
        }
        public async Task ModificaPizza(string nomePizza, double prezzo)
        {
            var pizzaDaModificare = _pizzaAppo.First(pizza => pizza.Name == nomePizza);

            pizzaDaModificare.Price = prezzo;
            
            await SavePizzaAsync();
        }
        public async Task AggiungiOrdineAsync(Ordine ordine)
        {
            if (_ordiniAppo.Count > 0)
            {
                ordine.OrderId = _ordiniAppo.Max(ordine => ordine.OrderId) + 1;
            }
            else
            {
                ordine.OrderId = 0;
            }
            _ordiniAppo.Add(ordine);

            await SaveOrdersAsync();
        }
        public async Task AggiungiPizzaAsync(Pizza pizza)
        {
            //lettura da file + deserializzazione nella lista _pizzaAppo
            if(_pizzaAppo.FirstOrDefault(pizzaElement => (pizzaElement.Name == pizza.Name)) == null)
            {
                  _pizzaAppo.Add(pizza);
                  await SavePizzaAsync();
            }

        }

        private async Task SavePizzaAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using (var writer = new StreamWriter(_pizzaFilePath, false))
            {
                string pizzeJson = JsonSerializer.Serialize(_pizzaAppo, options);
                await writer.WriteAsync(pizzeJson);
            }
        }
        private async Task SaveOrdersAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };

            using (var writer = new StreamWriter(_ordiniFilePath, false))
            {
                string ordininJson = JsonSerializer.Serialize(_ordiniAppo, options);
                await writer.WriteAsync(ordininJson);
            }
        }

        public List<Pizza> VisualizzaPizze()
        {
            return _pizzaAppo;
        }
    }
}
