using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Comarch.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage="Pole może mieć maksymalnie 50 znaków!")]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "Opis może mieć maksymalnie 200 znaków!")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Pole może mieć maksymalnie 50 znaków!")]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Range(1000, 2015, ErrorMessage="Podaj poprawną datę!")]
        [RegularExpression("([0-9]+)", ErrorMessage="Rok musi być liczbą!")]
        [Display(Name = "Rok wydania")]
        public int? Year { get; set; }
        
        public int CoverId { get; set; }
        public int GenreId { get; set; }

        [Required]
        public virtual Cover Cover { get; set; }
        [Required]
        public virtual Genre Genre { get; set; }
    }

    public class Cover
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Display(Name = "Okładka")]
        public string Data { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Display(Name = "Gatunek")]
        public string Data { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }

    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
            //różne odmiany inicjalizacji code first
            //Database.SetInitializer<LibraryContext>(new DropCreateDatabaseAlways<LibraryContext>());
            //Database.SetInitializer<LibraryContext>(null);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Cover> Covers { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}