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
    public class DuplicatasReceberController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DuplicatasReceber
        public ActionResult Index()
        {
            var duplicatasReceber = db.duplicatasReceber.Include(d => d.Cliente).Include(d => d.Pedido);
            return View(duplicatasReceber.ToList());
        }

        // GET: DuplicatasReceber/Details/5
        public ActionResult Details(int? id)
        {
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
            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome");
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao");
            return View();
        }

        // POST: DuplicatasReceber/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DuplicatasReceber duplicatasReceber)
        {
            if (ModelState.IsValid)
            {
                db.duplicatasReceber.Add(duplicatasReceber);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome", duplicatasReceber.idCliente);
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao", duplicatasReceber.idPedido);
            return View(duplicatasReceber);
        }

        // GET: DuplicatasReceber/Edit/5
        public ActionResult Edit(int? id)
        {
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
            return View(duplicatasReceber);
        }

        // POST: DuplicatasReceber/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DuplicatasReceber duplicatasReceber)
        {
            if (ModelState.IsValid)
            {
                db.Entry(duplicatasReceber).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Cliente, "Id", "Nome", duplicatasReceber.idCliente);
            ViewBag.idPedido = new SelectList(db.Pedido, "id", "observacao", duplicatasReceber.idPedido);
            return View(duplicatasReceber);
        }

        // GET: DuplicatasReceber/Delete/5
        public ActionResult Delete(int? id)
        {
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
                    Cliente = i.Cliente.Nome,
                    dataHemissao = i.dataHemissao.ToString(),
                    dataVencimento = i.dataVencimento.ToString(),
                    numeroDuplicata = i.numeroDuplicata,
                    observacao = i.observacao,
                    statusDuplicata = i.statusDuplicata,
                    valorDevedor = i.valorDevedor,
                    valorDuplicata = i.valorDuplicata
                });
            }

            return listaMovimentacoes;
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
    public string Cliente { get; set; }
    public string numeroDuplicata { get; set; }
    public string dataHemissao { get; set; }
    public string dataVencimento { get; set; }
    public decimal? valorDuplicata { get; set; }
    public decimal? valorDevedor { get; set; }
    public string observacao { get; set; }
    public string statusDuplicata { get; set; }
}
