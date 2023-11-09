using Microsoft.AspNetCore.Http;
namespace Infrastructure.Requests.Processor.Services.Common;

public class FileDetail {
    public float MediaImageWidth { get; set; }
    public float MediaImageHeight { get; set; }
    public bool MediaIsMain { get; set; }
    public IFormFile MediaRaw { get; set; }
}