using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    public static class PathUtility
    {
        public static string GetBasePath()
        {
            var path = Assembly.GetCallingAssembly().Location;
            var actualPath = path[..path.LastIndexOf("bin")];
            return new Uri(actualPath).LocalPath;
        }
    }
}
