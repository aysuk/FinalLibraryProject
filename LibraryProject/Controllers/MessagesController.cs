using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<LibraryUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public MessagesController(ApplicationDbContext context, UserManager<LibraryUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Inbox()
        {
            var userEmail = User.Identity.Name;
            var userId = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault().Id;

            var inboxMessages = _context.Messages.Where(m => m.ReceiverId == userId).Include(m => m.Sender).OrderByDescending(m => m.CreatedDate).ToList();

            return View(inboxMessages);
        }

        public IActionResult Sent()
        {
            var userEmail = User.Identity.Name;
            var userId = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault().Id;

            var sentMessages = _context.Messages.Where(m => m.SenderId == userId).Include(m => m.Receiver).OrderByDescending(m => m.CreatedDate).ToList();

            return View(sentMessages);
        }

        public IActionResult ComposeMessage(string Id)
        {
            var message = new Message();

            if (!String.IsNullOrEmpty(Id))
            {
                message.ReceiverId = Id;
            }

            var userEmail = User.Identity.Name;
            var userId = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault().Id;

            var allUsers = _context.Users.ToList();

            var userList = new List<UserViewModel>();

            foreach (var user in allUsers)
            {
                var userData = new UserViewModel() { FullName = user.FirstName + " " + user.LastName, Id = user.Id };
                userList.Add(userData);
            }

            ViewData["ReceiverId"] = new SelectList(userList, "Id", "FullName");

            return View(message);
        }

        [HttpPost]
        public async Task<IActionResult> ComposeMessage([Bind("Title,Content,ReceiverId")] Message message, IFormFile attachmentFile)
        {
            var userEmail = User.Identity.Name;
            var userId = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault().Id;

            if (String.IsNullOrEmpty(message.Title) || String.IsNullOrEmpty(message.Content) || String.IsNullOrEmpty(message.ReceiverId) || message.ReceiverId == userId)
            {
                ModelState.AddModelError(string.Empty, "Lütfen içeriği düzenleyiniz.");

                if(message.ReceiverId == userId)
                {
                    ModelState.AddModelError(string.Empty, "Kendinize mesaj gönderemezsiniz.");
                }

                var allUsers = _context.Users.ToList();

                var userList = new List<UserViewModel>();

                foreach (var user in allUsers)
                {
                    var userData = new UserViewModel() { FullName = user.FirstName + " " + user.LastName, Id = user.Id };
                    userList.Add(userData);
                }

                ViewData["ReceiverId"] = new SelectList(userList, "Id", "FullName");

                return View();
            }
            else
            {
                // File upload from cetstore project
                if (attachmentFile != null)
                {
                    string dirPath = Path.Combine(_hostingEnvironment.WebRootPath, @"uploads\");
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + attachmentFile.FileName;
                    using (var fileStream = new FileStream(dirPath + fileName, FileMode.Create))
                    {
                        await attachmentFile.CopyToAsync(fileStream);
                    }

                    message.AttachmentUrl = fileName;
                }
                
                message.CreatedDate = DateTime.Now;
                message.SenderId = userId;
                message.IsDraft = false;
                message.IsRead = false;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Sent");
        }

        public IActionResult MessageDetail(int id)
        {
            var message = _context.Messages.Where(m => m.Id == id).Include(m => m.Sender).Include(m => m.Receiver).FirstOrDefault();

            if(message == null)
            {
                return NotFound();
            }

            var messageDetail = new MessageDetail();

            var userEmail = User.Identity.Name;
            var userId = _context.Users.Where(u => u.Email == userEmail).FirstOrDefault().Id;

            messageDetail.Message = message;

            if (message.SenderId == userId)
            {
                messageDetail.User = message.Receiver;
                messageDetail.IsCurrentUserSender = true;
            }
            else if(message.ReceiverId == userId)
            {
                messageDetail.User = message.Sender;
                messageDetail.IsCurrentUserSender = false;
            }
            else
            {
                return NotFound();
            }

            return View(messageDetail);
        }

        //[HttpGet]
        //public IActionResult DownloadAttachment(string id)
        //{
        //    //var net = new System.Net.WebClient();
        //    //id = "/uploads/" + id;
        //    //var data = net.DownloadData(id);
        //    //var content = new System.IO.MemoryStream(data);
        //    //var contentType = "APPLICATION/octet-stream";
        //    //var fileName = "something.bin";
        //    //return File(content, contentType, fileName);

        //    string dirPath = Path.Combine(_hostingEnvironment.WebRootPath, @"uploads\");
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}
    }
}