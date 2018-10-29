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
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idMovimentacao,datamovimentacao,tipoMovimentacao,nrDocumento,totalMovimentacao,valorFrete,valorIpi,valorIcms,cfop,idFornecedor")] MovimentaEstoque movimentaEstoque)
        {
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
