using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class Message
    {
        [Key]
        [Display(Name = "Mesaj No")]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AttachmentUrl { get; set; }

        [Display(Name = "Gönderim Tarihi")]
        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsDraft { get; set; }

        public string SenderId { get; set; }
        public LibraryUser Sender { get; set; }

        public string ReceiverId { get; set; }
        public LibraryUser Receiver { get; set; }
    }
}
