using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using codeFirstSample.Models;

// Controller that will perform the CRUD operations on the Book entity. 

namespace codeFirstSample.Controllers
{
    public class BooksController : Controller
    {
        BooksDbContext context = new BooksDbContext();

        //
        // GET: /Books/

        public ActionResult Index()
        {
            List<Book> books = context.Books.ToList();
            return View(books);
        }

        //
        // GET: /Books/Details/5

        public ActionResult Details(int id)
        {
            Book book = context.Books.SingleOrDefault(b => b.BookID == id);
            
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // GET: /Books/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Books/Create

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                context.Books.Add(book);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        //
        // GET: /Books/Edit/5

        public ActionResult Edit(int id)
        {
            Book book = context.Books.Single(p => p.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // POST: /Books/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Book book)
        {
            Book _book = context.Books.Single(p => p.BookID == id);

            if (ModelState.IsValid)
            {
                _book.BookName = book.BookName;
                _book.ISBN = book.ISBN;

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        //
        // GET: /Books/Delete/5

        public ActionResult Delete(int id)
        {
            Book book = context.Books.Single(p => p.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        //
        // POST: /Books/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Book book)
        {
            Book _book = context.Books.Single(p => p.BookID == id);
            context.Books.Remove(_book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
