using CleanCodeTest.Aplication.Mappers;
using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using CleanCodeTest.Domain.UseCases;
using CleanCodeTest.Infrastucture.Adapters;
using CleanCodeTest.Infrastucture.Repositories.Crm;
using CleanCodeTest.Infrastucture.Services;
using Microsoft.Xrm.Sdk;
using System;
using System.Net.Http;

namespace CleanCodeTest
{
    public class Plugin : IPlugin
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


            Entity comic = (Entity)context.InputParameters["Target"];

            string timeStamp = "1";
            string token = "5a40d446e4b3103af8981e918ceca113";
            string hash = "6c6387edab1c14f20c07e639e51c46d8";
            
            MarvelApiService marvelApiService = new MarvelApiService(timeStamp,hash, token, new HttpClient());
            ComicApiMarvelDTO comicApiMarvelDTO = marvelApiService.getComicByID(comic.Attributes["gap_marvelapiid"].ToString());

            gap_Comic data = new gap_ComicsComicApiMarvelMappper().Map(comicApiMarvelDTO);

            new ImportDataComic(new ComicCrmRepository(service)).invoke(data,comic);
        }
    }
}
