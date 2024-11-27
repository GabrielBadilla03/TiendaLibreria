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
        private LibreriaDbContext db = new LibreriaDbContext();

        // GET: Ventas
        public ActionResult Index()
        {
            var ventas = db.Ventas.Include(v => v.HistorialVenta).Include(v => v.Producto);
            return View(ventas.ToList());
        }

        // GET: Ventas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ventas ventas = db.Ventas.Find(id);
            if (ventas == null)
            {
                return HttpNotFound();
            }
            return View(ventas);
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "HistorialVentasId");
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDetalle,IdPedido,CodigoProducto,Cantidad,PrecioUnitario,Subtotal,HistorialVentasId")] Ventas ventas)
        {
            if (ModelState.IsValid)
            {
                db.Ventas.Add(ventas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "HistorialVentasId", ventas.HistorialVentasId);
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", ventas.CodigoProducto);
            return View(ventas);
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ventas ventas = db.Ventas.Find(id);
            if (ventas == null)
            {
                return HttpNotFound();
            }
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "HistorialVentasId", ventas.HistorialVentasId);
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", ventas.CodigoProducto);
            return View(ventas);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDetalle,IdPedido,CodigoProducto,Cantidad,PrecioUnitario,Subtotal,HistorialVentasId")] Ventas ventas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ventas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HistorialVentasId = new SelectList(db.HistorialVentas, "HistorialVentasId", "HistorialVentasId", ventas.HistorialVentasId);
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "NombreProducto", ventas.CodigoProducto);
            return View(ventas);
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ventas ventas = db.Ventas.Find(id);
            if (ventas == null)
            {
                return HttpNotFound();
            }
            return View(ventas);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ventas ventas = db.Ventas.Find(id);
            db.Ventas.Remove(ventas);
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
