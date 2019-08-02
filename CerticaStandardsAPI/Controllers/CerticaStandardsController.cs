using System.Web.Http;
using CerticaStandardsAPI.Models;
using System.Collections.Generic;
using CerticaStandardsAPI.Models.Common;

namespace CerticaStandardsAPI.Controllers
{
    public class CerticaStandardsController : ApiController
    {
        CerticaStandards certicaStandards = new CerticaStandards();

        [HttpGet]
        public IEnumerable<Regions.FinalData> GetAllRegions()
        {
            return certicaStandards.GetAllRegions();
        }

        [HttpGet]      
        public IEnumerable<FinalData> GetAllAuthorities(string guidString)
        {          
           return certicaStandards.GetAllAuthorities(guidString);            
        }

        [HttpGet]
        public IEnumerable<FinalData> GetAllPublications(string guidString)
        {           
            return certicaStandards.GetAllPublications(guidString);
        }

        [HttpGet]
        public IEnumerable<FinalData> GetAllDocuments(string guidString)
        {            
            return certicaStandards.GetAllDocuments(guidString);
        }

        [HttpGet]
        public IEnumerable<FinalData> GetAllSections(string guidString)
        {
            return certicaStandards.GetAllSections(guidString);
        }

        [HttpGet]
        public StandardSetFinal GetSpecificStandard(string guidString)
        {
            return certicaStandards.GetSpecificStandard(guidString); 
        }

        [HttpGet]
        public IEnumerable<SummaryData> GetSpecificStandardSummary(string guidString,string app)
        {
            return certicaStandards.GetSpecificStandardSummary(guidString,app);
        }
                
        [HttpGet]
        public dynamic GetAppSpecificStandard(string guidString, string app)
        {
            return certicaStandards.GetAppSpecificStandard(guidString, app);
        }

        [HttpGet]
        public IEnumerable<FinalData> GetAllSubjects(string guidString)
        {
            return certicaStandards.GetAllSubjects(guidString);
        }
    }
}
