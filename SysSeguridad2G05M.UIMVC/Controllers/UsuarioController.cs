using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SysSeguridad2G05M.EN;
using SysSeguridad2G05M.BL;

namespace SysSeguridad2G05M.UIMVC.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioBL usuariobl = new UsuarioBL();
        RolBL rolBL = new RolBL();
        // GET: UsuarioController
        public  async Task<IActionResult> Index(Usuario pUsuario = null)
        {
            if (pUsuario == null)
                pUsuario = new Usuario();
            if (pUsuario.Top_Aux == 0)
                pUsuario.Top_Aux = 10;
            else
                if (pUsuario.Top_Aux == -1)
                    pUsuario.Top_Aux = 0;

            var taksBuscar = await usuariobl.BuscarIncluirRolesAsync(pUsuario);
            var taskObtenerRoles = rolBL.ObtenerTodosAsync();

            ViewBag.Top = pUsuario.Top_Aux;
            ViewBag.Roles = await taskObtenerRoles;

            return View(taksBuscar);
        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsuarioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
