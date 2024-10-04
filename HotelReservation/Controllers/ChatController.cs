using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservation.Controllers
{
    public class ChatController : Controller
    {
        // Simulated message storage (in-memory)
        private static List<Message> _messages = new List<Message>();


        [HttpGet]
        public IActionResult Index(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
            {
                return BadRequest("Receiver ID is required.");
            }

            ViewBag.ReceiverId = receiverId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveMessage(string senderId, string receiverId, string content)
        {
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(content))
            {
                return Json(new { success = false, message = "All fields are required" });
            }

            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.Now
            };

            _messages.Add(message);

            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetMessageHistory(string senderId, string receiverId)
        {
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId))
            {
                return Json(new { success = false, message = "Both sender and receiver IDs are required" });
            }

            var messageHistory = _messages
                .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == senderId))
                .OrderBy(m => m.Timestamp)
                .ToList();

            return Json(new { success = true, messages = messageHistory });
        }
    }
}
