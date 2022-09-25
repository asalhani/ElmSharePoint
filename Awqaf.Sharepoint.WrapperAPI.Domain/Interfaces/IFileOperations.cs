using Awqaf.Sharepoint.WrapperAPI.Domain.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces
{
    public interface IFileOperations
    {
        Task<string> UploadFileToDocLibrary(FileProperties fileProperties, Folder folder);
        byte[] GetFile(string fileUrl);
    }
}
