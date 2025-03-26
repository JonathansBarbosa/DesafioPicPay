using System;
using System.ComponentModel.DataAnnotations;

namespace PicPaySimplificado.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor da transação deve ser maior que zero.")]
        public decimal Valor { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public Guid PagadorId { get; set; }
        public Usuario Pagador { get; set; } = null!; // Inicializa com null! e suprime o warning

        public Guid RecebedorId { get; set; }
        public Usuario Recebedor { get; set; } = null!; // Inicializa com null! e suprime o warning

        public Transacao() { }

        public Transacao(Guid pagadorId, Guid recebedorId, decimal valor)
        {
            Id = Guid.NewGuid();
            PagadorId = pagadorId;
            RecebedorId = recebedorId;
            Valor = valor;
        }

    }
}