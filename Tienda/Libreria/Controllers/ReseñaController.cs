using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Libreria.Models;

namespace Libreria.Controllers
{
    public class ReseñaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reseña
        public ActionResult Index()
        {
            var reseñas = db.Reseñas.Include(r => r.Usuario).Include(r => r.Producto);
            return View(reseñas.ToList());
        }

        // GET: Reseña/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseñas
                      .Include(r => r.Usuario) // Cargar relación con Usuario
                      .Include(r => r.Producto) // Cargar relación con Producto
                      .FirstOrDefault(r => r.ReseñaId == id);
            if (reseña == null)
            {
                return HttpNotFound();
            }
            return View(reseña);
        }

        // GET: Reseña/Create
        public ActionResult Create()
        {
            ViewBag.CodigoCliente = new SelectList(db.Users, "Id", "UserName");
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto");
            return View();
        }

        // POST: Reseña/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReseñaId,CodigoProducto,CodigoCliente,Comentario,Calificacion,FechaReseña")] Reseña reseña)
        {
            if (ModelState.IsValid)
            {
                db.Reseñas.Add(reseña);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", reseña.CodigoProducto);
            return View(reseña);
        }

        // GET: Reseña/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseñas.Find(id);
            if (reseña == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", reseña.CodigoProducto);
            return View(reseña);
        }

        // POST: Reseña/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReseñaId,CodigoProducto,CodigoCliente,Comentario,Calificacion,FechaReseña")] Reseña reseña)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reseña).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", reseña.CodigoProducto);
            return View(reseña);
        }

        // GET: Reseña/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseña reseña = db.Reseñas
                      .Include(r => r.Usuario) // Cargar relación con Usuario
                      .Include(r => r.Producto) // Cargar relación con Producto
                      .FirstOrDefault(r => r.ReseñaId == id);
            if (reseña == null)
            {
                return HttpNotFound();
            }
            return View(reseña);
        }

        // POST: Reseña/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reseña reseña = db.Reseñas.Find(id);
            db.Reseñas.Remove(reseña);
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
