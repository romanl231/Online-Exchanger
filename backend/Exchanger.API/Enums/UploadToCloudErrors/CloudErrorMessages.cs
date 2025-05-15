using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Enums.UploadToCloudErrors
{
    public static class CloudErrorMessages
    {
        public static readonly Dictionary<CloudErrorCode, string> Messages = new()
        {
            { CloudErrorCode.FileFormat, "Invalid file format." },
            { CloudErrorCode.FileSize, "Size of file can't be more than 5MB" },
        };
    }
}
