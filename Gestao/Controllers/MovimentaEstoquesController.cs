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
    public class MovimentaEstoquesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index()
        {
            var movimentaEstoque = db.movimentaEstoque.Include(m => m.Fornecedor);
            return View(movimentaEstoque.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimentaEstoque movimentaEstoque = db.movimentaEstoque.Find(id);
            if (movimentaEstoque == null)
            {
                return HttpNotFound();
            }
            return View(movimentaEstoque);
        }

        
        public ActionResult Create()
        {
            List<SelectListItem> tipomov = new List<SelectListItem>();
            tipomov.Add(new SelectListItem
            {
                Text = "Entrada",
                Value = "Entrada",
                Selected = false
            });
            tipomov.Add(new SelectListItem
            {
                Text = "Saída",
                Value = "Saída",
                Selected = false
            });
            
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial");
            ViewBag.Produto = new SelectList(db.Produto, "id", "nome");
            ViewBag.tipoMovimentacao = tipomov;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( MovimentaEstoque movimentaEstoque)
        {
            var dados = dadosRecebidos();

            if (ModelState.IsValid)
            {
                db.movimentaEstoque.Add(movimentaEstoque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            return View(movimentaEstoque);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimentaEstoque movimentaEstoque = db.movimentaEstoque.Find(id);
            if (movimentaEstoque == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            return View(movimentaEstoque);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMovimentacao,datamovimentacao,tipoMovimentacao,nrDocumento,totalMovimentacao,valorFrete,valorIpi,valorIcms,cfop,idFornecedor")] MovimentaEstoque movimentaEstoque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimentaEstoque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            return View(movimentaEstoque);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimentaEstoque movimentaEstoque = db.movimentaEstoque.Find(id);
            if (movimentaEstoque == null)
            {
                return HttpNotFound();
            }
            return View(movimentaEstoque);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovimentaEstoque movimentaEstoque = db.movimentaEstoque.Find(id);
            db.movimentaEstoque.Remove(movimentaEstoque);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public decimal retornarValorProduto(int id)
        {
            var valorProduto = db.Produto.Where(p => p.id == id).First().valorVenda;
            return valorProduto;
        }
        public dadosRecebidosMovimentacao dadosRecebidos()
        {
            dadosRecebidosMovimentacao dadosRecebidosmov = new dadosRecebidosMovimentacao();
            dadosMovimentacao dadosMovimentacao = new dadosMovimentacao();
            produtosMovimentacao produtosMovimentacao = new produtosMovimentacao();

            //dados movimentacao
            dadosMovimentacao.idFornecedor = Convert.ToInt32(Request["dadosRecebidos[dadosMovimentacao][idForncedor]"]);
            dadosMovimentacao.numeroDocumento = Request["dadosRecebidos[dadosMovimentacao][numeroDocumento]"];
            dadosMovimentacao.totalMovimentacao = Convert.ToDecimal(Request["dadosRecebidos[dadosMovimentacao][totalMovimentacao]"]);
            dadosMovimentacao.valorFrete = Convert.ToDecimal(Request["dadosRecebidos[dadosMovimentacao][valorFrete]"]);
            dadosMovimentacao.tipoMovimentacao = Request["dadosRecebidos[dadosMovimentacao][tipoMovimentacao]"];
            dadosRecebidosmov.dadosMovimentacao = dadosMovimentacao;
            //dados produtos
            var produtoMov = this.Request.Form.AllKeys.Where(k => k.Contains("dadosRecebidos[produtosMovimentacao]")).ToList();
            var quantKeys = (produtoMov.Count) / 10;

            if (quantKeys == 0)
                return dadosRecebidosmov;

            dadosRecebidosmov.produtosMovimentacao = new List<produtosMovimentacao>();

            for (var i = 0; i < quantKeys; i++)
            {
                produtosMovimentacao.idproduto = Convert.ToInt32(Request["dadosRecebidos[produtosMovimentacao][" + i + "][idproduto]"]);
                produtosMovimentacao.valorUnitario = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][valorUnitario]"]);
                produtosMovimentacao.quantidade = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][quantidade]"]);
                produtosMovimentacao.porcDesconto = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][porcDesconto]"]);
                produtosMovimentacao.valorDesconto = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][valorDesconto]"]);
                produtosMovimentacao.valorTotal = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][valorTotal]"]);
                produtosMovimentacao.obs = Request["dadosRecebidos[produtosMovimentacao][" + i + "][obs]"];
                produtosMovimentacao.valorIpi = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][valorIpi]"]);
                produtosMovimentacao.valorIcms = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][valorIcms]"]);
                produtosMovimentacao.Cfop = Convert.ToDecimal(Request["dadosRecebidos[produtosMovimentacao][" + i + "][cfop]"]);

                dadosRecebidosmov.produtosMovimentacao.Add(produtosMovimentacao);

            }




            return dadosRecebidosmov;
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

public struct dadosMovimentacao
{
    public decimal totalMovimentacao { get; set; }
    public string numeroDocumento { get; set; }
    public decimal valorFrete { get; set; }
    public int idFornecedor { get; set; }
    public string tipoMovimentacao { get; set; }



}
public struct produtosMovimentacao
{
    public int idproduto { get; set; }
    public string obs { get; set; }
    public decimal porcDesconto { get; set; }
    public decimal quantidade { get; set; }
    public decimal valorDesconto { get; set; }
    public decimal valorTotal { get; set; }
    public decimal valorUnitario { get; set; }
    public decimal valorIcms { get; set; }
    public decimal valorIpi { get; set; }
    public decimal Cfop { get; set; }

}
public struct dadosRecebidosMovimentacao
{
    public dadosMovimentacao dadosMovimentacao;
    public List<produtosMovimentacao> produtosMovimentacao;
}
