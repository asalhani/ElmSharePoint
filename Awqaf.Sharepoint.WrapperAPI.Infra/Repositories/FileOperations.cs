using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Awqaf.Sharepoint.WrapperAPI.Domain.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public async Task<byte[]> GetFile(string fileUrl)
        {
            var fileInfo = File.OpenBinaryDirect(_clientContext, fileUrl);
            var stream = fileInfo.Stream;
            IList<byte> content = new List<byte>();
            int b;
            while ((b = fileInfo.Stream.ReadByte()) != -1)
            {
                content.Add((byte)b);
            }
            byte[] barray = content.ToArray();
            return barray;


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
                _clientContext.ExecuteQuery();
                return uploadFile.ServerRelativeUrl;
            

        }
    }
}
