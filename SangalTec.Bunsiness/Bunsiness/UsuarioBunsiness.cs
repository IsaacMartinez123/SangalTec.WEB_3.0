using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SangalTec.Bunsiness.Abstract;
using SangalTec.Bunsiness.Dtos;
using SangalTec.DAL;
using SangalTec.Models.EntitiesUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Bunsiness
{
    public class UsuarioBunsiness : IUsuarioBunsiness
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioBunsiness(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            
        }

        public async Task<UsuarioDto> ObtenerUsuarioDtoPorEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario != null)
            {
                UsuarioDto usuarioDto = new()
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Estado = usuario.Estado,
                    NumeroCelular = usuario.PhoneNumber
                };
                return usuarioDto;
            }
            return null;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerListaUsuarios()
        {
            List<UsuarioDto> listaUsuarioDtos = new();
            var usuarios = await _userManager.Users.ToListAsync();
            usuarios.ForEach(usuario =>
            {
                UsuarioDto usuarioDto = new()
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Estado = usuario.Estado,
                    NumeroCelular = usuario.PhoneNumber
                };
                listaUsuarioDtos.Add(usuarioDto);

            });
            return listaUsuarioDtos;
        }

        public async Task<Usuario> ObtenerUsuarioPorId(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EditarDto> ObtenerUsuarioDtoPorId(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var usuario = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario != null)
            {
                EditarDto editarDto = new()
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    Estado = usuario.Estado,
                    NumeroCelular = usuario.PhoneNumber,

                };
                return editarDto;

            }
            return null;
        }

        public async Task<string> Crear(RegistrarUsuarioDto registrarUsuarioDto)
        {
            if (registrarUsuarioDto == null)
                throw new ArgumentNullException(nameof(registrarUsuarioDto));
            Usuario usuario = new()
            {
                UserName = registrarUsuarioDto.Email,
                Email = registrarUsuarioDto.Email,
                Estado = registrarUsuarioDto.Estado = true,
                PhoneNumber = registrarUsuarioDto.NumeroCelular
            };
            var resultado = await _userManager.CreateAsync(usuario, registrarUsuarioDto.Password);
            if (resultado.Errors.Any())
                return "ErrorPassword";
            if (resultado.Succeeded)
                return usuario.Id;
            return null;

        }

        public async Task<string> Editar(EditarDto editarDto)
        {
            if (editarDto == null)
                throw new ArgumentNullException(nameof(editarDto));

            var usuario = await _userManager.FindByIdAsync(editarDto.Id);

            if (usuario != null)
            {

                usuario.UserName = editarDto.Email;
                usuario.NormalizedUserName = editarDto.Email.ToUpper();
                usuario.Email = editarDto.Email;
                usuario.NormalizedEmail = editarDto.Email.ToUpper();
                usuario.PhoneNumber = editarDto.NumeroCelular;

            }

            var resultado = await _userManager.UpdateAsync(usuario);

            if (resultado.Succeeded)
            {
                return "Ok";

            }

            return "No";

        }

        public async Task<string> CambiarEstado (UsuarioDto usuarioDto)
        {

            var usuario = await _userManager.FindByIdAsync(usuarioDto.Id);

            if (usuario != null)
            {
                if (usuario.Estado)
                    usuario.Estado = false;

                else
                    usuario.Estado = true;


            }

            var resultado = await _userManager.UpdateAsync(usuario);

            if (resultado.Succeeded)
            {
                return "Ok";

            }

            return "No";
        }

    }
}
