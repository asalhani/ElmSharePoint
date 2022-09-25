using System.Web;
using System.Web.Mvc;

namespace Awqaf.Sharepoint.WrapperAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
