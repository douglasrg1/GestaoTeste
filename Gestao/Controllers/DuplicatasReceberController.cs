﻿using System;
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
    public class DuplicatasReceberController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DuplicatasReceber
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            var duplicatasReceber = db.duplicatasReceber.Include(d => d.Cliente).Include(d => d.Pedido);
            
            return View(duplicatasReceber.ToList());
        }

        // GET: DuplicatasReceber/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasReceber duplicatasReceber = db.duplicatasReceber.Find(id);
            if (duplicatasReceber == null)
            {
                return HttpNotFound();
            }
            return View(duplicatasReceber);
        }

        // GET: DuplicatasReceber/Create
        public ActionResult Create()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome");
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao");
            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View();
        }

        // POST: DuplicatasReceber/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DuplicatasReceber duplicatasReceber)
        {
            duplicatasReceber.Cliente = db.Cliente.First(c => c.Id == duplicatasReceber.idCliente);
            duplicatasReceber.idPedido = duplicatasReceber.idPedido == 0 ? null : duplicatasReceber.idPedido;

            try
            {
                db.duplicatasReceber.Add(duplicatasReceber);
                if (db.SaveChanges() != 0)
                    TempData["msgsucesso"] = "Registro salvo com sucesso";

                if(duplicatasReceber.valorPago != 0)
                {
                    adicionarValorCaixa(duplicatasReceber.valorPago, duplicatasReceber.numeroDuplicata, duplicatasReceber.idPedido, duplicatasReceber.idDuplicataReceber);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msgerro"] = "Erro ao tentar salvar registro.";
            }

            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome", duplicatasReceber.idCliente);
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao", duplicatasReceber.idPedido);
            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View(duplicatasReceber);
        }

        // GET: DuplicatasReceber/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasReceber duplicatasReceber = db.duplicatasReceber.Find(id);
            if (duplicatasReceber == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome", duplicatasReceber.idCliente);
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao", duplicatasReceber.idPedido);

            if (duplicatasReceber.dataPagamento != null)
                ViewBag.datpag = Convert.ToDateTime(duplicatasReceber.dataPagamento).ToString("yyyy/MM/dd");

            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View(duplicatasReceber);
        }

        // POST: DuplicatasReceber/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DuplicatasReceber duplicatasReceber, int id)
        {
            var dup = db.duplicatasReceber.AsNoTracking().Where(d=>d.idDuplicataReceber == id).FirstOrDefault();
            duplicatasReceber.Cliente = db.Cliente.First(c => c.Id == duplicatasReceber.idCliente);
            duplicatasReceber.idPedido = duplicatasReceber.idPedido == 0 ? null : duplicatasReceber.idPedido;
            duplicatasReceber.idDuplicataReceber = id;

            try
            {
                db.Entry(duplicatasReceber).State = EntityState.Modified;
                if (db.SaveChanges() != 0)
                    TempData["msgsucesso"] = "Registro editado com sucesso";

                if (duplicatasReceber.valorPago > dup.valorPago)
                {
                    adicionarValorCaixa((duplicatasReceber.valorPago - dup.valorPago), duplicatasReceber.numeroDuplicata, duplicatasReceber.idPedido, duplicatasReceber.idDuplicataReceber);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msgerro"] = "Erro ao tentar editar registro.";
            }

            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome", duplicatasReceber.idCliente);
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao", duplicatasReceber.idPedido);
            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View(duplicatasReceber);
        }

        // GET: DuplicatasReceber/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasReceber duplicatasReceber = db.duplicatasReceber.Find(id);
            if (duplicatasReceber == null)
            {
                return HttpNotFound();
            }
            return View(duplicatasReceber);
        }

        // POST: DuplicatasReceber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DuplicatasReceber duplicatasReceber = db.duplicatasReceber.Find(id);
            db.duplicatasReceber.Remove(duplicatasReceber);
            db.SaveChanges();
            TempData["msgsucesso"] = "Registro removido com sucesso";
            return RedirectToAction("Index");
        }
        public JsonResult listarDuplicatas(int current, int rowCount, string searchPhrase)
        {

            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var duplicatas = db.duplicatasReceber.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                duplicatas = duplicatas.Where("Cliente.Nome.Contains(@0) OR numeroDuplicata.Contains(@0)", searchPhrase).ToList();
            }

            var duplicataspaginadas = duplicatas.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            var duplicatasr = retornaRowsTable(duplicataspaginadas);

            return Json(new { current = current, rowCount = rowCount, rows = duplicatasr, total = duplicatas.Count }, JsonRequestBehavior.AllowGet);
        }

        private List<rowsTableduplicatasreceber> retornaRowsTable(IEnumerable<DuplicatasReceber> duplicataspaginadas)
        {
            List<rowsTableduplicatasreceber> listaMovimentacoes = new List<rowsTableduplicatasreceber>();
            foreach (var i in duplicataspaginadas)
            {
                listaMovimentacoes.Add(new rowsTableduplicatasreceber
                {
                    id = i.idDuplicataReceber,
                    Cliente = i.Cliente.Nome,
                    dataHemissao = i.dataHemissao.ToString("dd/MM/yyyy"),
                    dataVencimento = i.dataVencimento.ToString("dd/MM/yyyy"),
                    numeroDuplicata = i.numeroDuplicata,
                    observacao = i.observacao,
                    statusDuplicata = i.statusDuplicata,
                    valorDevedor = i.valorDevedor.ToString().Replace(".",","),
                    valorDuplicata = i.valorDuplicata.ToString().Replace(".", ",")
                });
            }

            return listaMovimentacoes;
        }
        private IList<SelectListItem> selectListStatusDuplicata()
        {
            IList<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "Pago",
                    Value = "Pago",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Não Pago",
                    Value = "Nao Pago",
                    Selected = false
                },
                new SelectListItem
                {
                    Text = "Parcialmente Pago",
                    Value = "Parcialmente Pago",
                    Selected = false
                }
            };

            return list;
        }
        public decimal calcularJuros(int idDuplicata)
        {
            decimal valorMulta = 0;
            var duplicata = db.duplicatasReceber.First(d => d.idDuplicataReceber == idDuplicata);

            if(DateTime.Now > duplicata.dataVencimento && duplicata.statusDuplicata != "Pago")
            {
                var diasDeAtraso = (DateTime.Now.Date - duplicata.dataVencimento.Date).TotalDays;
                valorMulta = duplicata.valorMulta + (Convert.ToDecimal(diasDeAtraso) * duplicata.valorJurosPorDia);
            }

            return valorMulta;
        }
        private void adicionarValorCaixa(decimal valor, string numeroDuplicata, int? idPedido,int idDuplicata)
        {
            Caixa caixa = new Caixa()
            {
                DataMovimentacao = DateTime.Now,
                descricao = "Pagamento relacionado a duplicata: " + numeroDuplicata,
                PedidoId = idPedido,
                TipoMovimentacao = "Entrada",
                ValorMovimentacao = valor,
                DuplicataId = idDuplicata
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

public struct rowsTableduplicatasreceber
{
    public int id { get; set; }
    public string Cliente { get; set; }
    public string numeroDuplicata { get; set; }
    public string dataHemissao { get; set; }
    public string dataVencimento { get; set; }
    public string valorDuplicata { get; set; }
    public string valorDevedor { get; set; }
    public string observacao { get; set; }
    public string statusDuplicata { get; set; }
}
