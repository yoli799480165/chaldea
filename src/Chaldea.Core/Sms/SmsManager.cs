using System.Threading.Tasks;

namespace Chaldea.Core.Sms
{
    public class SmsManager
    {
        private readonly ISmsService _smsService;

        public SmsManager(ISmsService smsService)
        {
            _smsService = smsService;
        }

        public async Task GenerateCode(string phoneNumber)
        {
            var code = "";

            await _smsService.Send(phoneNumber, code);
        }
    }
}