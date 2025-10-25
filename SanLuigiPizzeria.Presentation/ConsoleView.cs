using SanLuigiPizzeria.Business;
using SanLuigiPizzeria.DataAccess.Entities;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanLuigiPizzeria.Presentation
{
    public class ConsoleView
    {
        private readonly PizzeriaService _service;
        public ConsoleView(PizzeriaService service)
        {
            _service = service;
        }
        public async Task StartApplicationAsync()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            var scelta = AnsiConsole.Prompt(
                        new SelectionPrompt<MenuChoices>()
                        .Title("[yellow]Seleziona un'opzione[/]")
                        .PageSize(10)
                        .UseConverter(menu => $"{(int)menu}) {FormatEnum(menu)}")
                        .AddChoices(Enum.GetValues<MenuChoices>()));

            switch (scelta)
            {
                case MenuChoices.AggiungiPizza:
                    await AggiungiPizzaMenuAsync();
                    break;
                case MenuChoices.VisualizzaPizze:
                    VisualizzaPizze();
                    break;
                case MenuChoices.AggiungiOrdine:
                    await AggiungiOrdineAsync();
                    break;
            }
        }
        private async Task AggiungiOrdineAsync()
        {
            var availablePizzas = _service.VisualizzaPizze();

            List<Pizza> pizzasToOrder = new();

            do
            {
                var pizzaScelta = AnsiConsole.Prompt(
                         new SelectionPrompt<Pizza>()
                         .Title("[yellow]Seleziona la pizza che vuoi aggiungere[/]")
                         .PageSize(10)
                         .UseConverter(pizza => $"{pizza.ToString()}")
                         .AddChoices(availablePizzas));

                pizzasToOrder.Add(pizzaScelta);

                AnsiConsole.MarkupLine($"[green]Hai aggiunto:[/] {pizzaScelta.Name} - €{pizzaScelta.Price:F2}");
            }
            while (AnsiConsole.Confirm("[bold]Vuoi aggiungere un'altra pizza?[/]"));

            await _service.AggiungiOrdineAsync(pizzasToOrder);
        }
        private void VisualizzaPizze()
        {
            var pizze = _service.VisualizzaPizze();

            var table = new Table();
            table.Border = TableBorder.Rounded;

            table.AddColumn("[bold]Nome[/]");
            table.AddColumn("[bold]Prezzo (€)[/]");

            // Aggiungi le righe dalla lista
            foreach (var pizza in pizze)
            {
                table.AddRow(pizza.Name, pizza.Price.ToString("F2"));
            }

            AnsiConsole.Write(table);
        }
        private async Task AggiungiPizzaMenuAsync()
        {
             string nome = AnsiConsole.Prompt(
                    new TextPrompt<string>("Inserisci il [green]nome[/] della pizza:")
                    .Validate(n =>
                        string.IsNullOrWhiteSpace(n)
                            ? ValidationResult.Error("[red]Il nome non può essere vuoto[/]")
                            : ValidationResult.Success()));

            // Chiedi il prezzo con validazione numerica
            double prezzo = AnsiConsole.Prompt(
                new TextPrompt<double>("Inserisci il [green]prezzo (€)[/]:")
                    .Validate(p =>
                        p <= 0
                            ? ValidationResult.Error("[red]Il prezzo deve essere maggiore di 0[/]")
                            : ValidationResult.Success()));

            // Conferma visuale
            AnsiConsole.MarkupLine($"\nHai inserito: [yellow]{nome}[/] - [green]{prezzo:C2}[/]");

            bool conferma = AnsiConsole.Confirm("[bold]Vuoi aggiungere questa pizza al menu?[/]");

            if (!conferma)
            {
                AnsiConsole.MarkupLine("[grey]Operazione annullata.[/]");
                return;
            }

            await AggiungiPizzaAsync(nome, prezzo);

        }

        private async Task AggiungiPizzaAsync(string nome, double prezzo)
        {
            await _service.AggiungiPizzaAsync(nome, prezzo);  
        }

        private static string FormatEnum(MenuChoices action)
        {
            // converte da "VisualizzaPizze" → "Visualizza pizze"
            return string.Concat(
                action.ToString()
                      .Select((x, i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString())
            );
        }
    }
}
