using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Newtonsoft.Json;
using System.Activities;
using System.Net.Http;



namespace Bootcamp23
{
    public class GetLinksWorkflow : CodeActivity
    {
        [Input("gap_marvelapiid")]
        public InArgument<int> marvelApiId { get; set; }

        [Output("crmItemUrl")]
        public OutArgument<string> crmItemUrl { get; set; }
        [Output("marvelApiUrl")]
        public OutArgument<string> marvelApiUrl { get; set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            string crmBaseUrl = "https://innovarguillem.crm4.dynamics.com/main.aspx";
            string itemCondition = $"?etn={context.PrimaryEntityName}&id={{{context.PrimaryEntityId}}}&pagetype=entityrecord";
            crmItemUrl.Set(executionContext, crmBaseUrl + itemCondition);

            string timeStamp = "1";
            string apiKey = "5a40d446e4b3103af8981e918ceca113";
            string hash = "6c6387edab1c14f20c07e639e51c46d8";
            string comicId = marvelApiId.Get(executionContext).ToString();
            string marvelApiBaseUrl = "https://gateway.marvel.com:443/v1/public";
            string endpoint = $"/comics/{comicId}";
            string url = $"{marvelApiBaseUrl}{endpoint}?ts={timeStamp}&apikey={apiKey}&hash={hash}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            string comicUrl = comicId;

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();

                string json = response.Content.ReadAsStringAsync().Result;

                // Parse the JSON response
                dynamic jsonResponse = JsonConvert.DeserializeObject(json);
                comicUrl = jsonResponse.data.results[0].urls[0].url;
            }

            marvelApiUrl.Set(executionContext, comicUrl);

        }
    }
}
