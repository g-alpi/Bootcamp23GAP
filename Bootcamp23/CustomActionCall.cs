using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace Bootcamp23
{
    public class CustomActionCall : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
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
            CustomFunctions customFunctions = new CustomFunctions();

            

            try
            {
                string myInputParam =(string) context.InputParameters["MyInputParam"];
                bool addComicConfirmation = (bool)context.InputParameters["AddComicConfirmation"];
                bool isAdmin = (bool) context.InputParameters["IsAdmin"];
                bool comicExistFlag = false;
                bool entityExist = customFunctions.EntityExist(service, "gap_comic", new string[] { "gap_title" }, "gap_marvelapiid", myInputParam.ToString());
                

                if (entityExist)
                {
                    Dictionary<string,object> comic = customFunctions.GetEntityAttributes(service, "gap_comic", new string[] { "gap_title","gap_cover" }, "gap_marvelapiid", myInputParam.ToString());
                    context.OutputParameters["MyOutputParam"] = comic["gap_title"];
                    context.OutputParameters["CoverURL"] = comic["gap_cover"];
                    comicExistFlag = true;
                }
                else
                {
                    if (addComicConfirmation == true && isAdmin == true)
                    {

                        Entity newComic = new Entity("gap_comic");

                        newComic["gap_marvelapiid"] = int.Parse(myInputParam);
                        newComic["gap_title"] = "";

                        Guid newComicId = service.Create(newComic);
                        comicExistFlag = true;
                    }
                    else if (addComicConfirmation == true && isAdmin == false)
                    {
                        
                        context.OutputParameters["MyOutputParam"] = $"Solo los administrador pueden crear comics!!";
                    }
                    else{
                        context.OutputParameters["MyOutputParam"] = $"El comic {myInputParam} NO existe :c";
                    }

                }
                context.OutputParameters["ComicExist"] = comicExistFlag;

            }

            catch (FaultException<OrganizationServiceFault> ex)
            {
                tracingService.Trace("MyPlugin: {0}", ex.ToString());
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
