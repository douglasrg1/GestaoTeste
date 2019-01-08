using Gestao.Models;
using GestaoUsuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Gestao.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult logar(Login login)
        {
            var user = Request["login"];
            var senha = sha256(Request["senha"]);

            dbcontextUser context = new dbcontextUser();
            var users = context.usuario.Where(u => u.cnpj == user && u.senha == senha).ToList();
            if (users.Count > 0)
            {
                Session["UsuarioLogado"] = users[0];
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = "usuario ou senha incorreto";
                return View("Index");
            }
        }
        private string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
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