using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class BookRecord
    {
        [Key]
        [Display(Name = "Kayıt No")]
        public int Id { get; set; }

        public string FileUrl { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Display(Name = "Sisteme Eklenme Tarihi")]
        public DateTime CreatedDate { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public string LibraryUserId { get; set; }
        public LibraryUser LibraryUser { get; set; }
    }
}
