using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skrift.Web.Extensions
{
    public static class ContentExtensions
    {
        #region IPublishedContent Extensions

        public static string GetSafeString(this IPublishedContent model, string propertyName)
        {
            return model.WillWork(propertyName) ? model.GetPropertyValue<string>(propertyName) : string.Empty;
        }

        public static bool WillWork(this IPublishedContent model, string propertyName)
        {
            return model.HasProperty(propertyName) && model.HasValue(propertyName);
        }

        #endregion

        #region IContent Extensions
        
        public static string GetSafeString(this IContent content, string propertyName, string defaultText = null)
        {
            return content.WillWork(propertyName) ? content.GetValue<string>(propertyName) : defaultText;
        }

        public static string GetSafeString(this IContent content, string propertyName)
        {
            return content.GetSafeString(propertyName, null);
        }

        public static bool WillWork(this IContent content, string propertyName)
        {
            return content.HasProperty(propertyName) && content.GetValue(propertyName) != null;
        }

        #endregion
    }
}
