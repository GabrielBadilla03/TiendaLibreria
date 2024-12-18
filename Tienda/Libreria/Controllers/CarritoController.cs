using Libreria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Libreria.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Procesar Pago
        [HttpGet]
        public ActionResult ProcesarPago()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            ViewBag.Total = carrito.Sum(c => (c.Cantidad ?? 0) * c.Producto.Precio);
            return View(carrito);
        }

       
        // POST: Cancelar Compra
        [HttpPost]
        public ActionResult CancelarCompra()
        {
            TempData["Info"] = "Compra cancelada.";
            return RedirectToAction("Index", "Productos");
        }

        // GET: Carrito
        public ActionResult IndexCart()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            ViewBag.Total = carrito.Sum(c => (c.Cantidad ?? 0) * c.Producto.Precio); // Manejo de null en Cantidad
            return View(carrito);
        }

        // GET: Contar los productos dentro del carrito
        [HttpGet]
        public ActionResult ContarProductos()
        {
            // Suma las cantidades ignorando nulos de forma segura
            int totalProductos = db.Carritos
                                   .Where(c => c.Cantidad.HasValue && c.Cantidad > 0) // Filtra nulos y valores negativos
                                   .ToList() // Trae los datos a memoria
                                   .Sum(c => c.Cantidad ?? 0); // Realiza la suma manejando null

            return Json(new { total = totalProductos }, JsonRequestBehavior.AllowGet);
        }

        // POST: Agregar al carrito
        [HttpPost]
        public ActionResult Agregar(int productoId, int cantidad)
        {
            if (cantidad <= 0)
            {
                TempData["Error"] = "La cantidad debe ser mayor a 0.";
                return RedirectToAction("IndexCart");
            }

            var producto = db.Productos.Find(productoId);

            if (producto != null && producto.DisponibilidadInventario >= cantidad)
            {
                var carritoItem = db.Carritos.FirstOrDefault(c => c.CodigoProducto == productoId);

                if (carritoItem != null)
                {
                    carritoItem.Cantidad = (carritoItem.Cantidad ?? 0) + cantidad; // Suma segura
                }
                else
                {
                    db.Carritos.Add(new Carrito
                    {
                        CodigoProducto = productoId,
                        Cantidad = cantidad
                    });
                }

                producto.DisponibilidadInventario -= cantidad;
                db.SaveChanges();
            }
            else
            {
                TempData["Error"] = "No hay suficiente inventario disponible.";
            }

            return RedirectToAction("IndexCart");
        }

        // POST: Finalizar Compra
        [HttpPost]
        public ActionResult FinalizarCompra()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            if (carrito.Count > 0)
            {
                var historialVenta = new HistorialVenta
                {
                    CantidadProductosVendidos = carrito.Sum(c => c.Cantidad ?? 0), // Suma segura
                    GananciaTotal = carrito.Sum(c => (c.Cantidad ?? 0) * c.Producto.Precio),
                    Ventas = carrito.Select(c => new Ventas
                    {
                        CodigoProducto = c.CodigoProducto,
                        Cantidad = c.Cantidad ?? 0,
                        PrecioUnitario = c.Producto.Precio
                    }).ToList()
                };

                db.HistorialVentas.Add(historialVenta);
                db.Carritos.RemoveRange(carrito);
                db.SaveChanges();
            }

            TempData["Mensaje"] = "Compra realizada con éxito.";

            return RedirectToAction("Index", "Productos");
        }

        // POST: Eliminar una unidad del producto en el carrito
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var item = db.Carritos.Find(id);

            if (item != null)
            {
                // Resta solo una unidad del producto
                if ((item.Cantidad ?? 0) > 1)
                {
                    item.Cantidad -= 1;
                }
                else
                {
                    // Si la cantidad es 1, elimina la fila completa del carrito
                    db.Carritos.Remove(item);
                }

                // Incrementa el inventario del producto
                var producto = db.Productos.Find(item.CodigoProducto);
                if (producto != null)
                {
                    producto.DisponibilidadInventario += 1;
                }

                db.SaveChanges();
            }

            return RedirectToAction("IndexCart");
        }
    }

}
