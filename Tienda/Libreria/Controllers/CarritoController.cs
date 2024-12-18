using Libreria.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Libreria.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Carrito
        public ActionResult IndexCart()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            ViewBag.Total = carrito.Sum(c => (c.Cantidad ?? 0) * c.Producto.Precio);
            return View(carrito);
        }

        // GET: Contar los productos dentro del carrito
        [HttpGet]
        public ActionResult ContarProductos()
        {
            int totalProductos = db.Carritos
                                   .Where(c => c.Cantidad.HasValue && c.Cantidad > 0)
                                   .ToList()
                                   .Sum(c => c.Cantidad ?? 0);

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
                    carritoItem.Cantidad = (carritoItem.Cantidad ?? 0) + cantidad;
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
                    CantidadProductosVendidos = carrito.Sum(c => c.Cantidad ?? 0),
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

            GenerarFactura();
            TempData["Mensaje"] = "Compra realizada con éxito.";
            return RedirectToAction("Index", "Productos");
        }

        // POST: Cancelar Compra
        [HttpPost]
        public ActionResult CancelarCompra()
        {
            TempData["Info"] = "Compra cancelada.";
            GenerarFactura();
            return RedirectToAction("Index", "Productos");
        }

        // GET: Procesar Pago
        [HttpGet]
        public ActionResult ProcesarPago()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            ViewBag.Total = carrito.Sum(c => (c.Cantidad ?? 0) * c.Producto.Precio);
            return View(carrito);
        }

        // POST: Eliminar una unidad del producto en el carrito
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var item = db.Carritos.Find(id);

            if (item != null)
            {
                if ((item.Cantidad ?? 0) > 1)
                {
                    item.Cantidad -= 1;
                }
                else
                {
                    db.Carritos.Remove(item);
                }

                var producto = db.Productos.Find(item.CodigoProducto);
                if (producto != null)
                {
                    producto.DisponibilidadInventario += 1;
                }

                db.SaveChanges();
            }

            return RedirectToAction("IndexCart");
        }

        // GET: Generar Factura
        public ActionResult GenerarFactura()
        {
            var carrito = db.Carritos.Include(c => c.Producto).ToList();
            if (carrito.Count == 0)
            {
                TempData["Error"] = "El carrito está vacío, no se puede generar la factura.";
                return RedirectToAction("IndexCart");
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

                doc.Open();

                doc.Add(new Paragraph("Comprobante de Compra")
                {
                    Alignment = Element.ALIGN_CENTER,
                    Font = FontFactory.GetFont("Arial", 18, Font.BOLD)
                });
                doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}")
                {
                    Alignment = Element.ALIGN_RIGHT
                });
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3, 1, 1, 1, 1, 1 });

                table.AddCell(new PdfPCell(new Phrase("Producto", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Precio", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Metodo Pago", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Estado Compra", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                { HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = BaseColor.LIGHT_GRAY });

                decimal total = 0;

                foreach (var item in carrito)
                {
                    decimal subtotal = (item.Cantidad ?? 0) * item.Producto.Precio;

                    table.AddCell(new PdfPCell(new Phrase(item.Producto.NombreProducto)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase((item.Cantidad ?? 0).ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(item.Producto.Precio.ToString("C"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(subtotal.ToString("C"))) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Credito")) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase("Cancelado")) { HorizontalAlignment = Element.ALIGN_CENTER });

                    total += subtotal;
                }

                doc.Add(table);
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Total: {total:C}")
                {
                    Alignment = Element.ALIGN_RIGHT,
                    Font = FontFactory.GetFont("Arial", 14, Font.BOLD)
                });

                doc.Close();
                return File(memoryStream.ToArray(), "application/pdf", "Factura.pdf");
            }
        }
    }
}