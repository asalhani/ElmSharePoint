using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Awqaf.Sharepoint.WrapperAPI.Domain.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Infra.Repositories
{
    public class FileOperations : IFileOperations
    {
        ClientContext _clientContext;
        List _list;
        public FileOperations(ClientContext clientContext, List list)
        {
            _list = list;
            _clientContext = clientContext;
        }

        public byte[] GetFile(string fileUrl)
        {
            var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(_clientContext, fileUrl);
            if (fileInfo != null)
            {
                using (var stream = fileInfo.Stream)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        byte[] barray = memoryStream.ToArray();
                        return barray;
                    }
                }
            }
            else
            {
                return null;
            }

        }

        public async Task<string> UploadFileToDocLibrary(FileProperties fileProperties, Folder folder)
        {

            var fileCreationInformation = new FileCreationInformation();
            fileCreationInformation.Content = fileProperties.FileContent;
            //Allow owerwrite of document

            fileCreationInformation.Overwrite = true;
            //Upload URL

            fileCreationInformation.Url = fileProperties.FileName;

            Microsoft.SharePoint.Client.File uploadFile = folder.Files.Add(fileCreationInformation);

            _clientContext.Load(uploadFile, f => f.ServerRelativeUrl);
            await _clientContext.ExecuteQueryAsync();
            return uploadFile.ServerRelativeUrl;


        }
    }
}
