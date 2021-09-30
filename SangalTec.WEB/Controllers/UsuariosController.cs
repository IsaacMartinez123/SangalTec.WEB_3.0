using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SangalTec.Bunsiness.Abstract;
using SangalTec.Bunsiness.Dtos;
using SangalTec.Models.EntitiesUsers;
using SangalTec.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SangalTec.WEB.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioBunsiness _usuarioBunsiness;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public UsuariosController(IUsuarioBunsiness usuarioBunsiness, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _usuarioBunsiness = usuarioBunsiness;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Titulo = "Lista de usuarios";
            return View(await _usuarioBunsiness.ObtenerListaUsuarios());
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(RegistrarUsuarioDto registrarUsuario)
        {
            if (ModelState.IsValid)
            {
                //comprobar su existe el usuario con ese correo

                var email = await _usuarioBunsiness.ObtenerUsuarioDtoPorEmail(registrarUsuario.Email);
                if (email == null)
                {
                    try
                    {
                        var usuarioId = await _usuarioBunsiness.Crear(registrarUsuario);

                        if (usuarioId == null)
                            return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el usuario" });

                        if (usuarioId.Equals("ErrorPassword"))
                            return Json(new { isValid = false, tipoError = "danger", error = "Error de password" });

                        return Json(new { isValid = true, operacion = "crear" });
                    }
                    catch (Exception)
                    {

                        return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el usuario" });
                    }
                }
                return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el usuario" });
            }

            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", registrarUsuario) });
        }



        [NoDirectAccessAttribute]
        public async Task<IActionResult> Detalle(string id)
        {
            if (id != null)
            {
                try
                {
                    var usuario = await _usuarioBunsiness.ObtenerUsuarioDtoPorId(id);
                    if (usuario != null)
                    {
                        return View(usuario);

                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error interno" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error interno" });
                }

            }
            return Json(new { isValid = false, tipoError = "error", mensaje = "Error interno" });
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RecordarMe, false);
                if (resultado.Succeeded)
                    return RedirectToAction("Dashboard", "Admin");

            }

            return View();
        }


        [HttpGet]
        public IActionResult OlvidePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OlvidePassword(RecuperarPasswordDto recuperarPasswordDto)
        {
            if (ModelState.IsValid)
            {
                //buscamos si el email existe
                var usuario = await _userManager.FindByEmailAsync(recuperarPasswordDto.Email);
                if (usuario != null)
                {
                    //generamos un token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

                    //creamos un link para resetear el password
                    var passwordresetLink = Url.Action("ResetearPassword", "Usuarios",
                        new { email = recuperarPasswordDto.Email, token = token }, Request.Scheme);

                    //Metodo tradicional de enviar correos por smtp
                    MailMessage mensaje = new();
                    mensaje.To.Add(recuperarPasswordDto.Email); //destinatario
                    mensaje.Subject = "SangalTec recuperar password";
                    mensaje.Body = passwordresetLink;
                    mensaje.IsBodyHtml = false;

                    mensaje.From = new MailAddress("sangaltec@gmail.com", "Notificaciones");
                    SmtpClient smtpClient = new("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential("sangaltec@gmail.com", "sangaltec123");
                    smtpClient.Send(mensaje);

                }
            }

            return View();
        }


        [HttpGet]
        public IActionResult ResetearPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Error token");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetearPassword(ResetearPasswordDto resetearPasswordDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(resetearPasswordDto.Email);

                if (usuario != null)
                {
                    //resetear el password
                    var resultado = await _userManager.ResetPasswordAsync(usuario, resetearPasswordDto.Token, resetearPasswordDto.Password);
                    if (resultado.Succeeded)
                    {
                        return RedirectToAction("Login", "Usuarios");
                    }
                }
            }
            return View();
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            if (id != null)
            {
                try
                {
                    var usuario = await _usuarioBunsiness.ObtenerUsuarioDtoPorId(id);
                    if (usuario != null)
                    {
                        ViewBag.Titulo = "Editar usuario";

                        return View(usuario);

                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error interno" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error interno" });
                }

            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Editar(string id, EditarDto editarDto)
        {
            if (id == null)
                Json(new { isValid = false, tipoError = "Error", mensaje = "Error al editar usuario" });

            if (ModelState.IsValid)
            {
                try
                {

                    var resultado = await _usuarioBunsiness.Editar(editarDto);

                    if (resultado.Equals("Ok"))
                    {
                        return Json(new { isValid = true, operacion = "editar" });

                    }

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar el usuario" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar el usuario" });
                }
            }
            ViewBag.Titulo = "Editar usuario";
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", editarDto) });
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> CambiarEstado(UsuarioDto usuarioDto)
        {
            if (usuarioDto != null)
            {
                try
                {
                    var resultado = await _usuarioBunsiness.CambiarEstado(usuarioDto);

                    if (resultado.Equals("Ok"))
                    {
                        return Json(new { isValid = true });

                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al cambiar el estado" });


                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al cambiar el estado" });
                }
            }
            return Json(new { isValid = false, tipoError = "error", mensaje = "Error al cambiar el estado" });

        }


        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Usuarios");
        }


    }
}
