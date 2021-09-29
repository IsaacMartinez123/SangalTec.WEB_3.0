using SangalTec.Bunsiness.Dtos;
using SangalTec.Models.EntitiesUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Abstract
{
    public interface IUsuarioBunsiness
    {
        Task<UsuarioDto> ObtenerUsuarioDtoPorEmail(string email);
        Task<string> Crear(RegistrarUsuarioDto registrarUsuarioDto);
        Task<IEnumerable<UsuarioDto>> ObtenerListaUsuarios();

        Task<Usuario> ObtenerUsuarioPorId(string id);

        Task<EditarDto> ObtenerUsuarioDtoPorId(string id);

        Task<string> Editar(EditarDto editarDto);

        Task<string> CambiarEstado(UsuarioDto usuarioDto);
    }

}
