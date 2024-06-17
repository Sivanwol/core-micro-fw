using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contract;

public class FileUpload
{
    public string FileName { get; set; }
    public string Path { get; set; }
    public string MineType { get; set; }
    public long FileSize { get; set; }
}
