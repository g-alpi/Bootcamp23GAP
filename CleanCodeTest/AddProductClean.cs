using CleanCodeTest.Aplication.Helper;
using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.UseCases;
using CleanCodeTest.Infrastucture.Repositories.Crm;
using Microsoft.Xrm.Sdk;
using System;

namespace CleanCodeTest
{
    public class AddProductClean : IPlugin
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

            var customFunctions = new CustomFunctions();
            Entity comic = (Entity)context.InputParameters["Target"];

            Product product = new Product
            {
                name = comic.Attributes["gap_title"].ToString(),
                productnumber = customFunctions.GetEntityAttribute(service,
                                "gap_comictest",
                                new string[] { "gap_marvelapiid" },
                                "gap_title",
                                comic.Attributes["gap_title"].ToString()),
                defaultuomscheduleid = customFunctions.GetEntityReference(service,
                                "uomschedule",
                                new string[] { "uomscheduleid" },
                                "name",
                                "Comic"),
                defaultuomid = customFunctions.GetEntityReference(service,
                                "uom",
                                new string[] { "uomid" },
                                "name",
                                "Unit"),
                quantitydecimal = 2,
                price = new Money(10),
                description = customFunctions.GetEntityAttribute(service,
                                "gap_comictest",
                                new string[] { "gap_descriptiontest" },
                                "gap_title",
                                comic.Attributes["gap_title"].ToString()),
                optionSetValue = new OptionSetValue(0),
            };


            new AddProduct(new ProductCrmRepository(service)).invoke(product);
            
        }
    }
}
