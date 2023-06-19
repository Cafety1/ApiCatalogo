namespace ApiCatalogo.Dtos
{
    public class TokenDto
    {
        public bool Autenticado { get; set; }
        public DateTime Expiracao { get; set; }
        public string? Token { get; set; }
        public string? Messagem { get; set; }
    }
}
