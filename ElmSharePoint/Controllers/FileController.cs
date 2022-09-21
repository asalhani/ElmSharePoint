using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Awqaf.Sharepoint.WrapperAPI.Domain.Models;
using Awqaf.Sharepoint.WrapperAPI.Infra.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ElmSharePoint.Controllers
{
    public class FileController : ApiController
    {
        ISharePointManager _sharePointManager;
        IFolderOperations _folderOperations;
        IFileOperations _fileOperations;
        private static readonly log4net.ILog log = log4net.LogManager
            .GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FileController()
        {
            _sharePointManager = new SharePointManager();
        }
        [HttpPost]
        public async Task<string> UploadFile([FromBody] FileProperties fileProperties)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var context = await _sharePointManager.GetClientContext();
                    var list = await _sharePointManager.GetListAsync();
                    _fileOperations = new FileOperations(context, list);
                    _folderOperations = new FolderOperations(context, list);
                    Folder folder = await _folderOperations.GetFoldersWithCreation(fileProperties.FilePath);

                    string url = await _fileOperations.UploadFileToDocLibrary(fileProperties, folder);
                    return url;
                }
                else
                {
                    string errorMessage = string.Empty;
                    foreach (var err in ModelState.Values)
                    {
                        foreach (var error in err.Errors)
                            errorMessage += error.ErrorMessage + " , ";
                    }

                    return errorMessage;
                }

            }
            catch (Exception ex)
            {
                log.Fatal("UploadFile Exception : " + ex.Message);
                return ex.Message;
            }
        }

        [HttpGet]
        public async Task<byte[]> GetFile([FromUri] string fileUrl)
        {
            try
            {
                var context = await _sharePointManager.GetClientContext();
                var list = await _sharePointManager.GetListAsync();
                _fileOperations = new FileOperations(context, list);
                return await _fileOperations.GetFile(fileUrl);

            }
            catch (Exception ex)
            {
                log.Fatal("GetFile Exception : " + ex.Message);
                return null;
            }

        }




    }
}
