using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Models.Email;

namespace TARge25_Shop.Controllers
{
    public class EmailController : Controller
    {
        //Konstruktor
        private readonly IEmailServices _emailServices;

        public EmailController(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        //
        ////Kuna puudub enda eraldi vaade, siis ebavajalik
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //Vaja teha meetod, mis võtab vastu EMailDto objekti              
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();
            
            //ning teha vahe instats, mis vahendab andmedi EmailDot ja ViewModeli vahel
            var dto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachments = files,
            };

            // ja kutsub välja _emailServices.SendEmail meetodi
            _emailServices.SendEmail(dto);

            ////Kui tahame suunata teise kausta vaatele
            //return RedirectToAction("Index", "Home");

            return RedirectToAction(nameof(Index));
        }
    }
}
