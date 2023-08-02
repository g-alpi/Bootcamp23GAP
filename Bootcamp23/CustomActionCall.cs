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

            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

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
