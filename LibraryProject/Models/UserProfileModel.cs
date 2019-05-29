using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Models
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public int RecordCount { get; set; }
        public int BookCount { get; set; }
        public int RequestCount { get; set; }
        public bool IsReader { get; set; }
        public List<BookRecord> BookRecords { get; set; }
        public Request BookRequest { get; set; }
        public List<Request> RequestedBooks { get; set; }
        public List<Request> RequestedBooksReceived { get; set; }
    }
}
