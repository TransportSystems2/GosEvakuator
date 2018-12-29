using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GosEvakuator.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);

        Task SendAuthSmsAsync(string number, string message);
    }
}
