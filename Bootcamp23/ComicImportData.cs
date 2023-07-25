using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.ServiceModel;

namespace MyPlugins
{
    public class ComicImportData : IPlugin
    {
        public async void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    string timeStamp = "1";
                    string apiKey = "5a40d446e4b3103af8981e918ceca113";
                    string hash = "6c6387edab1c14f20c07e639e51c46d8";
                    string comicId = entity.Attributes["gap_marvelapiid"].ToString();
                    string baseUrl = "https://gateway.marvel.com:443/v1/public";
                    string endpoint = $"/comics/{comicId}";
                    string url = $"{baseUrl}{endpoint}?ts={timeStamp}&apikey={apiKey}&hash={hash}";

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;
                        response.EnsureSuccessStatusCode();

                        string json = response.Content.ReadAsStringAsync().Result;

                        // Parse the JSON response
                        dynamic jsonResponse = JsonConvert.DeserializeObject(json);
                        dynamic results = jsonResponse.data.results[0];
                        string description = results.description;
                        string title = results.title;

                        //string cover = (string)results["images"][0]["path"];
                        string cover = results.thumbnail.path +"."+ results.thumbnail.extension;

                        if (description == null || description == "")
                        {
                            JArray textObjectsArray = results.textObjects;
                            if (textObjectsArray.Count > 0)
                            {
                                description = results.textObjects[0].text;
                            }
                            else
                            {
                                if (results.variantDescription == "")
                                {
                                    description = "No description available";
                                }
                                else
                                {
                                    description = results.variantDescription;
                                }
                            }
                        }
                        // Update the entity with the description
                        entity["gap_description"] = description;
                        entity["gap_title"] = title;
                        entity["gap_cover"] = cover;
                        service.Update(entity);
                    }

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }

    }
}
