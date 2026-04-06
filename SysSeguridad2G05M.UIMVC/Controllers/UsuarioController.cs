using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SysSeguridad2G05M.EN;
using SysSeguridad2G05M.BL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Runtime.Intrinsics.X86;

namespace SysSeguridad2G05M.UIMVC.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await rolBL.ObtenerTodosAsync();
            ViewBag.Error = "";
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario pUsuario)
        {
            try
            {
                var usuario = await usuariobl.BuscarAsync(
                    new Usuario { Login = User.Identity.Name, Top_Aux = 1}
                    );
                int result = await usuariobl.CrearAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await rolBL.ObtenerTodosAsync();
                return View(pUsuario);
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnURL = null)
        {
            ViewBag.Url = ReturnURL;
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Usuario pUsuario,string ReturnURL = null)
        {
            try
            {
                var usuario = await usuariobl.LoginAsync(pUsuario);
                if (usuario != null && usuario.Id > 0 && usuario.Login == pUsuario.Login)
                {
                    usuario.Rol = await rolBL.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
                    var claims = new[] {
                                        new Claim(ClaimTypes.Name, usuario.Login),
                                        new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
                                       };
                    var identity = new ClaimsIdentity(
                                                        claims,
                                                        CookieAuthenticationDefaults.AuthenticationScheme
                                                        );
                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity)
                        );
                }
                else
                    throw new Exception("Credenciales Incorrectas");

                if (!string.IsNullOrWhiteSpace(ReturnURL))
                    return Redirect(ReturnURL);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Url = ReturnURL;
                ViewBag.Error = ex.Message;
                return View(new Usuario { Login = pUsuario.Login });
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesion(string pReturnURL = null)
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }
    }
}
