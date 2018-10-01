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
using Newtonsoft.Json;

namespace Gestao.Controllers
{
    public class ClientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clientes
        public ActionResult Index()
        {

            return View(db.Cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
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

            IList<SelectListItem> listItens3 = new List<SelectListItem>();

            listItens3.Add(new SelectListItem
            {
                Text = "físico",   // Valor que será o Texto do Dropdown
                Value = "físico",  // Valor que será o Value do Dropdown
                Selected = true            // Indica se o item será selecionado por padrão no Dropdown
            });

            listItens3.Add(new SelectListItem
            {
                Text = "Jurídico",   // Valor que será o Texto do Dropdown
                Value = "Jurídico",  // Valor que será o Value do Dropdown
                Selected = false            // Indica se o item será selecionado por padrão no Dropdown
            });
            


            ViewBag.ufs = listItens;
            ViewBag.Cidades = listItens2;
            ViewBag.tipoPessoa = listItens3;

            return View();
        }

        // POST: Clientes/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,TipoCliente,CnpjCpf,Rua,Numero,Bairro,cidade,Estado,Telefone1,Telefone2,Email,DataCadastro,DataUtimaCompra")] Cliente cliente)
        {

            if (ModelState.IsValid)
            {
                
                cliente.DataCadastro = DateTime.Today;
                db.Cliente.Add(cliente);
                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Cliente cadastrado com sucesso";
                else
                    TempData["mensagem"] = "Falha ao cadastrar cliente";

            }
            

            return RedirectToAction("index");
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = db.Cliente.Find(id);

            if (cliente == null)
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

            var municipios = db.Municipios.Where(m => m.Uf == cliente.Estado).ToList();

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

            IList<SelectListItem> listItens3 = new List<SelectListItem>();

            listItens3.Add(new SelectListItem
            {
                Text = "físico",   // Valor que será o Texto do Dropdown
                Value = "físico",  // Valor que será o Value do Dropdown
                Selected = true            // Indica se o item será selecionado por padrão no Dropdown
            });

            listItens3.Add(new SelectListItem
            {
                Text = "Jurídico",   // Valor que será o Texto do Dropdown
                Value = "Jurídico",  // Valor que será o Value do Dropdown
                Selected = false            // Indica se o item será selecionado por padrão no Dropdown
            });

            ViewBag.ufs = listItens;
            ViewBag.Cidades = listItens2;
            ViewBag.tipoPessoa = listItens3;

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,TipoCliente,CnpjCpf,Rua,Numero,Bairro,cidade,Estado,Telefone1,Telefone2,Email,DataCadastro,DataUtimaCompra")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Cadastro editado com Sucesso";
                else
                    TempData["mensagem"] = "Erro ao editar cadastro";

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            if (db.SaveChanges() != 0)
                TempData["mensagem"] = "Cliente removido com Sucesso";
            else
                TempData["mensagem"] = "Erro ao remover cliente";
            
            return RedirectToAction("Index");
        }
        [HttpGet]
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
        public JsonResult listarClientes(int current, int rowCount,string searchPhrase)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var clientes = db.Cliente.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                clientes = clientes.Where("Nome.Contains(@0) OR TipoCliente.Contains(@0) OR CnpjCpf.Contains(@0)  OR Rua.Contains(@0) OR Bairro.Contains(@0) OR cidade.Contains(@0) ", searchPhrase).ToList();
            }

            var clientesPaginados = clientes.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            

            return Json(new { current = current, rowCount = rowCount, rows = clientesPaginados, total = clientes.Count }, JsonRequestBehavior.AllowGet);
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
