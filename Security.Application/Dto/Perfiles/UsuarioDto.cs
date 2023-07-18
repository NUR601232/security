namespace Application.Dto.Usuarios
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool Staff { get; set; }

    }
}
