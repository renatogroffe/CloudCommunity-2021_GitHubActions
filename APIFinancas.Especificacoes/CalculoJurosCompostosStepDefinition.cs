using System.IO;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;
using FluentAssertions;
using Refit;
using APIFinancas.Especificacoes.HttpClients;
using APIFinancas.Especificacoes.Models;

namespace APIFinancas.Especificacoes
{
    [Binding]
    public sealed class CalculoJurosCompostosStepDefinition
    {
        private readonly ICalculoJurosCompostosAPI _apiJurosCompostos;
        private double _valorEmprestimo;
        private int _numMeses;
        private double _percTaxa;
        private ApiResponse<Emprestimo> _apiResponse;

        public CalculoJurosCompostosStepDefinition()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables().Build();

            _apiJurosCompostos = RestService.For<ICalculoJurosCompostosAPI>(
                 configuration["UrlWebAppTestes"]);
        }

        [Given(@"que o valor o valor do empréstimo é de R\$ (.*)")]
        public void PreencherValorEmprestimo(double valorEmprestimo)
        {
            _valorEmprestimo = valorEmprestimo;
        }

        [Given(@"que este empréstimo será por (.*) meses")]
        public void PreencherNumeroMeses(int numMeses)
        {
            _numMeses = numMeses;
        }

        [Given(@"que a taxa de juros é de (.*)% ao mês")]
        public void PreencherPercentualTaxa(double percTaxa)
        {
            _percTaxa = percTaxa;
        }

        [When(@"eu solicitar o cálculo do valor total a ser pago ao final do período")]
        public void ProcessarCalculoJurosCompostos()
        {
            _apiResponse = _apiJurosCompostos.Get(
                _valorEmprestimo, _numMeses, _percTaxa).Result;
        }

        [Then(@"o status code da reposta será (.*) \((.*)\)")]
        public void ValidarStatusCode(int statusCode, string descStatusCode)
        {
            _apiResponse.StatusCode.Should().Be(statusCode,
                $"* Esperada uma resposta {statusCode} - {descStatusCode} *");
        }

        [Then(@"o resultado será (.*)")]
        public void ValidarResultado(double valorFinalEmprestimo)
        {
            _apiResponse.Content.ValorFinalComJuros
                .Should().Be(valorFinalEmprestimo,
                "* Valor nao corresponde ao esperado ao final do periodo # " +
                $"Valor do Emprestimo: {_valorEmprestimo} | Num. Meses: {_numMeses} | Taxa Mensal Juros %: {_percTaxa} *");
        }
    }
}