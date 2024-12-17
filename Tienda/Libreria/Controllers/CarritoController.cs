using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TuProyecto.Controllers
{
    public class CarritoController : Controller
    {
        // Acción para mostrar la vista del carrito
        public ActionResult IndexCart()
        {
            // Aquí puedes enviar datos adicionales si los necesitas
            return View();
        }
    }
}
