using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gestao.Models;

namespace Gestao.Controllers
{
    public class FornecedoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fornecedores
        public ActionResult Index()
        {
            return View(db.Fornecedor.ToList());
        }

        // GET: Fornecedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return View(fornecedor);
        }

        // GET: Fornecedores/Create
        public ActionResult Create()
        {
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

        // POST: Fornecedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,razaoSocial,nomeFantasia,cnpj,inscricaoEstadual,inscricaoMunicipal,rua,numero,bairro,complemento,estado,cidade,telefone1,telefone2,email,dataCadastro")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                fornecedor.dataCadastro = DateTime.Today;
                db.Fornecedor.Add(fornecedor);

                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Fornecedor cadastrado com sucesso";
                else
                    TempData["mensagem"] = "Falha ao cadastrar fornecedor";

                return RedirectToAction("Index");
            }

            return View(fornecedor);
        }

        // GET: Fornecedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
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

            var municipios = db.Municipios.Where(m => m.Uf == fornecedor.estado);

            foreach (var item in municipios)
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

            return View(fornecedor);
        }

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,razaoSocial,nomeFantasia,cnpj,inscricaoEstadual,inscricaoMunicipal,rua,numero,bairro,complemento,estado,cidade,telefone1,telefone2,email,dataCadastro")] Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fornecedor).State = EntityState.Modified;

                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Cadastro editado com Sucesso";
                else
                    TempData["mensagem"] = "Erro ao editar cadastro";

                return RedirectToAction("Index");
            }
            return View(fornecedor);
        }

        // GET: Fornecedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            if (fornecedor == null)
            {
                return HttpNotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fornecedor fornecedor = db.Fornecedor.Find(id);
            db.Fornecedor.Remove(fornecedor);

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

            foreach (var item in cidades)
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
        public JsonResult listarFornecedores(int current, int rowCount, string searchPhrase)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var fornecedores = db.Fornecedor.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                fornecedores = fornecedores.Where("razaoSocial.Contains(@0) OR nomeFantasia.Contains(@0) OR cnpj.Contains(@0)  OR estado.Contains(@0) OR cidade.Contains(@0) OR telefone1.Contains(@0) ", searchPhrase).ToList();
            }

            var fornecedoresPaginados = fornecedores.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);


            return Json(new { current = current, rowCount = rowCount, rows = fornecedoresPaginados, total = fornecedores.Count }, JsonRequestBehavior.AllowGet);
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
