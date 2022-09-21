using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Domain.Interfaces
{
    public interface ISharePointManager
    {
        Task<ClientContext> GetClientContext();

        Task<List> GetListAsync();
    }
}
