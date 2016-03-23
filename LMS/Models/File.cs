using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum UploadedFileType
    {
        TaskOut,
        TaskIn
    }

    public class UploadedFile
    {
        public UploadedFileType Type { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
    }
}