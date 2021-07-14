using AppCore.Business.Utils.Bases;
using Microsoft.Extensions.Configuration;

namespace AppCore.Business.Utils
{
    public class AppSettingsUtil : AppSettingsUtilBase
    {
        public AppSettingsUtil(IConfiguration configuration) : base(configuration)
        {
            
        }
    }
}
