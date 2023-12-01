using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MagicVilla_API.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;
        private string _secretKey;
        public UsuarioRepositorio(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUsuarioUnico(string UserName)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName.ToLower() == UserName.ToLower());
            if(usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower() &&
                                                                u.Password == loginRequestDto.Password);
            if(usuario == null)
            {
                return new LoginResponseDto(){
                    Token = "",
                    Usuario = null
                };
            }
            // Si el usuario existe, generar un Jwtoken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            LoginResponseDto loginResponseDto = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = usuario,
            };
            return loginResponseDto;
        }

        public async Task<Usuario> Registrar(RegistroRequestDto registroRequestDto)
        {
            Usuario usuario = new()
            {
                UserName = registroRequestDto.UserName,
                Password = registroRequestDto.Password,
                Nombres = registroRequestDto.Nombres,
                Rol = registroRequestDto.Rol,
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            usuario.Password = "";
            return usuario;
        }
    }
}
