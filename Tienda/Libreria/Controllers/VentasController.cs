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
    public class VentasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ventas
        public ActionResult Index()
        {
            var ventas = db.Ventas.Include(v => v.Producto).Include(v => v.HistorialVenta).ToList();
            return View(ventas);
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var venta = db.Ventas
                          .Include(v => v.Producto)
                          .Include(v => v.HistorialVenta)
                          .FirstOrDefault(v => v.IdDetalle == id);

            if (venta == null)
                return HttpNotFound();

            return View(venta);
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto");
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "FechaRegistro");
            return View();
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDetalle,CodigoProducto,Cantidad,PrecioUnitario,HistorialVentasId")] Ventas venta)
        {
            if (ModelState.IsValid)
            {
                db.Ventas.Add(venta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", venta.CodigoProducto);
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "FechaRegistro", venta.HistorialVentasId);
            return View(venta);
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var venta = db.Ventas.Find(id);
            if (venta == null)
                return HttpNotFound();

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", venta.CodigoProducto);
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "FechaRegistro", venta.HistorialVentasId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDetalle,CodigoProducto,Cantidad,PrecioUnitario,HistorialVentasId")] Ventas venta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", venta.CodigoProducto);
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "FechaRegistro", venta.HistorialVentasId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var venta = db.Ventas
                          .Include(v => v.Producto)
                          .Include(v => v.HistorialVenta)
                          .FirstOrDefault(v => v.IdDetalle == id);

            if (venta == null)
                return HttpNotFound();

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var venta = db.Ventas.Find(id);
            db.Ventas.Remove(venta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}