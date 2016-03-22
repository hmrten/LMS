using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public enum FileType
    {
        TaskOut,
        TaskIn
    }

    public class File
    {
        public FileType Type { get; set; }
        public string FileName { get; set; }
    }
}