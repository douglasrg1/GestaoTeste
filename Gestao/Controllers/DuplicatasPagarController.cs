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
    public class DuplicatasPagarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DuplicatasPagar
        public ActionResult Index()
        {
            var duplicatasPagar = db.duplicatasPagar.Include(d => d.Fornecedor);
            return View(duplicatasPagar.ToList());
        }

        // GET: DuplicatasPagar/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasPagar duplicatasPagar = db.duplicatasPagar.Find(id);
            if (duplicatasPagar == null)
            {
                return HttpNotFound();
            }
            return View(duplicatasPagar);
        }

        // GET: DuplicatasPagar/Create
        public ActionResult Create()
        {
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial");
            return View();
        }

        // POST: DuplicatasPagar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDuplicataPagar,numeroDuplicata,idFornecedor,dataHemissao,dataVencimento,dataPagamento,valorDuplicata,valorDesconto,valorPago,valorDevedor,valorMulta,valorJurosPorDia,observacao,statusDuplicata,nrDocumento")] DuplicatasPagar duplicatasPagar)
        {
            if (ModelState.IsValid)
            {
                db.duplicatasPagar.Add(duplicatasPagar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", duplicatasPagar.idFornecedor);
            return View(duplicatasPagar);
        }

        // GET: DuplicatasPagar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasPagar duplicatasPagar = db.duplicatasPagar.Find(id);
            if (duplicatasPagar == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", duplicatasPagar.idFornecedor);
            return View(duplicatasPagar);
        }

        // POST: DuplicatasPagar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDuplicataPagar,numeroDuplicata,idFornecedor,dataHemissao,dataVencimento,dataPagamento,valorDuplicata,valorDesconto,valorPago,valorDevedor,valorMulta,valorJurosPorDia,observacao,statusDuplicata,nrDocumento")] DuplicatasPagar duplicatasPagar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(duplicatasPagar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", duplicatasPagar.idFornecedor);
            return View(duplicatasPagar);
        }

        // GET: DuplicatasPagar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuplicatasPagar duplicatasPagar = db.duplicatasPagar.Find(id);
            if (duplicatasPagar == null)
            {
                return HttpNotFound();
            }
            return View(duplicatasPagar);
        }

        // POST: DuplicatasPagar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DuplicatasPagar duplicatasPagar = db.duplicatasPagar.Find(id);
            db.duplicatasPagar.Remove(duplicatasPagar);
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
