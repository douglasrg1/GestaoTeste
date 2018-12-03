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
    public class FuncionariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Funcionarios
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.Funcionario.ToList());
        }

        // GET: Funcionarios/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionario.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public ActionResult Create()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var itens = db.Estados.ToList();

            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Uf,  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            IList<SelectListItem> listItens2 = new List<SelectListItem>();

            listItens2.Add(new SelectListItem
            {
                Text = "",   // Valor que será o Texto do Dropdown
                Value = "",  // Valor que será o Value do Dropdown
                Selected = false            // Indica se o item será selecionado por padrão no Dropdown
            });
            ViewBag.ufs = listItens;
            ViewBag.Cidades = listItens2;

            return View();
        }

        // POST: Funcionarios/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nome,cpfcnpj,salario,rua,numero,bairro,estado,cidade,telefone1,telefone2,email,dataEmissao,dataDemissao")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                funcionario.dataEmissao = DateTime.Now;
                db.Funcionario.Add(funcionario);
                if (db.SaveChanges() != 0)
                    TempData["message"] = "Funcionario cadastrado com sucesso!";
                else
                    TempData["message"] = "Erro ao cadastrar funcionario!";

                return RedirectToAction("Index");
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionario.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            var itens = db.Estados.ToList();

            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Uf,  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            var municipios = db.Municipios.Where(m => m.Uf == funcionario.estado).ToList();

            IList<SelectListItem> listItens2 = new List<SelectListItem>();

            foreach (var item in municipios)
            {
                listItens2.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Nome,  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            ViewBag.ufs = listItens;
            ViewBag.Cidades = listItens2;

            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,cpfcnpj,salario,rua,numero,bairro,estado,cidade,telefone1,telefone2,email,dataEmissao,dataDemissao")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funcionario).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                    TempData["message"] = "Cadastro editado com Sucesso!";
                else
                    TempData["message"] = "Erro ao editar cadastro!";

                return RedirectToAction("Index");
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionario.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Funcionario funcionario = db.Funcionario.Find(id);
            db.Funcionario.Remove(funcionario);
            if (db.SaveChanges() != 0)
                TempData["message"] = "Registro removido com sucesso!";
            else
                TempData["message"] = "Erro ao remover registro selecionado!";

            return RedirectToAction("Index");
        }
        public ActionResult Listarcidades(string siglauf)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var itens = db.Municipios.Where(m => m.Uf == siglauf).ToList();
            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Nome,  // Valor que será o Value do Dropdown
                    Selected = false    // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            return Json(listItens, JsonRequestBehavior.AllowGet);
        }
        public JsonResult listarFuncionarios(int current, int rowCount, string searchPhrase)
        {
            

            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var funcionarios = db.Funcionario.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                funcionarios = funcionarios.Where("nome.Contains(@0) OR cpfcnpj.Contains(@0)", searchPhrase).ToList();
            }

            var funcionariosPaginados = funcionarios.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);


            return Json(new { current = current, rowCount = rowCount, rows = funcionariosPaginados, total = funcionarios.Count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult baixaFuncionario()
        {

            return Json("", JsonRequestBehavior.AllowGet);
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
