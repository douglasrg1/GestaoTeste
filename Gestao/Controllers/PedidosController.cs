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
    public class PedidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pedidos
        public ActionResult Index()
        {
            return View(db.Pedido.ToList());
        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();
            return View();
        }

        // POST: Pedidos/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dadosRecebidos dadosRecebidos)
        {
            Pedido pedido = new Pedido();
            //pedido.cliente = db.Cliente.Where(c => c.Id == dadospedido.idCliente).First();
            //pedido.numeroPedido = dadospedido.numeroPedido;
            //pedido.observacao = dadospedido.obs;
            //pedido.ProdutoPedido =dadospedido.produtoPedidos;
            //pedido.totalPago = dadospedido.totalPago;
            //pedido.totalPedido = dadospedido.toalPedido;
            //pedido.valorComissao = dadospedido.valorComissao;
            //pedido.valorDevedor = dadospedido.valorDevedor;

            if (ModelState.IsValid)
            {
                pedido.dataPedido = DateTime.Now;
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();

            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,dataPedido,valorComissao,observacao,totalPedido,totalPago,valorDevedor")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IList<SelectListItem> listItemclientes()
        {
            var itens = db.Cliente.ToList();

            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.Nome,   // Valor que será o Texto do Dropdown
                    Value = item.Id.ToString(),  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            return listItens;
        }
        public IList<SelectListItem> listProdutos()
        {
            var itens = db.Produto.ToList();

            IList<SelectListItem> listItens = new List<SelectListItem>();

            foreach (var item in itens)
            {
                listItens.Add(new SelectListItem
                {
                    Text = item.nome,   // Valor que será o Texto do Dropdown
                    Value = item.id.ToString(),  // Valor que será o Value do Dropdown
                    Selected = false            // Indica se o item será selecionado por padrão no Dropdown
                });

            }

            return listItens;
        }
        public decimal retornarValorProduto(int id)
        {
            var valorProduto = db.Produto.Where(p => p.id == id).First().valorVenda;
            return valorProduto;
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
public struct DadosPedido
{
    public int idCliente { get; set; }
    public decimal valorComissao { get; set; }
    public string obs { get; set; }
    public decimal toalPedido { get; set; }
    public decimal totalPago { get; set; }
    public decimal valorDevedor { get; set; }
    public int numeroPedido { get; set; }
    public List<ProdutoPedido> produtoPedidos { get; set; }


}
public struct dadosPedido
{
    public int id { get; set; }
    public string observacao { get; set; }
    public decimal totalPago { get; set; }
    public decimal totalPedido { get; set; }
    public decimal valorComissao { get; set; }
    public decimal valorDevedor { get; set; }

}
public struct produtosPedido
{
    public int idproduto { get; set; }
    public string obs { get; set; }
    public decimal porcDesconto { get; set; }
    public decimal quantidade { get; set; }
    public decimal valorDesconto { get; set; }
    public decimal valorTotal { get; set; }
    public decimal valorUnitario { get; set; }

}
public struct dadosRecebidos
{
    dadosPedido dadosPedido;
    List<produtosPedido> produtosPedidos;
}
