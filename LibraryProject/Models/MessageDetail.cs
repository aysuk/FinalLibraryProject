using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class MessageDetail
    {
        public Message Message { get; set; }
        public LibraryUser User { get; set; }
        public bool IsCurrentUserSender { get; set; }
    }
}
