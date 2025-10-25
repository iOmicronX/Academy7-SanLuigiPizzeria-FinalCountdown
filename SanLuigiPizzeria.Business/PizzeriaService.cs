using SanLuigiPizzeria.DataAccess;
using SanLuigiPizzeria.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.Business
{
    public class PizzeriaService
    {
        private readonly FileRepository _repository;
        public PizzeriaService(FileRepository repository)
        {
            _repository = repository;
        }
        public async Task AggiungiPizzaAsync(string nome, double prezzo)
        {
            Pizza pizza = new Pizza(nome, prezzo);

            await _repository.AggiungiPizzaAsync(pizza);
        }

        public List<Pizza> VisualizzaPizze()
        {
            return _repository.VisualizzaPizze();
        }

        public async Task AggiungiOrdineAsync(List<Pizza> pizze)
        {
            var ordine = new Ordine(pizze);

            await _repository.AggiungiOrdineAsync(ordine);
        }
    }
}
