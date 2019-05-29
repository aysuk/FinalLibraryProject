using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class LibraryUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string PhotoUrl { get; set; }

        [StringLength(1000)]
        public string About { get; set; }

        public virtual IList<BookRecord> BookRecords { get; set; }
    }
}
