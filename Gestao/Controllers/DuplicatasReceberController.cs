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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDuplicataReceber,numeroDuplicata,idCliente,dataHemissao,dataVencimento,dataPagamento,valorDuplicata,valorDesconto,valorPago,valorDevedor,valorMulta,valorJurosPorDia,observacao,statusDuplicata,idPedido")] DuplicatasReceber duplicatasReceber)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDuplicataReceber,numeroDuplicata,idCliente,dataHemissao,dataVencimento,dataPagamento,valorDuplicata,valorDesconto,valorPago,valorDevedor,valorMulta,valorJurosPorDia,observacao,statusDuplicata,idPedido")] DuplicatasReceber duplicatasReceber)
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
