using System;
using System.Web;
using Newtonsoft.Json;
using Skrift.Web.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skrift.Web.Converters
{
    public static class DocTypeChangePropertyConverters
    {
        /// <summary>
        /// Converts a media picker to a TextOverImage banner
        /// </summary>
        /// <param name="model">
        /// The previous version of <see cref="IContent"/> that is converting to the new version
        /// </param>
        /// <param name="id">
        /// The ID of the picked media in the banner
        /// </param>
        /// <returns>
        /// A Text Over Image Editor banner that has it's content pre-populated from the previously picked media
        /// </returns>
        public static string ConvertBannerToTextOverImage(IContent model, string id)
        {
            var uniqueId = id.Replace("umb://media/", string.Empty);

            IMedia content = UmbracoContext.Current.Application.Services.MediaService.GetById(Guid.Parse(uniqueId));
            var media = content == null ? null : UmbracoContext.Current.MediaCache.GetById(content.Id);

            if (media != null)
            {
                var bannerJson = @"{
                  ""headline"": """ + model.GetSafeString("headline", model.Name) + @""",
                  ""height"": ""short"",
                  ""link"": {
                    ""id"": 0,
                    ""name"": ""link"",
                    ""target"": ""_self"",
                    ""url"": """"
                  },
                  ""media"": {
                    ""id"": " + media.Id + @",
                    ""url"": """ + media.Url + @""",
                    ""width"": " + media.GetPropertyValue<int>("umbracoWidth") + @",
                    ""height"": " + media.GetPropertyValue<int>("umbracoHeight") + @",
                    ""altText"": """ + media.GetSafeString("altText") + @"""
                  },
                  ""subheadline"": ""Sub-Headline"",
                  ""position"": ""mc""
                }";

                return bannerJson;
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts content from a Rich Text Editor into a grid row pre-populated with the content
        /// </summary>
        /// <param name="rteValue">
        /// The original value from the Rich Text Editor
        /// </param>
        /// <returns>
        /// A JSON string of Umbraco Grid content
        /// </returns>
        public static string ConvertRteToGridContent(string rteValue)
        {
            if (!string.IsNullOrEmpty(rteValue))
            {
                var text = JsonConvert.SerializeObject(HttpUtility.HtmlDecode(rteValue));
                var gridJson = @"{
                  ""name"": ""Content Row"",
                  ""sections"": [
                    {
                      ""grid"": 12,
                      ""rows"": [
                        {
                          ""name"": ""Content"",
                          ""areas"": [
                            {
                              ""grid"": 12,
                              ""allowAll"": true,
                              ""hasConfig"": false,
                              ""controls"": [
                                {
                                  ""value"":" + text + @",
                                  ""editor"": {
                                    ""alias"": ""rte""
                                  },
                                  ""active"": false
                                }
                              ],
                              ""active"": false
                            }
                          ],
                          ""label"": ""Content"",
                          ""hasConfig"": false,
                          ""id"": """ + Guid.NewGuid() + @""",
                          ""active"": false
                        }
                      ]
                    }
                  ]
                }";

                return gridJson;
            }

            return string.Empty;
        }
    }
}
