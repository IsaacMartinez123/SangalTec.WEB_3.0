using Microsoft.AspNetCore.Mvc;
using SangalTec.Bunsiness.Abstract;
using SangalTec.Models.Entities;
using SangalTec.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SangalTec.WEB.Controllers
{
    public class InsumosController : Controller
    {

        private readonly IInsumoBusiness _IInsumoBusiness;

        public InsumosController(IInsumoBusiness IInsumoBusiness)
        {
            _IInsumoBusiness = IInsumoBusiness;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Titulo = "Lista de insumos";
            return View(await _IInsumoBusiness.ObtenerInsumos());
        }


        [NoDirectAccessAttribute]
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Titulo = "Crear insumo";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Crear(Insumo insumo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _IInsumoBusiness.Crear(insumo);

                    var guardarCambio = await _IInsumoBusiness.GuardarCambios();

                    if (guardarCambio)
                        return Json(new { isValid = true, operacion = "crear" });

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear insumo" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear insumo" });
                }
            }
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", insumo) });

        }

        [NoDirectAccessAttribute]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var insumo = await _IInsumoBusiness.ObetenerInsumoPorId(id);
                    if (insumo != null)
                    {
                        return View(insumo);

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

        [NoDirectAccessAttribute]
        [HttpGet]

        public async Task<IActionResult> Editar(int? id)
        {
            if (id != null)
            {
                try
                {
                    var insumo = await _IInsumoBusiness.ObetenerInsumoPorId(id);

                    if (insumo != null)
                    {

                        ViewBag.Titulo = "Editar insumo";
                        return View(insumo);
                    }

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar insumo" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar insumo" });
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int? id, Insumo insumo)
        {
            if (id == null)
                return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar insumo" });

            if (ModelState.IsValid)
            {
                try
                {
                    _IInsumoBusiness.Editar(insumo);

                    var guardarCambios = await _IInsumoBusiness.GuardarCambios();

                    if (guardarCambios)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar insumo" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar insumo" });
                }
            }
            //Si el insumo tiene errores en las validaciones
            ViewBag.Titulo = "Editar insumo";


            return Json(new { isValid = false, tipoError = "warning", mensaje = "Debe diligenciar todos los campos", html = Helper.RenderRazorViewToString(this, "Editar", insumo) });
        }


        [NoDirectAccessAttribute]
        [HttpGet]

        public async Task<IActionResult> Eliminar(int? id)
        {
            if(id != null)
            {
                try
                {
                    var insumo = await _IInsumoBusiness.ObetenerInsumoPorId(id); 

                    if(insumo != null)
                    {
                        _IInsumoBusiness.Eliminar(insumo);

                        var guardarCambios = await _IInsumoBusiness.GuardarCambios();

                        if (guardarCambios)
                            return Json(new { isValid = true });
                    }


                    return Json(new { isValid = false, tipoError = "warning", mensaje = "Error al eliminar insumo" });
                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "warning", mensaje = "Error al eliminar insumo" });
                }
            }

            return Json(new { isValid = false, tipoError = "warning", mensaje = "Error al eliminar insumo" });
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id != null)
            {
                try
                {
                    var insumo = await _IInsumoBusiness.ObetenerInsumoPorId(id);

                    if (insumo != null)
                    {
                        if (insumo.Estado)
                            insumo.Estado = false;
                        else
                            insumo.Estado = true;

                        _IInsumoBusiness.Editar(insumo);

                        var guardarCambios = await _IInsumoBusiness.GuardarCambios();

                        if (guardarCambios)
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

    }
}
