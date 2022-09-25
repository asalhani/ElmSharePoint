using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Awqaf.Sharepoint.WrapperAPI.Domain.Models;
using Awqaf.Sharepoint.WrapperAPI.Infra.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Awqaf.Sharepoint.WrapperAPI.Controllers
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
        public async Task<object> UploadFile([FromBody] FileProperties fileProperties)
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
                    var result = new
                    {
                        data = new
                        {
                            url = url
                        },
                        isSuccess = true
                    };
                    return result;
                }
                else
                {
                    var result = new
                    {
                        data = new List<object>(),
                        isSuccess = false
                    };

                    //string errorMessage = string.Empty;
                    foreach (string key in ModelState.Keys)
                    {
                        foreach (var error in ModelState[key].Errors)
                        {
                            result.data.Add(new
                            {
                                code = "ValidationError",
                                message = error.ErrorMessage,
                                errorData = key
                            });
                        }

                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                var result = new
                {
                    data = new
                    {
                        code = "ExceptionError",
                        message = ex.Message,
                        isSuccess = false
                    },
                    isSuccess = false
                };
                log.Fatal("UploadFile Exception : " + ex.Message);
                return result;
            }
        }

        [HttpGet]
        public async Task<object> GetFile([FromUri] string fileUrl)
        {
            try
            {
                var context = await _sharePointManager.GetClientContext();
                var list = await _sharePointManager.GetListAsync();
                _fileOperations = new FileOperations(context, list);
                byte[] barray = _fileOperations.GetFile(fileUrl);

                var result = new
                {
                    data = new
                    {
                        fileBytes = barray
                    },
                    isSuccess = true
                };
                return result;

            }
            catch (Exception ex)
            {
                var result = new
                {
                    data = new
                    {
                        code = "ExceptionError",
                        message = ex.Message,
                        isSuccess = false
                    },
                    isSuccess = false
                };
                log.Fatal("GetFile Exception : " + ex.Message);
                return result;
            }

        }




    }
}
