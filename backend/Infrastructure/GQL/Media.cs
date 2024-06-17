using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Infrastructure.GQL;

[Description("Media File Info")]
public class Media
{
    [Description("Media id")]
    public int Id { get; set; }

    [Description("Media file name")]
    public string FileName { get; set; }

    [Description("Media path")]
    public string Path { get; set; }

    [Description("Media file mine type")]
    public string MimeType { get; set; }
    
    [Description("Media file size")]
    public long FileSize { get; set; }
    
    [Description("Media bucket name")]
    public string BucketName { get; set; }
}
