using CerticaStandardsAPI.Controllers;
using CerticaStandardsAPI.Models.Common;
using Newtonsoft.Json;

namespace RiversideStandardsAPI.Models
{
    public class StandardsHelper
    {
        protected internal string ApiCallRedirector(string redirectedFrom, string provider,string guidString=null,string app=null)
        {
            string responseJson=string.Empty;
            if (provider == "Certica")
            {
                CerticaStandardsController certicaController = new CerticaStandardsController();
                if (redirectedFrom == "regions")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetAllRegions());
                }
                else if (redirectedFrom == "authorities")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetAllAuthorities(guidString));
                }          
                else if (redirectedFrom == "publications")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetAllPublications(guidString));
                }
                else if (redirectedFrom == "sections")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetAllSections(guidString));
                }
                else if (redirectedFrom == "documents")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetAllDocuments(guidString));
                }                
                else if (redirectedFrom == "standards")
                {
                    responseJson = JsonConvert.SerializeObject(certicaController.GetSpecificStandard(guidString));
                }
                else if (redirectedFrom == "appstandards")
                {
                  responseJson = JsonConvert.SerializeObject(certicaController.GetAppSpecificStandard(guidString,app));
                }
            }
            return responseJson;            
        }  
    }
}