using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application.Interfaces; // Importe a interface ITransacaoService
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PicPaySimplificado.Controllers
{
    [ApiController]
    [Route("api/transacoes")] // Rota corrigida
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService; // Injeção da interface

        public TransacoesController(ITransacaoService transacaoService) // Injeção da interface no construtor
        {
            _transacaoService = transacaoService ?? throw new ArgumentNullException(nameof(transacaoService));
        }

        [HttpPost]
        public async Task<IActionResult> RealizarTransferencia([FromBody] TransferenciaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _transacaoService.RealizarTransferencia(request.PagadorId, request.BeneficiarioId, request.Valor);

                switch (resultado)
                {
                    case "Transferência realizada com sucesso.":
                        return Ok(new { Mensagem = resultado });
                    case "Usuário(s) não encontrado(s).":
                        return NotFound(new { Mensagem = resultado });
                    case "Saldo insuficiente.":
                        return BadRequest(new { Mensagem = resultado });
                    case "Transferência não autorizada.":
                        return Unauthorized(new { Mensagem = resultado });
                    default: // Outros erros
                        return BadRequest(new { Mensagem = resultado }); // Ou StatusCode(500, ...) se for um erro interno
                }
            }
            catch (Exception ex)
            {
                // Logar a exceção (usar um logger adequado em produção)
                Console.WriteLine($"Erro ao realizar transferência: {ex.Message}");
                return StatusCode(500, new { Mensagem = "Ocorreu um erro interno no servidor." });
            }
        }
    }

    public class TransferenciaRequest
    {
        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O ID do pagador é obrigatório.")]
        public Guid PagadorId { get; set; }

        [Required(ErrorMessage = "O ID do beneficiário é obrigatório.")]
        public Guid BeneficiarioId { get; set; }
    }
}