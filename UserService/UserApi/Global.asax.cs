using System;
using System.Web.Http;

namespace UserApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(StartUp.Configure);
        }

        void Application_End(object sender, EventArgs e)
        {
            StartUp.CleanUp();
        }
    }
}
