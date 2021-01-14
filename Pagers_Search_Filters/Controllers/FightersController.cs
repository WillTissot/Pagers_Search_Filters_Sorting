using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Pagers_Search_Filters.DAL;
using Pagers_Search_Filters.Models;
using PagedList;
using PagedList.Mvc;

namespace Pagers_Search_Filters.Controllers
{
    public class FightersController : Controller
    {
        private FightersContext db = new FightersContext();

        // GET: Fighters
        public ActionResult Index(string sortOrder, string searchstring, string abilityFilter, string nameFilter, int? i)
        {
            //Paging
            ViewBag.CurrentSearchString = searchstring;
            ViewBag.CurrentAbilityFilter = abilityFilter;
            ViewBag.CurrentNameFilter = nameFilter;
 


            //viewbags for sorting
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name" : "";
            ViewBag.AbilitySortParm = String.IsNullOrEmpty(sortOrder) ? "Ability" : "";
            var fighters = from s in db.Fighters select s;

            //Searchbar
            if (!String.IsNullOrEmpty(searchstring))
            {
                fighters = fighters.Where(f => f.Name.ToUpper().Contains(searchstring.ToUpper()) || f.Ability.ToUpper().Contains(searchstring.ToUpper()));
            }

            //Sorting
            switch (sortOrder)
            {
                case "Name":
                    fighters = fighters.OrderBy(s => s.Name);
                    break;
                case "Ability":
                    fighters = fighters.OrderBy(s => s.Ability);
                    break;
               
            }

            ViewBag.AbilityFilter = db.Fighters.Select(a => a.Ability).Distinct();
            ViewBag.NameFilter = db.Fighters.Select(n => n.Name).Distinct();

            if (!String.IsNullOrEmpty(abilityFilter) && String.IsNullOrEmpty(nameFilter))
            {
                fighters = fighters.Where(f => f.Ability == abilityFilter).OrderBy(f => f.Ability);
            }
            else if (!String.IsNullOrEmpty(nameFilter) && String.IsNullOrEmpty(abilityFilter))
            {
                fighters = fighters.Where(f => f.Name == nameFilter).OrderBy(f => f.Name);
            }
            else if (!String.IsNullOrEmpty(nameFilter) && !String.IsNullOrEmpty(abilityFilter))
            {
                fighters = fighters.Where(f => f.Name == nameFilter).Where(f => f.Ability == abilityFilter).OrderBy(f => f.Name);
            }



            return View(fighters.ToList().ToPagedList(i ?? 1, 5));
        }

        // GET: Fighters/Details/5
        public ActionResult Details(int? id, string sortOrder, string searchstring, string abilityFilter, string nameFilter, int? i)
        {
            ViewBag.CurrentSearchString = searchstring;
            ViewBag.CurrentAbilityFilter = abilityFilter;
            ViewBag.CurrentNameFilter = nameFilter;




            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fighter fighter = db.Fighters.Find(id);
            if (fighter == null)
            {
                return HttpNotFound();
            }
            return View(fighter);
        }

        // GET: Fighters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fighters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Ability")] Fighter fighter)
        {
            if (ModelState.IsValid)
            {
                db.Fighters.Add(fighter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fighter);
        }

        // GET: Fighters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fighter fighter = db.Fighters.Find(id);
            if (fighter == null)
            {
                return HttpNotFound();
            }
            return View(fighter);
        }

        // POST: Fighters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Ability")] Fighter fighter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fighter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fighter);
        }

        // GET: Fighters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fighter fighter = db.Fighters.Find(id);
            if (fighter == null)
            {
                return HttpNotFound();
            }
            return View(fighter);
        }

        // POST: Fighters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fighter fighter = db.Fighters.Find(id);
            db.Fighters.Remove(fighter);
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
