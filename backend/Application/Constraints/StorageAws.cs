using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Constraints;

public class StorageAws
{
    public const string BucketUploadBins= "upload-bins";
    public const string BucketUserMedia= "media-user-files";
    public const string BucketGlobalMedia= "global-media-files";
}

public class StorageGlobalPath {
    public const string Providers = "providers";
    public static string Provider(int providerId) => Path.Combine(Providers, providerId.ToString());
    public const string Vendors = "vendors";
    public static string Vendor(int providerId) => Path.Combine(Vendors, providerId.ToString());
}

public class StorageUserPath {
    public static string Providers(Guid userId) => Path.Combine(userId.ToString(), "providers");
    public static string Vendors(Guid userId) => Path.Combine(userId.ToString(), "vendors");
}