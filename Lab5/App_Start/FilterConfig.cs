using System.Web;
using System.Web.Mvc;
using Lab5.CustomAttributes;

namespace Lab5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireSecureConnectionFilter());
        }
    }
}
