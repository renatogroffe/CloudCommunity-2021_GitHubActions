using System.Threading.Tasks;
using APIFinancas.Especificacoes.Models;
using Refit;

namespace APIFinancas.Especificacoes.HttpClients
{
    public interface ICalculoJurosCompostosAPI
    {
        [Get("/calculofinanceiro/juroscompostos")]
        Task <ApiResponse<Emprestimo>> Get(
            double valorEmprestimo, int numMeses, double percTaxa);
    }
}