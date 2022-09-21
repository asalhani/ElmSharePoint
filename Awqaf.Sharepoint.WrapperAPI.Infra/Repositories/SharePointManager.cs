using Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Infra.Repositories
{
    public class SharePointManager : ISharePointManager
    {
        private ClientContext _clientContext;
        public async Task<ClientContext> GetClientContext()
        {
            ClientContext clientContext = new ClientContext(ConfigurationManager.AppSettings["DMSUrl"])
            {
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["DMSUserName"],
               ConfigurationManager.AppSettings["DMSPassword"], ConfigurationManager.AppSettings["DMSDomain"])
            };
            _clientContext = clientContext;
            return clientContext;
        }


        public async Task<List> GetListAsync()
        {
            Web web = _clientContext.Web;
            string listName = ConfigurationManager.AppSettings["DMSListName"];
            List _list = web.Lists.GetByTitle(listName);
            _clientContext.Load(_list, f => f.RootFolder.ServerRelativeUrl, f => f.ItemCount);
            _clientContext.ExecuteQuery();
            return _list;
        }


    }
}
