using IdentityChatMailDay.Context;
using IdentityChatMailDay.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityChatMailDay.Controllers
{
    public class MessageController : Controller
    {
        private readonly MailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(MailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.v1 = values.Name + " " + values.Surname;
            ViewBag.v2 = values.Email;
            return View();
        }
        public async Task<IActionResult> Inbox(string search)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var email = user.Email;

            var query = _context.Messages
                .Where(x => x.ReceiverEmail == email);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.Subject.Contains(search) ||
                    x.MessageDetail.Contains(search));
            }

            var result = await query.OrderByDescending(x => x.SendDate).ToListAsync();
            return View(result);
        }

        //public async Task<IActionResult> Inbox()
        //{
        //    var values = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var messageList = _context.Messages.Where(x => x.ReceiverEmail == values.Email).ToList();

        //    return View(messageList);
        //}

        public async Task<IActionResult> Sendbox()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            string email = values.Email;

            var sendMessageList = _context.Messages.Where(x => x.SenderEmail == email).ToList();
            
            return View(sendMessageList);
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);

            string senderMail=values.Email;
            message.SenderEmail = senderMail;

            message.SendDate = DateTime.Now;
            message.IsRead = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            TempData["MessageSent"] = "Mesaj başarıyla gönderildi!";
            return RedirectToAction("Sendbox");
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            var values = await _context.Messages.FindAsync(id);

            return View(values);
            
        }

    }
}
