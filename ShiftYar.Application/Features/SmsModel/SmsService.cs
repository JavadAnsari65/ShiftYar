using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShiftYar.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Application.Features.SmsModel
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly IConfiguration _configuration;

        public SmsService(ILogger<SmsService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            // در نسخه واقعی: اتصال به سرویس پیامک
            _logger.LogInformation($"ارسال پیامک به {phoneNumber}: {message}");

            var sender = _configuration["KavenegarConfig:Sender"];
            var receptor = phoneNumber;
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_configuration["KavenegarConfig:Key"]);
            api.Send(sender, receptor, message);

            await Task.CompletedTask;
        }
    }
}
