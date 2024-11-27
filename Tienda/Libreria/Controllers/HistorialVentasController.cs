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
    public class HistorialVentasController : Controller
    {
        private LibreriaDbContext db = new LibreriaDbContext();

        // GET: HistorialVentas
        public ActionResult Index()
        {
            return View(db.HistorialVentas.ToList());
        }

        // GET: HistorialVentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialVenta historialVenta = db.HistorialVentas.Find(id);
            if (historialVenta == null)
            {
                return HttpNotFound();
            }
            return View(historialVenta);
        }

        // GET: HistorialVentas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HistorialVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistorialVentasId,FechaRegistro,CantidadProductosVendidos,GananciaTotal")] HistorialVenta historialVenta)
        {
            if (ModelState.IsValid)
            {
                db.HistorialVentas.Add(historialVenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(historialVenta);
        }

        // GET: HistorialVentas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialVenta historialVenta = db.HistorialVentas.Find(id);
            if (historialVenta == null)
            {
                return HttpNotFound();
            }
            return View(historialVenta);
        }

        // POST: HistorialVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistorialVentasId,FechaRegistro,CantidadProductosVendidos,GananciaTotal")] HistorialVenta historialVenta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialVenta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(historialVenta);
        }

        // GET: HistorialVentas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialVenta historialVenta = db.HistorialVentas.Find(id);
            if (historialVenta == null)
            {
                return HttpNotFound();
            }
            return View(historialVenta);
        }

        // POST: HistorialVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialVenta historialVenta = db.HistorialVentas.Find(id);
            db.HistorialVentas.Remove(historialVenta);
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
