using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GosEvakuator.WebApi.Managers;
using Microsoft.AspNetCore.Identity;
using GosEvakuator.WebApi.Services;
using Microsoft.Extensions.Logging;
using GosEvakuator.WebApi.Data;
using GosEvakuator.WebApi.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace GosEvakuator.WebApi.Controllers.Account
{
    [Route("api/[controller]")]
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
            this.logger = loggerFactory.CreateLogger<AccountController>();
            this.dbContext = dbContext;
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody]PhoneNumberModel phoneNumberModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameOrCreateAsync(phoneNumberModel.PhoneNumber, phoneNumberModel.PhoneNumber);
                if (user != null)
                {
                    var code = await userManager.GenerateCodeAsync(user);
                    var phoneNumber = string.Format("{0}{1}", phoneNumberModel.CountryCode, phoneNumberModel.PhoneNumber);
                    var message = string.Format("your code {0}", code);
                    await smsSender.SendSmsAsync(phoneNumber, message);

                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]VerifyCodeModel verifyCodeModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(verifyCodeModel.PhoneNumberModel.PhoneNumber);
                if (user != null)
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    var success = await userManager.VerifyCodeAsync(user, verifyCodeModel.VerifyCode);
                    if (success)
                    {
                        var token = userManager.GenerateToken(user, identity, verifyCodeModel.VerifyCode);
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