using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Request
    {
        [Key]
        [Display(Name = "Talep No")]
        public int Id { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Display(Name = "Talep Tarihi")]
        public DateTime CreatedDate { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public string RequesterId { get; set; }
        public LibraryUser Requester { get; set; }

        public string ReceiverId { get; set; }
        public LibraryUser Receiver { get; set; }
    }
}
