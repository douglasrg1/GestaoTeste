using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestao.Models;
using GestaoUsuarios;
using System.Security.Cryptography;
using System.Text;

namespace Gestao.Controllers
{
    public class UsuarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.estado = new SelectList(db.Estados.ToList(), "Uf", "Nome");
            ViewBag.cidade = new List<SelectListItem>()
            {
                new SelectListItem {Text= "",Value="", Selected = false }
            };
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            usuario.senha = sha256(usuario.senha);
            Principal p = new Principal();
            var resp = p.verificaCnpjCadastrado(usuario.cnpj);
            if(resp == true)
            {
                ViewBag.estado = new SelectList(db.Estados.ToList(), "Uf", "Nome");

                ViewBag.cidade = new List<SelectListItem>()
                {
                    new SelectListItem {Text= "",Value="", Selected = false }
                };

                ViewBag.cnpjcadastrado = "CNPJ informado já está cadastrado no sistema";
                return View(usuario);
            }


            if (ModelState.IsValid)
            {
                
                db.usuario.Add(usuario);
                db.SaveChanges();
                p.Criar(convertUsers(usuario));
                return RedirectToAction("Index");

            }

            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            ViewBag.estado = new SelectList(db.Estados.ToList(), "Uf", "Nome");
            ViewBag.cidade = new SelectList(db.Municipios.Where(m => m.Uf == usuario.estado).ToList(), "Nome", "Nome");
            
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
        public ActionResult Listarcidades(string siglauf)
        {
            var itens = db.Municipios.Where(m => m.Uf == siglauf).ToList();
            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Nome,  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            return Json(listItens, JsonRequestBehavior.AllowGet);
        }
        private GestaoUsuarios.Usuarios.Usuarios convertUsers(Usuario usuario)
        {
            GestaoUsuarios.Usuarios.Usuarios user = new GestaoUsuarios.Usuarios.Usuarios();
            user.bairro = usuario.bairro;
            user.cidade = usuario.cidade;
            user.cnpj = usuario.cnpj;
            user.email = usuario.email;
            user.endereco = usuario.endereco;
            user.estado = usuario.estado;
            user.idUsuario = usuario.idUsuario;
            user.numero = usuario.numero;
            user.razaoSocial = usuario.razaoSocial;
            user.senha = usuario.senha;
            user.telefoneContato1 = usuario.telefoneContato1;
            user.telefoneContato2 = usuario.telefoneContato2;

            return user;
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
