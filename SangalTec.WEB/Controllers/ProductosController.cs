using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SangalTec.Bunsiness.Abstract;
using SangalTec.Models.Entities;
using SangalTec.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SangalTec.WEB.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoBunsiness _IProductoBunsiness;
        private readonly ICategoriaBunsiness _ICategoriaBunsiness;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductosController(IProductoBunsiness ProductoBunsiness, ICategoriaBunsiness ICategoriaBunsiness, IWebHostEnvironment hostingEnvironment)
        {
            _IProductoBunsiness = ProductoBunsiness;
            _hostingEnvironment = hostingEnvironment;
            _ICategoriaBunsiness = ICategoriaBunsiness;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Titulo = "Lista de productos";
            return View(await _IProductoBunsiness.ObtenerProductos());
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Titulo = "Crear producto";
            ViewBag.Categoria = new SelectList(await _ICategoriaBunsiness.ObtenerCategoria(), "CategoriaId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    _IProductoBunsiness.Crear(producto);

                    var guardar = await _IProductoBunsiness.GuardarCambios();

                    if (guardar)
                    {
                        return Json(new { isValid = true, operacion = "crear" });
                    }

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el producto" });

                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "danger", error = "Error al crear el producto" });
                }
            }

            ViewBag.Categoria = new SelectList(await _ICategoriaBunsiness.ObtenerCategoria(), "CategoriaId", "Nombre");

            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Crear", producto) });
        }

        [NoDirectAccessAttribute]
        public async Task<IActionResult> Detalle(int? id)
        {
            if (id != null)
            {
                try
                {
                    var producto = await _IProductoBunsiness.ObtenerProductoPorId(id);
                    if (producto != null)
                    {
                        return View(producto);

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
                    var producto = await _IProductoBunsiness.ObtenerProductoPorId(id);

                    if (producto != null)
                    {
                        ViewBag.Titulo = "Editar Producto";
                        ViewBag.Categoria = new SelectList(await _ICategoriaBunsiness.ObtenerCategoria(), "CategoriaId", "Nombre");
                        return View(producto);
                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar categría" });
                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar categría" });
                }
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Editar(int? id, Producto producto)
        {
            if (id != producto.ProductoId)
                return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar el producto" });

            if (ModelState.IsValid)
            {
                try
                {

                    _IProductoBunsiness.Editar(producto);

                    var guardarCambios = await _IProductoBunsiness.GuardarCambios();

                    if (guardarCambios)
                        return Json(new { isValid = true, operacion = "editar" });
                    else
                        return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar el producto" });


                }
                catch (Exception)
                {
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al editar el producto" });
                }
            }

            //Si el producto tiene errores en las validaciones
            ViewBag.Titulo = "Editar Producto";
            ViewBag.Categoria = new SelectList(await _ICategoriaBunsiness.ObtenerCategoria(), "CategoriaId", "Nombre");

            return Json(new { isValid = false, tipoError = "warning", error = "Debe diligenciar los campos requeridos", html = Helper.RenderRazorViewToString(this, "Editar", producto) });
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if(id != null)
            {
                try
                {
                    var producto = await _IProductoBunsiness.ObtenerProductoPorId(id); 

                    if(producto != null)
                    {
                        _IProductoBunsiness.Eliminar(producto);

                        var guardarCambios = await _IProductoBunsiness.GuardarCambios();

                        if (guardarCambios)
                            return Json(new { isValid = true }); ;

                        return Json(new { isValid = false, tipoError = "error", mensaje  = "Error al eliminar el producto"});
                    }
                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el producto" });
                }
                catch (Exception)
                {

                    return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el producto" });
                }
            }
            return Json(new { isValid = false, tipoError = "error", mensaje = "Error al eliminar el producto" });
        }

        [NoDirectAccessAttribute]
        [HttpGet]
        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id != null)
            {
                try
                {
                    var producto = await _IProductoBunsiness.ObtenerProductoPorId(id);

                    if(producto != null)
                    {
                        if (producto.Estado)
                            producto.Estado = false;
                        else
                            producto.Estado = true;

                        _IProductoBunsiness.Editar(producto);

                        var guardarCambios = await _IProductoBunsiness.GuardarCambios(); 

                        if(guardarCambios)
                            return Json(new { isValid = true});

                    }
                    return Json(new {isValid = false, tipoError = "error", mensaje = "Error al cambiar el estado"});
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
