using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Book
    {
        [Key]
        [Display(Name = "Kitap No")]
        public int Id { get; set; }

        [Display(Name = "Kitap Adı")]
        public string Name { get; set; }

        [Display(Name = "Yazar")]
        public string Author { get; set; }

        [Display(Name = "Yayın Yılı")]
        public int PublishDate { get; set; }

        [Display(Name = "Yayınevi")]
        public string Publisher { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string ImageUrl { get; set; }

        [Display(Name = "Sisteme Eklenme Tarihi")]
        public DateTime CreatedDate { get; set; }

        public virtual IList<BookRecord> BookRecords { get; set; }

    }
}
