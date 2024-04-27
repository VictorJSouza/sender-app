using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Sender.Model;
using Sender.Services;

namespace Sender.Controllers
{
    [Route("sender/messages")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public MessageService messageService;

        public MessageController(MessageService messageService)
        {
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid chatId)
        {
            try
            {
                ICollection<Message> message = await messageService.GetAllByChatId(chatId);
                return Ok(message);
            }
            catch (Exception error)
            {
                return NotFound(error);
            }
        }
    }
}