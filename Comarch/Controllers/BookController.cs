using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Comarch.Models;

namespace Comarch.Controllers
{
    public class BookController : Controller
    {
        LibraryContext context = new LibraryContext();

        public ActionResult Index() 
        {
            initialize();
            prepare();
            return View();
        }

        public ActionResult Create()
        {
            prepare();

            Book book = new Book { Id = 0, CoverId = 1, GenreId = 1, Year = 2000 };
            return PartialView("ModalView", book);
        }

        [HttpPost]
        public ActionResult Update()
        {
            prepare();

            int id = Int32.Parse(Request.Form["id"]);
            Book book = new Book { Id = 0 };

            if (id != 0)
                book = context.Books.Find(id);

            return PartialView("ModalView", book);
        }

        [HttpPost]
        public ActionResult Delete()
        {
            int id = Int32.Parse(Request.Form["id"]);

            if(id != 0)
            {
                Book book = context.Books.Find(id);
                context.Books.Remove(book);
                context.SaveChanges();
            }

            List<Book> books = context.Books.ToList();
            return PartialView("ListView", books);
        }

        public ActionResult ShowBook(Book book)
        {
            return PartialView("BookView", book);
        }
        
        public ActionResult GetBooks()
        {
            List<Book> books = context.Books.ToList();
            return PartialView("ListView", books);
        }
        
        [HttpPost]
        public ActionResult GetBooks(Book book)
        {
            prepare();
            book.Cover = context.Covers.Find(book.CoverId);
            book.Genre = context.Genres.Find(book.GenreId);

            if(book.Id != 0)
            {
                Book edited = context.Books.Find(book.Id);
                edited.Title = book.Title;
                edited.Description = book.Description;
                edited.Author = book.Author;
                edited.Year = book.Year; 
                edited.Cover = book.Cover;
                edited.Genre = book.Genre;
            }
            else
                context.Books.Add(book);

            context.SaveChanges();

            List<Book> books = context.Books.ToList();
            return PartialView("ListView", books);
        }

        [HttpPost]
        public ActionResult FilterList()
        {
            prepare();

            string author = Request.Form["author"];
            string title = Request.Form["title"];

            List<Book> books = context.Books.ToList();

            if(author.Length != 0)
                books = books.FindAll(k => Regex.IsMatch(k.Author, author + ".*"));

            if (title.Length != 0)
                books = books.FindAll(k => Regex.IsMatch(k.Title, title + ".*"));

            return PartialView("ListView", books);
        }

        void prepare()
        {
            ViewBag.Covers = context.Covers.ToList();
            ViewBag.Genres = context.Genres.ToList();
        }

        //funkcja inicjalizuje bazę przy pierwszym uruchomieniu
        void initialize()
        {
            if (context.Covers.Count() == 0)
            {
                context.Covers.Add(new Cover { Data = "twarda" });
                context.Covers.Add(new Cover { Data = "miękka" });
            }

            context.SaveChanges();

            if (context.Genres.Count() == 0)
            {
                context.Genres.Add(new Genre { Data = "sf" });
                context.Genres.Add(new Genre { Data = "sensacja" });
                context.Genres.Add(new Genre { Data = "kryminał" });
            }

            context.SaveChanges();

            if (context.Books.Count() == 0)
            {
                Book book = new Book { Id = 0, Author = "Grzegorz XV", CoverId = 1, GenreId = 1, Description = "", Title = "Nie ma nas", Year = 1901 };
                book.Cover = context.Covers.Find(1);
                book.Genre = context.Genres.Find(1);
                context.Books.Add(book);

                book = new Book { Id = 0, Author = "Miodek", CoverId = 1, GenreId = 1, Description = "ąćęłńóśźż", Title = "Polskie znaki", Year = 1999 };
                book.Cover = context.Covers.Find(2);
                book.Genre = context.Genres.Find(2);
                context.Books.Add(book);
            }

            context.SaveChanges();
        }
    }
}
