using Microsoft.Extensions.Configuration;

namespace AppCore.Business.Utils.Bases
{
    public abstract class AppSettingsUtilBase
    {
        private readonly IConfiguration _configuration;

        protected AppSettingsUtilBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual T Bind<T>(string sectionKey = "AppSettings") where T : class, new()
        {
            T t = null;
            IConfigurationSection section = _configuration.GetSection(sectionKey);
            if (section != null)
            {
                t = new T();
                section.Bind(t);
            }
            return t;
        }
    }
}
