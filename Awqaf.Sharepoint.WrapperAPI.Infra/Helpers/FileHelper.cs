using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awqaf.Sharepoint.WrapperAPI.Infra.Helpers
{
    public static class FileHelper
    {
        public static string GetExtension(string fileName)
        {
            string retVal = fileName;
            int lastIndex = retVal.LastIndexOf('.');
            if (lastIndex == retVal.Length - 1)
            {
                lastIndex = retVal.Substring(0, lastIndex).LastIndexOf('.');
            }
            return lastIndex > 0 ? retVal.Substring(lastIndex + 1) : retVal;
        }
    }
}
