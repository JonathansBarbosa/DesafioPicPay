namespace PicPaySimplificado.Application.DTOs
{
    public class ResultadoCadastroUsuario
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}
