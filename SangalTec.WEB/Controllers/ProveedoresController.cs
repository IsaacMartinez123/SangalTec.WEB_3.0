using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SangalTec.Bunsiness.Abstract;
using SangalTec.Models.Entities;
using SangalTec.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SangalTec.WEB.Controllers
{
    public class ProveedoresController : Controller
    {

        private readonly IProveedorBusiness _proveedorBusiness;
        private readonly IInsumoBusiness _insumoBusiness;

       
        public ProveedoresController(IProveedorBusiness proveedorBusiness, IInsumoBusiness insumoBusiness)
        {
            _proveedorBusiness = proveedorBusiness;
            _insumoBusiness = insumoBusiness;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Titulo = "Lista de proveedores";
            return View(await _proveedorBusiness.ObtenerProveedores());
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Titulo = "Crear proveedor";
            ViewBag.Insumos = new SelectList(await _insumoBusiness.ObtenerInsumos(), "InsumoId", "Nombre");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Proveedor proveedor)
        {
            if (ModelState.IsValid) {

                try
                {
                    _proveedorBusiness.Crear(proveedor);

                    var guardarCambios = await _proveedorBusiness.GuardarCambios();

                    if (guardarCambios)
                        return Json(new { isValid = true, operacion = "crear" });

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear proveedor" });
                }

                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear proveedor" });
                }

               
            }

            ViewBag.Insumos = new SelectList(await _insumoBusiness.ObtenerInsumos(), "InsumoId", "Nombre");
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", proveedor) });
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var proveedor = await _proveedorBusiness.ObtenerProveedorPorId(id); 

                    if(proveedor != null)
                    {
                        return View(proveedor);
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
            if(id != null)
            {
                try
                {
                    var proveedor = await _proveedorBusiness.ObtenerProveedorPorId(id);

                    if(proveedor != null)
                    {
                        ViewBag.Titulo = "Editar proveedor";
                        ViewBag.Insumos = new SelectList(await _insumoBusiness.ObtenerInsumos(), "InsumoId", "Nombre");
                        return View(proveedor);
                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar proveedor" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar proveedor" });
                }
            }
            return NotFound();
        }

        [NoDirectAccessAttribute]
        [HttpPost]
        public async Task<IActionResult> Editar(int? id, Proveedor proveedor)
        {
            if (id == null)
                return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar proveedor"});

            if (ModelState.IsValid)
            {
                try
                {
                    _proveedorBusiness.Editar(proveedor);

                    var guardarCambios = await _proveedorBusiness.GuardarCambios();

                    if (guardarCambios)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar proveedor" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar proveedor" });
                    throw;
                }
            }

            //Si el proveedor tiene errores en las validaciones
            ViewBag.Titulo = "Editar proveedor";
            ViewBag.Insumos = new SelectList(await _insumoBusiness.ObtenerInsumos(), "InsumoId", "Nombre");
            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar todos los campos", html = Helper.RenderRazorViewToString(this, "Editar", proveedor)});
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id != null)
            {
                try
                {
                    var producto = await _proveedorBusiness.ObtenerProveedorPorId(id);

                    if (producto != null)
                    {
                        _proveedorBusiness.Eliminar(producto);


                        var guardarCambios = await _proveedorBusiness.GuardarCambios();

                        if (guardarCambios)
                            return Json(new { isValid = true });
                        return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el proveedor" });
                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el proveedor" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el proveedor" });
                }
            }
            return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el proveedor" });
        }

        [NoDirectAccessAttribute]
        [HttpGet]

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id != null)
            {
                try
                {
                    var proveedor = await _proveedorBusiness.ObtenerProveedorPorId(id);

                    if(proveedor != null)
                    {

                        if (proveedor.Estado)
                            proveedor.Estado = false;
                        else
                            proveedor.Estado = true;

                        _proveedorBusiness.Editar(proveedor);

                        var guardarCambios = await _proveedorBusiness.GuardarCambios(); 

                        if(guardarCambios)
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
