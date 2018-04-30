using Umbraco.Core;
using Umbraco.Core.Services;

namespace Skrift.Web.Events
{
    public class Startup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbraco, ApplicationContext context)
        {
            ContentService.Saving += SaveAndPublishEvents.ConvertTextPageToLandingPage;
        }
    }
}
