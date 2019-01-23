using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Gestao.Models;
using Newtonsoft.Json;

namespace Gestao.Controllers
{
    public class PedidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pedidos
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var pedido = db.Pedido.ToList();
            return View("Index",pedido);
        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

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
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();
            var countPedido = db.Pedido.Count();
            if (countPedido == 0)
                ViewBag.numeropedido = 00001;
            else
            {
                int numeropedido = db.Pedido.OrderByDescending(p => p.id).FirstOrDefault().numeroPedido + 1;
                ViewBag.numeropedido = numeropedido;
            }
                
            
            return View();
        }

        // POST: Pedidos/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pedido pedido)
        {
            var dados = dadosRecebidos();

            pedido.Cliente = db.Cliente.Where(c => c.Id == dados.dadosPedido.idCliente).First();
            pedido.numeroPedido = dados.dadosPedido.numeroPedido;
            pedido.observacao = dados.dadosPedido.observacao;
            pedido.totalPago = dados.dadosPedido.totalPago;
            pedido.totalPedido = dados.dadosPedido.totalPedido;
            pedido.valorComissao = dados.dadosPedido.valorComissao;
            pedido.valorDevedor = dados.dadosPedido.valorDevedor;
            pedido.dataPedido = DateTime.Now;

            db.Pedido.Add(pedido);

            if (db.SaveChanges() != 0 )
            {
                 var idmov = adicionarMovimentacaoEstoque(pedido);

                ProdutoPedido produtoPedido = new ProdutoPedido();
                foreach(var item in dados.produtosPedidos)
                {
                    produtoPedido.observacao = item.obs;
                    produtoPedido.Pedido = pedido;
                    produtoPedido.porcDesconto = item.porcDesconto;
                    produtoPedido.Produto = db.Produto.Find(item.idproduto);
                    produtoPedido.quantidade = item.quantidade;
                    produtoPedido.valorDesconto = item.valorDesconto;
                    produtoPedido.valorTotal = item.valorTotal;
                    produtoPedido.valorUnitario = item.valorUnitario;
                    produtoPedido.Produto_id = item.idproduto;

                    db.ProdutoPedido.Add(produtoPedido);
                    if(db.SaveChanges() != 0)
                    {
                        adicionaProdutoMovimentacao(produtoPedido, idmov);

                        var produto = db.Produto.Find(item.idproduto);
                        produto.dataUltimaSaida = DateTime.Now;
                        produto.quantidadeEstoque -= item.quantidade;
                        db.Entry(produto).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                TempData["msg"] = "Pedido Registrado com Sucesso";
                
            }

            if (dados.duplicatas.numeroDuplicatas != 0)
            {
                gerarDuplicatasPedido(dados, pedido.id);
            }
            if(pedido.totalPago != 0)
            {
                adicionarValorCaixa(pedido.totalPago,pedido.numeroPedido,pedido.id);
            }
            

            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();

            return View(pedido);
        }

        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }

            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();
            var produtosPedido = db.ProdutoPedido.Where(p => p.Pedido.id == pedido.id).ToList();
            foreach(var item in produtosPedido)
                item.Pedido = null;

            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Pedido pedido)
        {
            var dados = dadosRecebidos();
            var ped = db.Pedido.AsNoTracking().Where(p => p.id == dados.dadosPedido.idPedido).FirstOrDefault();
            pedido.id = dados.dadosPedido.idPedido;
            pedido.idCliente = dados.dadosPedido.idCliente;
            pedido.Cliente = db.Cliente.Find(dados.dadosPedido.idCliente);
            pedido.numeroPedido = dados.dadosPedido.numeroPedido;
            pedido.observacao = dados.dadosPedido.observacao;
            pedido.totalPago = dados.dadosPedido.totalPago;
            pedido.totalPedido = dados.dadosPedido.totalPedido;
            pedido.valorComissao = dados.dadosPedido.valorComissao;
            pedido.valorDevedor = dados.dadosPedido.valorDevedor;
            pedido.dataPedido = DateTime.Now;

            var idmov = db.movimentaEstoque.Where(m => m.nrDocumento == "PD-" + pedido.numeroPedido).FirstOrDefault().idMovimentacao;

            db.Entry(pedido).State = EntityState.Modified;

            if (db.SaveChanges() != 0)
            {
                editaMovimentacao(pedido);
                removeProdutosPedido(pedido.id);
                ProdutoPedido produtoPedido = new ProdutoPedido();

                if(dados.produtosPedidos != null)
                {
                    foreach (var item in dados.produtosPedidos)
                    {
                        produtoPedido.observacao = item.obs;
                        produtoPedido.Pedido = pedido;
                        produtoPedido.porcDesconto = item.porcDesconto;
                        produtoPedido.Produto = db.Produto.Find(item.idproduto);
                        produtoPedido.quantidade = item.quantidade;
                        produtoPedido.valorDesconto = item.valorDesconto;
                        produtoPedido.valorTotal = item.valorTotal;
                        produtoPedido.valorUnitario = item.valorUnitario;
                        produtoPedido.Produto_id = item.idproduto;

                        db.ProdutoPedido.Add(produtoPedido);
                        if (db.SaveChanges() != 0)
                        {
                            adicionaProdutoMovimentacao(produtoPedido, idmov);

                            var produto = db.Produto.Find(item.idproduto);
                            produto.dataUltimaSaida = DateTime.Now;
                            produto.quantidadeEstoque -= item.quantidade;
                            db.Entry(produto).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

                if(pedido.totalPago != ped.totalPago)
                {
                    var registros = db.Caixa.Where(c => c.PedidoId == pedido.id && c.DuplicataId == null).ToList();

                    if(registros.Count > 0)
                        db.Caixa.RemoveRange(registros);

                    adicionarValorCaixa(pedido.totalPago, pedido.numeroPedido, pedido.id);
                }

                TempData["msg"] = "Pedido Editado com Sucesso";
                
            }

            ViewBag.produtos = listProdutos();
            ViewBag.clientes = listItemclientes();

            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

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

            if (db.SaveChanges() != 0)
                removeMovimentacao(pedido);
           
            TempData["msg"] = "Pedido Removido com Sucesso";
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

        
        public ActionResult retornaProdutosDoPedido(int idreceita)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var produtos = db.ProdutoPedido.Where(p => p.Pedido.id == idreceita).ToList();
            foreach(var produto in produtos)
            {
                produto.Produto = db.Produto.Where(p => p.id == produto.Produto_id).First();
            }
            return Json(produtos,JsonRequestBehavior.AllowGet);
        }

        public dadosRecebidos dadosRecebidos()
        {
            dadosRecebidos dadosRecebidos = new dadosRecebidos();
            dadosPedido dadosPedido = new dadosPedido();
            produtosPedido produtosPedido = new produtosPedido();
            duplicatas duplicatas = new duplicatas();


            //dados duplicatas
            
            if (Request["dadosRecebidos[dadosDuplicata][dataVencimento]"] != null)
            {
                duplicatas.dataVencimento = Convert.ToDateTime(Request["dadosRecebidos[dadosDuplicata][dataVencimento]"]);
                duplicatas.intervaloEntreDuplicatas = Convert.ToInt32(Request["dadosRecebidos[dadosDuplicata][diasIntervalo]"]);
                duplicatas.numeroDuplicatas = Convert.ToInt32(Request["dadosRecebidos[dadosDuplicata][numeroDupl]"]);
                dadosRecebidos.duplicatas = duplicatas;
            }
            

            //dados pedido
            dadosPedido.idPedido = Convert.ToInt32(Request["dadosRecebidos[dadosPedido][idPedido]"]);
            dadosPedido.numeroPedido = Convert.ToInt32(Request["dadosRecebidos[dadosPedido][numeroPedido]"]);
            dadosPedido.idCliente = Convert.ToInt32(Request["dadosRecebidos[dadosPedido][idCliente]"]);
            dadosPedido.valorComissao = Convert.ToDecimal(Request["dadosRecebidos[dadosPedido][valorComissao]"]);
            dadosPedido.observacao = Request["dadosRecebidos[dadosPedido][observacao]"];
            dadosPedido.totalPedido = Convert.ToDecimal(Request["dadosRecebidos[dadosPedido][totalPedido]"]);
            dadosPedido.totalPago = Convert.ToDecimal(Request["dadosRecebidos[dadosPedido][totalPago]"]);
            dadosPedido.valorDevedor = Convert.ToDecimal(Request["dadosRecebidos[dadosPedido][valorDevedor]"]);
            dadosRecebidos.dadosPedido = dadosPedido;
            //dados produtos
            var produtosdoPedido = this.Request.Form.AllKeys.Where(k => k.Contains("dadosRecebidos[produtosPedido]")).ToList();
            var quantKeys = (produtosdoPedido.Count - 1)/6;

            if (quantKeys == 0)
                return dadosRecebidos;

            dadosRecebidos.produtosPedidos = new List<produtosPedido>();

            for(var i = 0; i < quantKeys; i++)
            {
                produtosPedido.idproduto = Convert.ToInt32(Request["dadosRecebidos[produtosPedido][" + i + "][idproduto]"]);
                produtosPedido.valorUnitario = Convert.ToDecimal(Request["dadosRecebidos[produtosPedido][" + i + "][valorUnitario]"]);
                produtosPedido.quantidade = Convert.ToDecimal(Request["dadosRecebidos[produtosPedido][" + i + "][quantidade]"]);
                produtosPedido.porcDesconto = Convert.ToDecimal(Request["dadosRecebidos[produtosPedido][" + i + "][porcDesconto]"]);
                produtosPedido.valorDesconto = Convert.ToDecimal(Request["dadosRecebidos[produtosPedido][" + i + "][valorDesconto]"]);
                produtosPedido.valorTotal = Convert.ToDecimal(Request["dadosRecebidos[produtosPedido][" + i + "][valorTotal]"]);
                produtosPedido.obs = Request["dadosRecebidos[produtosPedido][" + i + "][obs]"];

                dadosRecebidos.produtosPedidos.Add(produtosPedido);

            }

            


            return dadosRecebidos;
        }

        private void removeProdutosPedido(int idpedido)
        {
            var produtosPedido = db.ProdutoPedido.Where(p => p.Pedido.id == idpedido).ToList();

            foreach(var item in produtosPedido)
            {
                db.ProdutoPedido.Remove(item);
                
                if(db.SaveChanges() != 0)
                {
                    var produto = db.Produto.Find(item.Produto_id);
                    produto.quantidadeEstoque += item.quantidade;

                    db.Entry(produto).State = EntityState.Modified;
                    db.SaveChanges();

                    removeProdutoMovimentacao(db.Pedido.Where(p => p.id == idpedido).First(), item.Produto_id);
                }
            }
        }
        public JsonResult listarPedidos(int current, int rowCount, string searchPhrase)
        {
            

            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var pedidos = db.Pedido.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                pedidos = pedidos.Where("Cliente.Nome.Contains(@0)", searchPhrase).ToList();
            }

            var pedidosPaginados = pedidos.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            var pedidosp = retornaRowsTable(pedidosPaginados);

            return Json(new {current = current, rowCount = rowCount, rows = pedidosp, total = pedidos.Count }, JsonRequestBehavior.AllowGet);
        }
        private List<rowsTable> retornaRowsTable(IEnumerable<Pedido> pedidos)
        {
            List<rowsTable> listapedidos = new List<rowsTable>();
            foreach(var i in pedidos)
            {
                listapedidos.Add(new rowsTable
                {
                    id = i.id,
                    dataPedido = i.dataPedido.ToString(),
                    nomeCliente = i.Cliente.Nome,
                    numeroPedido = i.numeroPedido,
                    observacao = i.observacao,
                    totalPago = i.totalPago,
                    totalPedido = i.totalPedido,
                    valorDevedor = i.valorDevedor
                });
            }

            return listapedidos;
        }
        private int adicionarMovimentacaoEstoque(Pedido pedido)
        {
           
            var resposta = 0;

            MovimentaEstoque movimentacao = new MovimentaEstoque();
            movimentacao.datamovimentacao = DateTime.Now;
            movimentacao.idFornecedor = null;
            movimentacao.nrDocumento = "PD-" + pedido.numeroPedido;
            movimentacao.tipoMovimentacao = "Saída";
            movimentacao.totalMovimentacao = pedido.totalPedido;
            db.movimentaEstoque.Add(movimentacao);

            if(db.SaveChanges()!= 0)
            {
                resposta = movimentacao.idMovimentacao;
            }

            return resposta;
        }
        private void adicionaProdutoMovimentacao(ProdutoPedido produto,int idMovimentacao)
        {
            
            ProdutosMovimentacao produtomov = new ProdutosMovimentacao();
            produtomov.idMovimentacaoEstoque = idMovimentacao;
            produtomov.MovimentaEstoque = db.movimentaEstoque.Find(idMovimentacao);
            produtomov.idProduto = produto.Produto_id;
            produtomov.observacao = produto.observacao;
            produtomov.porcdesconto = produto.porcDesconto;
            produtomov.Produto = db.Produto.Find(produto.Produto_id);
            produtomov.quantidade = produto.quantidade;
            produtomov.valorDesconto = produto.valorDesconto;
            produtomov.valorTotal = produto.valorTotal;
            produtomov.valorUnitario = produto.valorUnitario;

            db.produtosMovimentacao.Add(produtomov);
            var resp = db.SaveChanges();
        }
        private void removeProdutoMovimentacao(Pedido pedido, int idProduto)
        {
           

            var movimentacao = db.movimentaEstoque.Where(p => p.nrDocumento == "PD-" + pedido.numeroPedido).FirstOrDefault();

            var produtosMov = db.produtosMovimentacao.Where(p => p.idMovimentacaoEstoque == movimentacao.idMovimentacao).ToList();

            var produto = produtosMov.Where(p=>p.idProduto == idProduto).FirstOrDefault();

            if(produto != null)
            {
                db.produtosMovimentacao.Remove(produto);
                var resp = db.SaveChanges();
            }
                
        }
        private void removeMovimentacao(Pedido pedido)
        {
            var movimentacao = db.movimentaEstoque.Where(p => p.nrDocumento == "PD-" + pedido.numeroPedido).FirstOrDefault();

            if(movimentacao != null)
            {
                db.movimentaEstoque.Remove(movimentacao);
                var resp = db.SaveChanges();
            }
            
        }
        private void editaMovimentacao(Pedido pedido)
        {
            var movimentacao = db.movimentaEstoque.Where(p => p.nrDocumento == "PD-" + pedido.numeroPedido).FirstOrDefault();

            movimentacao.totalMovimentacao = pedido.totalPedido;

            db.Entry(movimentacao).State = EntityState.Modified;
            var resp = db.SaveChanges();
        }
        private void gerarDuplicatasPedido(dadosRecebidos dadosDuplicata, int idpedido)
        {
            var datavencimento = dadosDuplicata.duplicatas.dataVencimento;
            var valoresDuplicatas = calcularValorDuplicatas(dadosDuplicata.dadosPedido.valorDevedor, dadosDuplicata.duplicatas.numeroDuplicatas);

            for(int i = 0; i < dadosDuplicata.duplicatas.numeroDuplicatas; i++)
            {
                DuplicatasReceber duplicata = new DuplicatasReceber()
                {
                    Cliente = db.Cliente.Find(dadosDuplicata.dadosPedido.idCliente),
                    dataHemissao = DateTime.Now,
                    dataPagamento = null,
                    dataVencimento = datavencimento,
                    idCliente = dadosDuplicata.dadosPedido.idCliente,
                    idPedido = idpedido,
                    numeroDuplicata = "PD-" + dadosDuplicata.dadosPedido.numeroPedido,
                    Pedido = db.Pedido.Find(idpedido),
                    statusDuplicata = "Nao Pago",
                    valorDesconto = 0,
                    valorDevedor = valoresDuplicatas[i],
                    valorDuplicata = valoresDuplicatas[i],
                    valorJurosPorDia = 0,
                    valorMulta = 0,
                    valorPago = 0
                };

                db.duplicatasReceber.Add(duplicata);
                var resp = db.SaveChanges();
                datavencimento = datavencimento.AddDays(dadosDuplicata.duplicatas.intervaloEntreDuplicatas);
            }
        }
        private decimal[] calcularValorDuplicatas(decimal valorPedido, int numeroDuplicatas)
        {
            decimal[] duplicatas = new decimal[numeroDuplicatas];
            var valor = valorPedido / numeroDuplicatas;
            var valordecimal = valor.ToString().Split(',')[1];
            decimal valorDuplicata = 0;
            decimal valorUltimaDuplicata = 0;
            if (valordecimal.Length > 2)
            {
                valorDuplicata = Convert.ToDecimal(valor.ToString().Split(',')[0] + "," + valordecimal.Substring(0,2));
                valorUltimaDuplicata = valorPedido - (valorDuplicata * (numeroDuplicatas - 1));
            }
            else
            {
                valorDuplicata = valor;
                valorUltimaDuplicata = valor;
            }

            for(int i = 0; i < numeroDuplicatas - 1; i++)
            {
                duplicatas[i] = valorDuplicata;
            }
            duplicatas[numeroDuplicatas - 1] = valorUltimaDuplicata;

            return duplicatas;
        }
        private void adicionarValorCaixa(decimal valor,int numeroPedido,int idPedido)
        {
            Caixa caixa = new Caixa()
            {
                DataMovimentacao = DateTime.Now,
                descricao = "Pagamento relacionado ao pedido " + numeroPedido,
                PedidoId = idPedido,
                TipoMovimentacao = "Entrada",
                ValorMovimentacao = valor
            };

            db.Caixa.Add(caixa);
            db.SaveChanges();
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

public struct dadosPedido
{
    public int idPedido { get; set; }
    public int idCliente { get; set; }
    public decimal valorComissao { get; set; }
    public string observacao { get; set; }
    public decimal totalPedido { get; set; }
    public decimal totalPago { get; set; }
    public decimal valorDevedor { get; set; }
    public int numeroPedido { get; set; }


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
public struct duplicatas
{
    public int numeroDuplicatas { get; set; }
    public DateTime dataVencimento { get; set; }
    public int intervaloEntreDuplicatas { get; set; }
}
public struct dadosRecebidos
{
    public dadosPedido dadosPedido;
    public List<produtosPedido> produtosPedidos;
    public duplicatas duplicatas;
}
public struct rowsTable
{
    public int id { get; set; }
    public string nomeCliente { get; set; }
    public string dataPedido { get; set; }
    public string observacao { get; set; }
    public decimal totalPedido { get; set; }
    public decimal totalPago { get; set; }
    public decimal valorDevedor { get; set; }
    public int numeroPedido { get; set; }
}
