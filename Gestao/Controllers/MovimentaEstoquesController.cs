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
  
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial");
            ViewBag.Produto = new SelectList(db.Produto, "id", "nome");
            ViewBag.tipoMovimentacao = tipomov();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( MovimentaEstoque movimentaEstoque)
        {
            var dados = dadosRecebidos();
            var idmov = salvaMovimentacao(dados.dadosMovimentacao);

            if (idmov != 0)
            {
                if(dados.produtosMovimentacao != null)
                {
                    var resposta = salvaProdutosMovimentacao(idmov, dados.produtosMovimentacao);

                    if (resposta)
                        TempData["msgsucesso"] = "Movimentação Inserida com sucesso.";
                    else
                        TempData["msgsucesso"] = "Erro ao adicionar os produtos na movimentação, confira os dados e tente novamente.";
                }
                else
                    TempData["msgsucesso"] = "Movimentação Inserida com sucesso.";
            }
            else
                TempData["msg"] = "Erro ao salvar a movimentação, confira os dados e tente novamente.";


            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            ViewBag.Produto = new SelectList(db.Produto, "id", "nome");
            ViewBag.tipoMovimentacao = tipomov();
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
                produtosMovimentacao.Cfop = Request["dadosRecebidos[produtosMovimentacao][" + i + "][cfop]"];

                dadosRecebidosmov.produtosMovimentacao.Add(produtosMovimentacao);

            }




            return dadosRecebidosmov;
        }
        private int salvaMovimentacao(dadosMovimentacao dados)
        {
            var resposta = true;
            MovimentaEstoque movestoque = new MovimentaEstoque();
            movestoque.datamovimentacao = DateTime.Now;
            movestoque.idFornecedor = dados.idFornecedor;
            movestoque.Fornecedor = db.Fornecedor.Find(dados.idFornecedor);
            movestoque.nrDocumento = dados.numeroDocumento;
            movestoque.tipoMovimentacao = dados.tipoMovimentacao;
            movestoque.totalMovimentacao = dados.totalMovimentacao;
            movestoque.valorFrete = dados.valorFrete;

            db.movimentaEstoque.Add(movestoque);

            var a = db.SaveChanges();

            return movestoque.idMovimentacao;
        }
        private bool salvaProdutosMovimentacao(int idmovimentacao,List<produtosMovimentacao> produtos)
        {
            var resposta = true;
            ProdutosMovimentacao produtosmov;

            foreach(var item in produtos)
            {
                produtosmov = (new ProdutosMovimentacao
                {
                    CFOP = item.Cfop,
                    idMovimentacaoEstoque = idmovimentacao,
                    quantidade = item.quantidade,
                    valorDesconto = item.valorDesconto,
                    idProduto = item.idproduto,
                    valorICMS = item.valorIcms,
                    valorIPI = item.valorIpi,
                    valorTotal = item.valorTotal,
                    valorUnitario = item.valorUnitario
                    
                });

                db.produtosMovimentacao.Add(produtosmov);
                if (db.SaveChanges() == 0)
                    return false;
            }
            

            return resposta;
        }
        private List<SelectListItem> tipomov()
        {
            List<SelectListItem> tipomov = new List<SelectListItem>();
            tipomov.Add(new SelectListItem
            {
                Text = "Entrada",
                Value = "Entrada",
                Selected = true
            });
            tipomov.Add(new SelectListItem
            {
                Text = "Saída",
                Value = "Saída",
                Selected = false
            });

            return tipomov;
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
    public string Cfop { get; set; }

}
public struct dadosRecebidosMovimentacao
{
    public dadosMovimentacao dadosMovimentacao;
    public List<produtosMovimentacao> produtosMovimentacao;
}
