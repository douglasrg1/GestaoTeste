using Gestao.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gestao.Controllers
{
    public class CaixaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Caixa
        public ActionResult Index()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            return View(db.Caixa.ToList());
        }

        // GET: Caixa/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var caixa = db.Caixa.Find(id);
            if (caixa == null)
            {
                return HttpNotFound();
            }
            return View(caixa);
        }

        // GET: Caixa/Create
        public ActionResult Create()
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            IList<SelectListItem> tipomov = new List<SelectListItem>()
            {
                new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
            };

            ViewBag.tipomov = tipomov;
            ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(),"id","nome");
            

            return View();
        }

        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Caixa caixa)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    caixa.DataMovimentacao = DateTime.Now;
                    db.Caixa.Add(caixa);
                    if (db.SaveChanges() != 0)
                        TempData["mensagem"] = "Registro adicionado com sucesso";
                    else
                        TempData["mensagem"] = "Falha ao adicionar registro";

                    return RedirectToAction("Index");

                }

                IList<SelectListItem> tipomov = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                    new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
                };

                ViewBag.tipomov = tipomov;
                ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(), "id", "nome");

                return View(caixa);
            }
            catch
            {
                IList<SelectListItem> tipomov = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                    new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
                };

                ViewBag.tipomov = tipomov;
                ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(), "id", "nome");

                return View(caixa);
            }
        }

        // GET: Caixa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Caixa caixa = db.Caixa.Find(id);

            if (caixa == null)
            {
                return HttpNotFound();
            }

            IList<SelectListItem> tipomov = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                    new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
                };

            ViewBag.tipomov = tipomov;
            ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(), "id", "nome");

            return View(caixa);
        }

        // POST: Caixa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Caixa caixa)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(caixa).State = EntityState.Modified;
                    if (db.SaveChanges() != 0)
                        TempData["mensagem"] = "Registro editado com Sucesso";
                    else
                        TempData["mensagem"] = "Erro ao editar Registro";

                    return RedirectToAction("Index");
                }

                IList<SelectListItem> tipomov = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                    new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
                };

                ViewBag.tipomov = tipomov;
                ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(), "id", "nome");
                return View(caixa);
            }
            catch(Exception ex)
            {
                IList<SelectListItem> tipomov = new List<SelectListItem>()
                {
                    new SelectListItem{Text = "Entrada",Value="Entrada",Selected = false},
                    new SelectListItem{Text="Saída",Value = "Saida",Selected = false}
                };

                ViewBag.tipomov = tipomov;
                ViewBag.FuncionarioId = new SelectList(db.Funcionario.ToList(), "id", "nome");
                return View(caixa);
            }
        }

        // GET: Caixa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UsuarioLogado"] == null)
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Caixa caixa = db.Caixa.Find(id);
            if (caixa == null)
            {
                return HttpNotFound();
            }
            return View(caixa);
        }

        // POST: Caixa/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                if (Session["UsuarioLogado"] == null)
                    return RedirectToAction("Index", "Login");

                Caixa caixa = db.Caixa.Find(id);
                db.Caixa.Remove(caixa);
                if (db.SaveChanges() != 0)
                    TempData["mensagem"] = "Registro removido com Sucesso";
                else
                    TempData["mensagem"] = "Erro ao remover registro";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult listarRegistros(int current, int rowCount, string searchPhrase)
        {
            string chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            string campoOrdenacao = chave.Replace("sort[", "").Replace("]", "").Trim();
            string tipoOrdenacao = Request[chave];
            var registros = db.Caixa.ToList();
            var ordenacao = string.Format("{0} {1}", campoOrdenacao, tipoOrdenacao);

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                registros = registros.Where("TipoMovimentacao.Contains(@0) OR descricao.Contains(@0)", searchPhrase).ToList();
            }

            var registrosPaginados = registros.OrderBy(ordenacao).Skip((current - 1) * rowCount).Take(rowCount);
            var registrosR = retornaRowsTable(registrosPaginados);

            return Json(new { current = current, rowCount = rowCount, rows = registrosR, total = registros.Count }, JsonRequestBehavior.AllowGet);
        }
        private List<rowsTableCaixa> retornaRowsTable(IEnumerable<Caixa> caixaPaginadas)
        {
            List<rowsTableCaixa> listaMovimentacoes = new List<rowsTableCaixa>();
            foreach (var i in caixaPaginadas)
            {
                listaMovimentacoes.Add(new rowsTableCaixa
                {
                    CaixaId = i.CaixaId,
                    DataMovimentacao = i.DataMovimentacao.ToString("dd/MM/yyyy"),
                    descricao = i.descricao,
                    TipoMovimentacao = i.TipoMovimentacao,
                    ValorMovimentacao = i.ValorMovimentacao.ToString()
                });
            }

            return listaMovimentacoes;
        }
    }
}

public struct rowsTableCaixa
{
    public int CaixaId { get; set; }
    public string TipoMovimentacao { get; set; }
    public string DataMovimentacao { get; set; }
    public string ValorMovimentacao { get; set; }
    public string descricao { get; set; }
}
