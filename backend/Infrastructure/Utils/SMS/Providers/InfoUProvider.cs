using System.Net;
using System.Text;
using System.Web;
using Application.Configs.Providers;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Utils.SMS.Providers;

public class InfoUProvider : BaseSMSProvider {
    private readonly StringBuilder _buffer = new();
    private readonly InfoUSmsConfig _config;
    private string _requestContent;
    public InfoUProvider(ILoggerFactory logger, InfoUSmsConfig config) : base(logger) {
        _logger.LogInformation("InfoUProvider Loaded");
        _config = config;
    }
    public override void Init(string countryNumber, string phoneNumber, string messageTemplate) {
        base.Init(countryNumber, phoneNumber, messageTemplate);
        _logger.LogInformation("InfoUProvider init");
        _buffer.Clear();
        _requestContent = ParseBuffer(_config.UserName, _config.Token, _config.Sender);
    }

    private string ParseBuffer(string username, string token, string sender) {
        _buffer.Append("<Inforu>");
        _buffer.Append("<User>");
        _buffer.Append($"<Username>{username}</Username>");
        _buffer.Append($"<ApiToken>{token}</ApiToken>");
        _buffer.Append("</User>");
        _buffer.Append("<Content Type=\"sms\">");
        _buffer.Append($"<Message>{_messageTemplate}</Message>");
        _buffer.Append("</Content>");
        _buffer.Append("<Recipients>");
        _buffer.Append($"<PhoneNumber>{_phoneNumber}</PhoneNumber>");
        _buffer.Append("</Recipients>");
        _buffer.Append("<Settings>");
        _buffer.Append($"<Sender>{sender}</Sender>");
        _buffer.Append("<MessageInterval>0</MessageInterval>");
        _buffer.Append($"<TimeToSend>{_timeToSend}</TimeToSend>");
        _buffer.Append("</Settings>");
        _buffer.Append("</Inforu >");
        _logger.LogInformation("InfoUProvider Parse XML {0}", _buffer.ToString());
        return HttpUtility.UrlEncode(_buffer.ToString(), Encoding.UTF8);
    }
    public override void SendSMS(string code) {
        base.SendSMS(code);
        _logger.LogInformation("InfoUProvider SendSMS");
        var recipientPhoneNumberMessage = string.Format("Recipient phone number: {0}", _phoneNumber);
        _requestContent = _requestContent.Replace("%23%23code%23%23", code);
        var szResult = PostDataToURL("https://uapi.inforu.co.il/SendMessageXml.ashx", $"InforuXML={_requestContent}");
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(recipientPhoneNumberMessage);
        stringBuilder.AppendLine(szResult);
        _logger.LogInformation("The message was sent \n XML = {0} \n Response = {1}", _requestContent, stringBuilder.ToString());
    }

    private string PostDataToURL(string szUrl, string szData) {
        //Setup the web request
        var szResult = string.Empty;
        var request = WebRequest.Create(szUrl);
        request.Timeout = 30000;
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        //Set the POST data in a buffer
        byte[] buf;
        try {
            // replacing " " with "+" according to Http post RPC
            szData = szData.Replace(" ", "+");
            //Specify the length of the buffer
            buf = Encoding.UTF8.GetBytes(szData);
            request.ContentLength = buf.Length;
            //Open up a request stream
            var rstream = request.GetRequestStream();
            //Write the POST data
            rstream.Write(buf, 0, buf.Length);
            //Close the stream
            rstream.Close();
            //Create the Response object
            WebResponse response;
            response = request.GetResponse();
            //Create the reader for the response
            var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //Read the response
            szResult = sr.ReadToEnd();
            //Close the reader, and response
            sr.Close();
            response.Close();
            return szResult;
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message);
            return szResult;
        }
    }
}