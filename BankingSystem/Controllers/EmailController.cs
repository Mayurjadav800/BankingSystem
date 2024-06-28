//using BankingSystem.Model;
//using BankingSystem.Repository;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;

//namespace BankingSystem.Controllers
//{
//    public class EmailController:ControllerBase
//    {
//        private readonly IEmailRepository emailservice;
//        private readonly ILogger<EmailController> _logger;

//        public EmailController(IEmailRepository emailservice,ILogger<EmailController>logger)
//        {
//            this.emailservice = emailservice;
//            _logger = logger;
//        }
//        [HttpPost("SendMail")]
//        public async Task<IActionResult> SendMail()
//        {
//            try
//            {
//                MailRequest mailRequest = new MailRequest();
//                mailRequest.ToEmail = "jadavmayur800@gamil.com";
//                mailRequest.Subject = "Welcome to Mayur";
//                mailRequest.Body = "Thank you";
//                await emailservice.SendEmailAsync(mailRequest);
//                return Ok();
//            }catch(Exception ex)
//            {
//                throw ();
//            }


//        }
        

//    }
//}
