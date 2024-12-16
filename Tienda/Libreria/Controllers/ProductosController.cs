using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;

namespace Libreria.Controllers
{
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Productos
        public ActionResult Index()
        {
            var productos = db.Productos.Include(p => p.Categoria);

            return View(productos.ToList());
        }

        // GET: Productos/Details/5
        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Carga ansiosa de la categoría asociada al producto
            Producto producto = db.Productos
                                 .Include(p => p.Categoria) // Incluir la relación
                                 .FirstOrDefault(p => p.CodigoProducto == id);

            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }


        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria");
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoProducto,NombreProducto,Descripcion,Descuento,Precio,DisponibilidadInventario,Estado,CategoriaId")] Producto producto, HttpPostedFileBase ImagenProducto)
        {
            if (ModelState.IsValid)
            {
                if (ImagenProducto != null && ImagenProducto.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(ImagenProducto.InputStream))
                    {
                        producto.ImagenProducto = binaryReader.ReadBytes(ImagenProducto.ContentLength);
                    }
                }
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoProducto,NombreProducto,Descripcion,Descuento,Precio,DisponibilidadInventario,Estado,CategoriaId")] Producto producto, HttpPostedFileBase ImagenProducto)
        {
            if (ModelState.IsValid)
            {
                if (ImagenProducto != null && ImagenProducto.ContentLength > 0)
                {
                    using (var binaryReader = new BinaryReader(ImagenProducto.InputStream))
                    {
                        producto.ImagenProducto = binaryReader.ReadBytes(ImagenProducto.ContentLength);
                    }
                }
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "CategoriaId", "NombreCategoria", producto.CategoriaId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto producto = db.Productos
                                 .Include(p => p.Categoria) // Incluir la relación
                                 .FirstOrDefault(p => p.CodigoProducto == id);

            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
