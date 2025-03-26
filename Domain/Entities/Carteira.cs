using System;
using System.ComponentModel.DataAnnotations;

namespace PicPaySimplificado.Domain.Entities
{
    public class Carteira
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O saldo é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O saldo não pode ser negativo.")]
        public decimal Saldo { get; set; }

        public Guid UsuarioId { get; set; } // Chave estrangeira
        public Usuario Usuario { get; set; } = null!; // Propriedade de navegação (inicializada para evitar warnings)
    }
}