﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
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
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var movimentaEstoque = db.movimentaEstoque.Include(m => m.Fornecedor);
            return View(movimentaEstoque.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

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
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

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
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimentaEstoque movimentaEstoque = db.movimentaEstoque.Find(id);
            if (movimentaEstoque == null)
            {
                return HttpNotFound();
            }

            ViewBag.Produto = new SelectList(db.Produto, "id", "nome");
            ViewBag.tipoMovimentacao = tipomov();
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            return View(movimentaEstoque);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovimentaEstoque movimentaEstoque)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var dados = dadosRecebidos();
            movimentaEstoque.cfop = "";
            movimentaEstoque.datamovimentacao = db.movimentaEstoque.Find((int)dados.dadosMovimentacao.idMovimetacao).datamovimentacao;
            movimentaEstoque.Fornecedor = db.Fornecedor.Find((int)dados.dadosMovimentacao.idFornecedor);
            movimentaEstoque.idFornecedor = dados.dadosMovimentacao.idFornecedor;
            movimentaEstoque.idMovimentacao = dados.dadosMovimentacao.idMovimetacao;
            movimentaEstoque.nrDocumento = dados.dadosMovimentacao.numeroDocumento;
            movimentaEstoque.tipoMovimentacao = dados.dadosMovimentacao.tipoMovimentacao;
            movimentaEstoque.totalMovimentacao = dados.dadosMovimentacao.totalMovimentacao;
            movimentaEstoque.valorFrete = dados.dadosMovimentacao.valorFrete;
            movimentaEstoque.valorIcms = 0;
            movimentaEstoque.valorIpi = 0;

            db.Set<MovimentaEstoque>().AddOrUpdate(movimentaEstoque);

            if (db.SaveChanges() != 0)
            {
                removeProdutosMovimentacao(movimentaEstoque.idMovimentacao);
                ProdutosMovimentacao produtosMovimentacao = new ProdutosMovimentacao();

                if (dados.produtosMovimentacao != null)
                {
                    var resposta = salvaProdutosMovimentacao(movimentaEstoque.idMovimentacao, dados.produtosMovimentacao);

                    if (resposta)
                        TempData["msgsucesso"] = "Movimentação Atualizada com sucesso.";
                    else
                        TempData["msgsucesso"] = "Erro ao adicionar os produtos na movimentação, confira os dados e tente novamente.";
                }
                else
                    TempData["msgsucesso"] = "Movimentação Atualizada com sucesso.";

            }
            else
                TempData["msg"] = "Erro ao salvar a movimentação, confira os dados e tente novamente.";

            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", movimentaEstoque.idFornecedor);
            ViewBag.Produto = new SelectList(db.Produto, "id", "nome");
            ViewBag.tipoMovimentacao = tipomov();
            return View(movimentaEstoque);

        }

        
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

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
            if(db.SaveChanges() != 0)
                TempData["msgsucesso"] = "Movimentação Removida com sucesso.";

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
            dadosMovimentacao.idMovimetacao = Convert.ToInt32(Request["dadosRecebidos[dadosMovimentacao][idMovimentacao]"]);
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
                    valorUnitario = item.valorUnitario,
                    observacao = item.obs,
                    porcdesconto = item.porcDesconto
                    
                });

                db.produtosMovimentacao.Add(produtosmov);
                if (db.SaveChanges() != 0)
                {
                    var produto = db.Produto.Find(produtosmov.idProduto);
                    produto.quantidadeEstoque += item.quantidade;
                    db.Entry(produto).State = EntityState.Modified;
                    var resp = db.SaveChanges();
                }
                else
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
                Value = "Saida",
                Selected = false
            });

            return tipomov;
        }
        public JsonResult listarMovimentacoes(int current, int rowCount, string searchPhrase)
        {
            

            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var movimenacoes = db.movimentaEstoque.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                movimenacoes = movimenacoes.Where("Fornecedor.razaoSocial.Contains(@0)", searchPhrase).ToList();
            }

            var movpaginadas = movimenacoes.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            var movimentacoesp = retornaRowsTable(movpaginadas);

            return Json(new { current = current, rowCount = rowCount, rows = movimentacoesp, total = movimenacoes.Count }, JsonRequestBehavior.AllowGet);
        }
        private List<rowsTablemov> retornaRowsTable(IEnumerable<MovimentaEstoque> movestoque)
        {
            List<rowsTablemov> listaMovimentacoes = new List<rowsTablemov>();
            foreach (var i in movestoque)
            {
                listaMovimentacoes.Add(new rowsTablemov
                {
                    id = i.idMovimentacao,
                    datamovimentacao = i.datamovimentacao.ToString(),
                    nrDocumento = i.nrDocumento,
                    razaoSocial = i.Fornecedor != null ? i.Fornecedor.razaoSocial : "",
                    tipoMovimentacao = i.tipoMovimentacao,
                    totalMovimentacao = i.totalMovimentacao,
                    valorFrete = i.valorFrete
                });
            }

            return listaMovimentacoes;
        }
        public ActionResult retornaProdutosDaMovimentacao(int idmovimentacao)
        {
            var produtos = db.produtosMovimentacao.Where(p => p.idMovimentacaoEstoque == idmovimentacao).ToList();
            foreach (var produto in produtos)
            {
                produto.Produto = db.Produto.Where(p => p.id == produto.idProduto).First();
            }
            return Json(produtos, JsonRequestBehavior.AllowGet);
        }
        private void removeProdutosMovimentacao(int idmovimentacao)
        {
            var produtosMovimentacao = db.produtosMovimentacao.Where(p => p.idMovimentacaoEstoque == idmovimentacao).ToList();

            foreach (var item in produtosMovimentacao)
            {
                db.produtosMovimentacao.Remove(item);

                if (db.SaveChanges() != 0)
                {
                    var produto = db.Produto.Find(item.idProduto);
                    produto.quantidadeEstoque -= item.quantidade;

                    db.Entry(produto).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
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
    public int idMovimetacao { get; set; }
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
public struct rowsTablemov
{
    public int id { get; set; }
    public string razaoSocial { get; set; }
    public string datamovimentacao { get; set; }
    public string tipoMovimentacao { get; set; }
    public string nrDocumento { get; set; }
    public decimal? totalMovimentacao { get; set; }
    public decimal? valorFrete { get; set; }
}
