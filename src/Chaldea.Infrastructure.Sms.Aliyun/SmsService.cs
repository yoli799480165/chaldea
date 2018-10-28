using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Chaldea.Core.Sms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chaldea.Infrastructure.Sms.Aliyun
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly SmsServiceSettings _options;

        public SmsService(ILogger<SmsService> logger, IOptions<SmsServiceSettings> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public Task<SmsResponse> Send(string phoneNumber, string code)
        {
            var profile = DefaultProfile.GetProfile("cn-hangzhou", _options.AccessKeyId, _options.AccessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", _options.Product, _options.Domain);
            var acsClient = new DefaultAcsClient(profile);
            var request = new SendSmsRequest();
            SendSmsResponse response = null;
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phoneNumber;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = "迦勒底动漫";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = "SMS_145910390";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\"" + code + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                // request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                response = acsClient.GetAcsResponse(request);
            }
            catch (ServerException e)
            {
                _logger.LogError(e.ErrorCode, e.ErrorMessage);
            }

            return Task.FromResult(new SmsResponse
            {
                RequestId = response.RequestId,
                BizId = response.BizId,
                Code = response.Code,
                Message = response.Message
            });
        }
    }
}