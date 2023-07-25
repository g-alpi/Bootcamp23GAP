using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace Bootcamp23
{
    public class AddProdcut : IPlugin
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

            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity comic = (Entity)context.InputParameters["Target"];

                try
                {
                    CustomFunctions customFunctions = new CustomFunctions();

                    Entity product = new Entity("product");

                    product["name"] = comic.Attributes["gap_title"];
                    product["productnumber"] = customFunctions.GetEntityAttribute(service,"gap_comic",new string[] { "gap_marvelapiid" },"gap_title", comic.Attributes["gap_title"].ToString());
                    product["defaultuomscheduleid"] = customFunctions.GetEntityReference(service, "uomschedule", new string[] { "uomscheduleid" }, "name", "Comic");
                    product["defaultuomid"] = customFunctions.GetEntityReference(service, "uom", new string[] { "uomid" }, "name", "Unit"); 
                    product["quantitydecimal"] = 2;
                    product["price"] = new Money(10);
                    product["pricelevelid"] = customFunctions.GetEntityReference(service, "pricelevel", new string[] { "pricelevelid" }, "name", "EU Price List Test");
                    product["description"] = customFunctions.GetEntityAttribute(service, "gap_comic", new string[] { "gap_description" }, "gap_title", comic.Attributes["gap_title"].ToString());

                    Guid createdProductId = service.Create(product);

                    Entity updatedProduct = new Entity("product", createdProductId);
                    updatedProduct["statecode"] = new OptionSetValue(0) ;
                    service.Update(updatedProduct);

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
