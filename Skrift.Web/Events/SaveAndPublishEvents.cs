using System.Linq;
using Skrift.Web.Converters;
using Skrift.Web.Extensions;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Skrift.Web.Events
{
    public class SaveAndPublishEvents
    {
        public static void ConvertTextPageToLandingPage(IContentService sender, SaveEventArgs<IContent> args)
        {
            foreach (var node in args.SavedEntities)
            {
                if (node.HasPublishedVersion)
                {
                    // This only works if the page has been published before...
                    var previousPublishedVersion = sender.GetPublishedVersion(node.Id);

                    // Check if this page was a Text Page and is turning into a Landing Page
                    if (previousPublishedVersion.ContentType.Alias == "textPage" && node.ContentType.Alias == "landingPage")
                    {
                        // Get the main Rich Text Editor content from the previous version
                        var rteContent = previousPublishedVersion.GetSafeString("bodyText");

                        // Make sure the new node actually has the Grid content
                        if (!string.IsNullOrEmpty(rteContent) && node.HasProperty("gridContent"))
                        {
                            // Find the specific grid content property
                            var property = node.Properties.First(x => x.Alias == "gridContent");

                            // Populate the grid content with the text from the Rich Text Editor
                            property.Value = DocTypeChangePropertyConverters.ConvertRteToGridContent(rteContent);
                        }

                        // Make sure the original version had a banner
                        if (previousPublishedVersion.WillWork("banner"))
                        {
                            // Get the specific banner property
                            var property = node.Properties.First(x => x.Alias == "banner");

                            // Convert the picked media into a Text Over Image banner with the image and headline pre-populated
                            property.Value = DocTypeChangePropertyConverters.ConvertBannerToTextOverImage(previousPublishedVersion, previousPublishedVersion.GetValue<string>("banner"));
                        }
                    }
                }
            }
        }
    }
}
