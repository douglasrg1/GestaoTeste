using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestao.Models;

namespace Gestao.Controllers
{
    public class EmpresasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empresas
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.Empresa.ToList());
        }

        // GET: Empresas/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // GET: Empresas/Create
        public ActionResult Create()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            int idempresa = 0;
            var empresa = db.Empresa.ToList();
            if (empresa.Count > 0)
                idempresa = empresa.First().id;
            
            if ( idempresa != 0)
                return RedirectToAction("Edit",new {id = idempresa });

            var estados = db.Estados.ToList();

            List<SelectListItem> list = new List<SelectListItem>();
            
            foreach(var item in estados)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Nome,
                    Value = item.Uf,
                    Selected = false
                });
            }

            List<SelectListItem> list2 = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = "",
                Value = "",
                Selected = false
            });

            ViewBag.estados = list;
            ViewBag.cidades = list2;

            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,razaoSocial,nomeFantasia,cnpj,inscricaoEstadual,inscricaoMunicipal,rua,numero,bairro,complemento,estado,cidade,telefone1,telefone2,email")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Empresa.Add(empresa);
                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Empresa cadastrada com sucesso";
                else
                    TempData["mensagem"] = "Falha ao adicionar empresa";

                return RedirectToAction("Index");

            }

            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public ActionResult Edit(int id = 1)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }

            var estados = db.Estados.ToList();

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in estados)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Nome,
                    Value = item.Uf,
                    Selected = false
                });
            }

            List<SelectListItem> list2 = new List<SelectListItem>();
            var municipios = db.Municipios.Where(m => m.Uf == empresa.estado);

            foreach(var item in municipios)
            {
                list2.Add(new SelectListItem
                {
                    Text = item.Nome,
                    Value = item.Nome,
                    Selected = false
                });
            }

            

            ViewBag.estados = list;
            ViewBag.cidades = list2;

            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,razaoSocial,nomeFantasia,cnpj,inscricaoEstadual,inscricaoMunicipal,rua,numero,bairro,complemento,estado,cidade,telefone1,telefone2,email")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Cadastro editado com Sucesso";
                else
                    TempData["mensagem"] = "Erro ao editar cadastro";

                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = db.Empresa.Find(id);
            db.Empresa.Remove(empresa);
            if (db.SaveChanges() != 0)
                TempData["mensagem"] = "Cadastro removido com Sucesso";
            else
                TempData["mensagem"] = "Erro ao remover cadastro";

            return RedirectToAction("Index");
        }

        public JsonResult listarMunicipios(string uf)
        {
           
            var cidades = db.Municipios.Where(m => m.Uf == uf).ToList();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach(var item in cidades)
            {
                list.Add(new SelectListItem
                {
                    Text = item.Nome,
                    Value = item.Nome,
                    Selected = false
                });
            }

            return Json(list, JsonRequestBehavior.AllowGet);
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
