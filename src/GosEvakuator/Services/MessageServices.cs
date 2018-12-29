using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace GosEvakuator.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class MessageServices : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public async Task SendSmsAsync(string number, string message)
        {
            var httpClient = new HttpClient();

            var parameters = new Dictionary<string, string>();
            parameters.Add("api_id", "05C73C31-B3EE-B51C-1602-335148E17A9B");
            parameters.Add("to", number);
            parameters.Add("msg", message);
            parameters.Add("json", "1");
            parameters.Add("from", "GosEvak.ru");

            var query = DictionaryToPostParams(parameters);

            var uri = new Uri(string.Format("{0}?{1}", "https://sms.ru/sms/send", query));

            var result = await httpClient.GetStringAsync(uri);
        }

        public Task SendAuthSmsAsync(string number, string message)
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

        private string DictionaryToPostParams(Dictionary<string,string> parameters)
        {
            var parameterList = new List<string>();

            foreach(var pair in parameters)
            {
                parameterList.Add(string.Format("{0}={1}", pair.Key, pair.Value));
            }

            return String.Join("&", parameterList);
        }
    }
}
