using CleanCodeTest.Domain.Entities;
using CleanCodeTest.Domain.Repositories;
using CleanCodeTest.Aplication.Helper.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CleanCodeTest.Infrastucture.Repositories.Crm
{
    internal class ComicCrmRepository : IComicRepository
    {
        IOrganizationService _crmServcie;
        public ComicCrmRepository(IOrganizationService crmService) {
            _crmServcie = crmService;
        }

        public string GetComicAttribute(string entityName, string[] column, string conditionColum, string conditionValue)
        {
            QueryExpression query = new QueryExpression(entityName);
            query.ColumnSet = new ColumnSet(column);
            query.Criteria.AddCondition(conditionColum, ConditionOperator.Equal, conditionValue);

            EntityCollection results = _crmServcie.RetrieveMultiple(query);
            if (results.Entities.Count > 0)
            {
                Entity entity = results.Entities[0];
                string attributeValue = entity.Attributes[column[0]].ToString();
                return attributeValue;
            }
            else
            {
                throw new InvalidPluginExecutionException($"Attribute '{column[0]}' has not been found.");
            }
        }

        public void UpdateComic(Gap_Comic data, Entity comic)
        {  
            comic.Attributes["gap_covertest"] = data.cover;
            comic.Attributes["gap_descriptiontest"] = data.description;
            comic.Attributes["gap_title"] = data.title;
            _crmServcie.Update(comic);
        }
    }
}
