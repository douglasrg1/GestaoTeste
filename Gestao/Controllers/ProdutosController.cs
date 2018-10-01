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
    public class ProdutosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Produtos
        public ActionResult Index()
        {
            return View(db.Produto.ToList());
        }

        // GET: Produtos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nome,valorCompra,valorFrete,despesasAdicionaisdeCompra,valorVenda,despesasAdicionaisdeVenda,dataCadastro,dataUltimaEntrada,dataUltimaSaida,desconto,condigoReferencia,quantidadeEstoque,observacoes")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                var produtos = db.Produto.Where(p => p.nome == produto.nome).ToList();
                if (produtos.Count == 0)
                {
                    produto.dataCadastro = DateTime.Now;
                    produto.quantidadeEstoque = 0;
                    if (produto.condigoReferencia == null)
                        produto.condigoReferencia = "0";
                    db.Produto.Add(produto);
                    if (db.SaveChanges() != 0)
                        TempData["message"] = "Produto cadastrado com sucesso!";
                    else
                        TempData["message"] = "Erro ao realizar o cadastro do produto!";
                }
                else
                    TempData["erro"] = "Produto já cadastrado no sistema. Não é possivel cadastrar mais de um produto com o mesmo nome";
                
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,valorCompra,valorFrete,despesasAdicionaisdeCompra,valorVenda,despesasAdicionaisdeVenda,dataCadastro,dataUltimaEntrada,dataUltimaSaida,desconto,condigoReferencia,quantidadeEstoque,observacoes")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                    TempData["message"] = "Registro editado com sucesso!";
                else
                    TempData["message"] = "Erro ao editar registro selecionado!";
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produtos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
            if (db.SaveChanges() != 0)
                TempData["message"] = "Registro removido com sucesso!";
            else
                TempData["message"] = "Erro ao remover o registro selecionado!";

            return RedirectToAction("Index");
        }
        public JsonResult listarProdutos(int current, int rowCount, string searchPhrase)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var produtos = db.Produto.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                produtos = produtos.Where("nome.Contains(@0) OR condigoReferencia.Contains(@0)", searchPhrase).ToList();
            }

            var produtosPaginados = produtos.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);


            return Json(new { current = current, rowCount = rowCount, rows = produtosPaginados, total = produtos.Count }, JsonRequestBehavior.AllowGet);
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


