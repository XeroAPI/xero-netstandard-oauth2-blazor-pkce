using System;

namespace BlazorFilesApp.Shared
{
    public class XeroUpload
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public Guid? FolderId { get; set; }
    }
}
