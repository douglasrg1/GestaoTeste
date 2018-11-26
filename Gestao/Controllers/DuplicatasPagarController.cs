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
            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View();
        }

        // POST: DuplicatasPagar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( DuplicatasPagar duplicatasPagar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.duplicatasPagar.Add(duplicatasPagar);
                    if (db.SaveChanges() != 0)
                        TempData["msgsucesso"] = "Registro salvo com sucesso";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["msgerro"] = "Erro ao tentar salvar registro.";
                }
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

            if (duplicatasPagar.dataPagamento != null)
                ViewBag.datpag = Convert.ToDateTime(duplicatasPagar.dataPagamento).ToString("yyyy/MM/dd");

            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", duplicatasPagar.idFornecedor);
            ViewBag.statusDuplicata = selectListStatusDuplicata();
            return View(duplicatasPagar);
        }

        // POST: DuplicatasPagar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DuplicatasPagar duplicatasPagar,int id)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    duplicatasPagar.idDuplicataPagar = id;
                    db.Entry(duplicatasPagar).State = EntityState.Modified;
                    if (db.SaveChanges() != 0)
                        TempData["msgsucesso"] = "Registro editado com sucesso";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["msgerro"] = "Erro ao tentar editar registro.";
                }
            }

            if (duplicatasPagar.dataPagamento != null)
                ViewBag.datpag = Convert.ToDateTime(duplicatasPagar.dataPagamento).ToString("yyyy/MM/dd");

            ViewBag.idFornecedor = new SelectList(db.Fornecedor, "id", "razaoSocial", duplicatasPagar.idFornecedor);
            ViewBag.statusDuplicata = selectListStatusDuplicata();
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
        public JsonResult listarDuplicatas(int current, int rowCount, string searchPhrase)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var duplicatas = db.duplicatasPagar.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                duplicatas = duplicatas.Where("Fornecedor.razaoSocial.Contains(@0) OR numeroDuplicata.Contains(@0)", searchPhrase).ToList();
            }

            var duplicataspaginadas = duplicatas.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            var duplicatasr = retornaRowsTable(duplicataspaginadas);

            return Json(new { current = current, rowCount = rowCount, rows = duplicatasr, total = duplicatas.Count }, JsonRequestBehavior.AllowGet);
        }
        private List<rowsTableduplicatasPagar> retornaRowsTable(IEnumerable<DuplicatasPagar> duplicataspaginadas)
        {
            List<rowsTableduplicatasPagar> listaMovimentacoes = new List<rowsTableduplicatasPagar>();
            foreach (var i in duplicataspaginadas)
            {
                listaMovimentacoes.Add(new rowsTableduplicatasPagar
                {
                    id = i.idDuplicataPagar,
                    Fornecedor = i.Fornecedor.razaoSocial,
                    dataHemissao = i.dataHemissao.ToString("dd/MM/yyyy"),
                    dataVencimento = i.dataVencimento.ToString("dd/MM/yyyy"),
                    numeroDuplicata = i.numeroDuplicata,
                    statusDuplicata = i.statusDuplicata,
                    valorDuplicata = i.valorDuplicata.ToString().Replace(".", ","),
                    nrDocumento = i.nrDocumento
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

public struct rowsTableduplicatasPagar
{
    public int id { get; set; }
    public string Fornecedor { get; set; }
    public string numeroDuplicata { get; set; }
    public string dataHemissao { get; set; }
    public string dataVencimento { get; set; }
    public string valorDuplicata { get; set; }
    public string statusDuplicata { get; set; }
    public string nrDocumento { get; set; }
}
