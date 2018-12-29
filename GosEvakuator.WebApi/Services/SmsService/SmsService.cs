using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace GosEvakuator.WebApi.Services.SmsService
{
    public class SmsService : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            string accountSid = "AC853c8a8f4f8813b8e56cb5f7ea5c3eab";
            string authToken = "859b54d3da5480251cd35ca3bab6efc1";
            string twilioPhoneNumber = "+18627033037";

            var twilio = new TwilioRestClient(accountSid, authToken);

            TwilioClient.Init(accountSid, authToken);
            var twilioMessage = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                to: new Twilio.Types.PhoneNumber(number),
                body: message);


            return Task.FromResult(0);
        }
    }
}