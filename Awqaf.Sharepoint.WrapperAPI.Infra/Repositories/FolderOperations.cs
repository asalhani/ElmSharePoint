using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Infra.Repositories
{
    public class FolderOperations : IFolderOperations
    {
        ClientContext _clientContext;
        List _list;
        public FolderOperations(ClientContext clientContext, List list)
        {
            _list = list;
            _clientContext = clientContext;
        }

        public async Task<Folder> GetFoldersWithCreation(string folderPath)
        {
            Folder folder = await getServices();
            List<string> folderLevels = folderPath.Split('/').AsEnumerable().Where(x => x != "" & x != " ").ToList();


            for (int i = 0; i < folderLevels.Count; i++)
            {
                string folderName = folderLevels[i];
                if (!string.IsNullOrWhiteSpace(folderName))
                {
                    FolderOperations folderOperations = new FolderOperations(_clientContext, _list);
                    folder = await GetFolderByName(folderName, folder);
                }
            }
            return folder;
        }


        //get folder for eservices
        public async Task<Folder> getServices()
        {
            string serviceFolder = ConfigurationManager.AppSettings["Services"];
            //get Eservices
            Folder folder = _list.RootFolder.Folders.GetByUrl(_list.RootFolder.ServerRelativeUrl + "/" + serviceFolder);
            _clientContext.Load(folder);
            await _clientContext.ExecuteQueryAsync();

            return folder;

        }


        //Create Folder 
        public async Task<Folder> CreateFolder(Folder folder, string name)
        {
            Folder folderCreated = folder.Folders.Add(name);
            _clientContext.Load(folderCreated);
            await _clientContext.ExecuteQueryAsync();
            return folderCreated;
        }


        //Get folder By name 
        public async Task<Folder> GetFolderByName(string folderName, Folder folder)
        {

            if (_list != null && _list.ItemCount > 0)
            {
                Microsoft.SharePoint.Client.CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml =
                   @"<View Scope='RecursiveAll'>  
                               <Query> 
                                  <Where><And><Eq><FieldRef Name='FileLeafRef' /><Value Type='File'>" + folderName + @"</Value></Eq><Eq><FieldRef Name='FSObjType' /><Value Type='Integer'>1</Value></Eq></And></Where> 
                               </Query> 
                         </View>";

                ListItemCollection listItems = _list.GetItems(camlQuery);
                _clientContext.Load(listItems);
                await _clientContext.ExecuteQueryAsync();


                if (listItems.Count == 0)
                {
                    Folder folderCreated = folder.Folders.Add(folderName);
                    _clientContext.Load(folderCreated);
                    await _clientContext.ExecuteQueryAsync();
                    return folderCreated;
                }
                Folder folderFounded = listItems.FirstOrDefault().Folder;
                return folderFounded;
            }
            return null;

        }

    }
}
