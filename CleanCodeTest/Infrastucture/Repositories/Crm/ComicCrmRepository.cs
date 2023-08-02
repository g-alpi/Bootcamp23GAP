using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using Microsoft.Xrm.Sdk;

namespace CleanCodeTest.Infrastucture.Repositories.Crm
{
    internal class ComicCrmRepository : IComicRepository
    {
        IOrganizationService _crmServcie;
        public ComicCrmRepository(IOrganizationService crmService) {
            _crmServcie = crmService;
        }

        public void UpdateByMarvelApiId(gap_Comic data, Entity comic)
        {
            //QueryExpression query = new QueryExpression("gap_comictest");
            //query.ColumnSet = new ColumnSet("gap_comictestid");
            //query.Criteria.AddCondition("gap_marvelapiid", ConditionOperator.Equal, data.marvelApiId);
            //EntityCollection results = _crmServcie.RetrieveMultiple(query);

            //if (results.Entities.Count > 0)
            //{
            //    Entity comic = results.Entities[0];  
            //    comic.Attributes["gap_covertest"] = data.cover;
            //    comic.Attributes["gap_descriptiontest"] = data.description;
            //    comic.Attributes["gap_title"] = data.title;
            //    _crmServcie.Update(comic);
            //}
            //else
            //{
            //    throw new InvalidPluginExecutionException($"The comic could not be found.");
            //}
            
            comic.Attributes["gap_covertest"] = data.cover;
            comic.Attributes["gap_descriptiontest"] = data.description;
            comic.Attributes["gap_title"] = data.title;
            _crmServcie.Update(comic);


        }
    }
}
