
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces
{
    public interface IFolderOperations
    {
        Task<Folder> GetFoldersWithCreation(string folderPath);
        Task<Folder> GetFolderByName(string folderName, Folder folder);
        Task<Folder> CreateFolder(Folder folder, string name);
    }
}
