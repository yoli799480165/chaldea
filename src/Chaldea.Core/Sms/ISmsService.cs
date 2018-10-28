using System.Threading.Tasks;

namespace Chaldea.Core.Sms
{
    public interface ISmsService
    {
        Task<SmsResponse> Send(string phoneNumber, string msg);
    }

    public class SmsResponse
    {
        public string RequestId { get; set; }
        public string BizId { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}