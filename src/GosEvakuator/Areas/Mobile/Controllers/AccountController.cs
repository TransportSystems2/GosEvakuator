using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

using GosEvakuator.Areas.Mobile.Models;
using GosEvakuator.Data;
using GosEvakuator.Models;
using GosEvakuator.Options;
using GosEvakuator.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using GosEvakuator.Managers;

namespace GosEvakuator.Areas.Mobile.Controllers
{
    [Area("Mobile")]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ISmsSender smsSender;
        private readonly ILogger logger;
        private readonly ApplicationDbContext dbContext;

        public AccountController(
            ApplicationUserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.smsSender = smsSender;
            this.logger = loggerFactory.CreateLogger<Controllers.AccountController>();
            this.dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendCode([FromBody]SendCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameOrCreateAsync(model.PhoneNumber, model.PhoneNumber);
                if (user != null)
                {
                    var code = await userManager.GenerateCodeAsync(user);
                    var phoneNumber = string.Format("{0}{1}", model.CountryCode, model.PhoneNumber);
                    var message = string.Format("your code {0}", code);
                    await smsSender.SendAuthSmsAsync(phoneNumber, message);

                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Token([FromBody]ConfirmCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.PhoneNumber);
                if (user != null)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var success = await userManager.VerifyCodeAsync(user, model.VerifyCode);
                    if (success)
                    {
                        var token = userManager.GenerateToken(user, identity, model.VerifyCode);
                        if (token != null)
                        {
                            await signInManager.SignInAsync(user, false);

                            var tokenModel = new TokenModel();
                            tokenModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
                            tokenModel.UserId = user.Id;

                            return Ok(tokenModel);
                        }
                    }
                }
            }

            return BadRequest();
        }
    }
}