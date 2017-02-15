using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactList.DAL;
using ContactList.Models;

namespace ContactList.Controllers
{
    public class ContactsController : Controller
    {
        private ContactListContext db = new ContactListContext();

        // GET: Contacts
        [Authorize]
        public ActionResult Index()
        { 
         return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }
        [Authorize(Roles = "admin")]
        public ActionResult AddNumber(int id)
        {
            PhoneNumber newnumb = new PhoneNumber();
            newnumb.ContactID = id;
            newnumb.Type = PhoneNumber.PhoneType.Personal;

            db.PhoneNumbers.Add(newnumb);
            db.SaveChanges();

            return RedirectToAction("Edit", new { ID = id });
        }
        // GET: Contacts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Adress")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Adress,PhoneNumbers")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                foreach (PhoneNumber PhNum in contact.PhoneNumbers)
                {
                    db.Entry(PhNum).State = EntityState.Modified;
                }

                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id, int? PhId = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (PhId == null)
            {
                Contact contact = db.Contacts.Find(id);
                if (contact == null)
                {
                    return HttpNotFound();
                }
                return View(contact);
            }

            else
            {
                PhoneNumber PhNumber = db.Contacts.Find(id).PhoneNumbers.Find(x => x.ID == PhId);
                db.PhoneNumbers.Remove(PhNumber);
                db.SaveChanges();
                return RedirectToAction("Edit", new { ID = id });

            }



        }


        //public ActionResult Delete(int? id, int? PhId)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Contact contact = db.Contacts.Find(id);
        //    if (contact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(contact);
        //}


        // POST: Contacts/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
