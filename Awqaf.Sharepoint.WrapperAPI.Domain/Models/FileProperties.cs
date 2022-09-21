using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Domain.Models
{
    public class FileProperties
    {
        [Required(ErrorMessage = "File Path is Required")]
        public string FilePath { get; set; }

        [Required(ErrorMessage = "File Content is Required")]

        public byte[] FileContent { get; set; }

        [Required(ErrorMessage = "File Name is Required")]
        public string FileName { get; set; }
    }
}
