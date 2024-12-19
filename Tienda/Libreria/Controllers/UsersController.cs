using Libreria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Libreria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
       

        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        // Listar usuarios
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Editar usuario (GET)
        public ActionResult Edit(string id)
        {
            var user = _userManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }



            // Obtener roles del usuario
            var roles = _userManager.GetRoles(user.Id);
            var allRoles = _roleManager.Roles.ToList();

            // Pasar los roles a la vista
            ViewBag.UserRoles = roles; // Roles asignados al usuario
            ViewBag.AllRoles = allRoles;   // Todos los roles disponibles
            ViewBag.SelectedRole = roles.FirstOrDefault();

            return View(user);
        }

        // Editar usuario (POST)
         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser userModel, string selectedRole)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = _userManager.FindById(userModel.Id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Actualizar email y nombre de usuario
            user.Email = userModel.Email;
            user.UserName = userModel.UserName;

            // Eliminar roles actuales y asignar el nuevo rol
            var currentRoles = _userManager.GetRoles(user.Id);
            _userManager.RemoveFromRoles(user.Id, currentRoles.ToArray());

            if (!string.IsNullOrEmpty(selectedRole))
            {
                _userManager.AddToRole(user.Id, selectedRole);
            }

            // Actualizar el usuario
            var result = _userManager.Update(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "No se pudo actualizar el usuario");
            return View(userModel);
        }

        // Eliminar usuario
        public ActionResult Delete(string id)
        {
            var user = _userManager.FindById(id);
            if (user == null) return HttpNotFound();

            var result = _userManager.Delete(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(500, "Error al eliminar el usuario");
        }
    }
}

