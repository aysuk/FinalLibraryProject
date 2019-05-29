using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<LibraryUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<LibraryUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyProfile()
        {
            var userEmail = User.Identity.Name;

            var user = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            var userProfileModel = new UserProfileModel();

            userProfileModel.Id = user.Id;
            userProfileModel.FirstName = user.FirstName;
            userProfileModel.LastName = user.LastName;
            userProfileModel.About = user.About;
            userProfileModel.Email = user.Email;
            userProfileModel.PhotoUrl = user.PhotoUrl;

            var records = _context.BookRecord.Where(r => r.LibraryUserId == user.Id).Include(r => r.Book).ToList();

            userProfileModel.RecordCount = records != null && records.Count() > 0 ? records.Count() : 0;

            var books = records.Select(r => r.BookId).Distinct().ToList();

            userProfileModel.BookCount = books != null && books.Count() > 0 ? books.Count() : 0;

            bool isReader = await _userManager.IsInRoleAsync(user, "Okuyucu");

            userProfileModel.IsReader = isReader;
            userProfileModel.BookRecords = records;

            // Aşağıdaki kodları internetten araştırdım ancak emin olamadım ve bilen arkadaşlarıma danıştım.

            var requestedBooks = _context.Requests.Where(r => r.RequesterId == user.Id).Include(r => r.Book).Include(r => r.Receiver).ToList();

            userProfileModel.RequestedBooks = requestedBooks;

            var requestedBooks2 = requestedBooks.Select(r => r.BookId).Distinct().ToList();

            userProfileModel.RequestCount = requestedBooks2 != null && requestedBooks2.Count() > 0 ? requestedBooks2.Count() : 0;

            var requestedBooks3 = _context.Requests.Where(r => r.ReceiverId == user.Id).Include(r => r.Book).Include(r => r.Requester).ToList();

            userProfileModel.RequestedBooksReceived = requestedBooks3;

            return View(userProfileModel);
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var user = _context.Users.Where(u => u.Id == id).FirstOrDefault();

            var userEmail = User.Identity.Name;
            var currentUser = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            if(user.Id == currentUser.Id)
            {
                return RedirectToAction("MyProfile");
            }

            if (user == null)
            {
                return NotFound();
            }

            var userProfileModel = new UserProfileModel();

            userProfileModel.Id = id;
            userProfileModel.FirstName = user.FirstName;
            userProfileModel.LastName = user.LastName;
            userProfileModel.About = user.About;
            userProfileModel.Email = user.Email;
            userProfileModel.PhotoUrl = user.PhotoUrl;

            var records = _context.BookRecord.Where(r => r.LibraryUserId == user.Id).Include(r => r.Book).ToList();

            userProfileModel.RecordCount = records != null && records.Count() > 0 ? records.Count() : 0;

            var books = records.Select(r => r.BookId).Distinct().ToList();

            userProfileModel.BookCount = books != null && books.Count() > 0 ? books.Count() : 0;

            bool isReader = await _userManager.IsInRoleAsync(user, "Okuyucu");

            userProfileModel.IsReader = isReader;
            userProfileModel.BookRecords = records;

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name");

            ViewData["IsReader"] = await _userManager.IsInRoleAsync(currentUser, "Okuyucu");

            // Aşağıdaki kodları internetten araştırdım ancak emin olamadım ve bilen arkadaşlarıma danıştım.

            var requestedBooks = _context.Requests.Where(r => r.RequesterId == id).Include(r => r.Book).Include(r => r.Receiver).ToList();

            userProfileModel.RequestedBooks = requestedBooks;

            var requestedBooks2 = requestedBooks.Select(r => r.BookId).Distinct().ToList();

            userProfileModel.RequestCount = requestedBooks2 != null && requestedBooks2.Count() > 0 ? requestedBooks2.Count() : 0;

            return View(userProfileModel);
        }

        public async Task<IActionResult> UserList()
        {
            var users = _context.Users.ToList();

            var vm = new List<UserListViewModel>();

            foreach (var user in users)
            {
                var userModel = new UserListViewModel();
                userModel.UserBasic = user;
                bool isReader = await _userManager.IsInRoleAsync(user, "Okuyucu");
                userModel.IsReader = isReader;
                vm.Add(userModel);
            }

            return View(vm);
        }
    }
}