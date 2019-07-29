
using RiversideStandardsAPI.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RiversideStandardsAPI.Controllers
{

    public class StandardsController : Controller
    {
        StandardsHelper helper = new StandardsHelper();
        private Dictionary<string, string> _urlKeyValues = new Dictionary<string, string>();

        private void UriHandler(bool withGuid, bool app = false)
        {

            string guidString = null;
            string provider = HttpContext.Request.Query["Provider"].ToString();

            if (withGuid)
            {
                guidString = HttpContext.Request.Query["Guid"].ToString();
            }
            if (app)
            {
                string application = HttpContext.Request.Query["App"].ToString();
                _urlKeyValues.Add("application", application);
            }
            _urlKeyValues.Add("provider", provider);
            _urlKeyValues.Add("guidString", guidString);
        }

        //sample call looks like http://localhost:80/api/Regions?Provider=Certica
        [HttpGet]
        [Route("api/Regions/{Provider=provider}")]
        public IActionResult GetAllRegions()
        {
            UriHandler(false);
            string responseJson = helper.ApiCallRedirector("regions", _urlKeyValues["provider"]);
            return Ok(JToken.Parse(responseJson)); ;

        }

        //sample call looks like http://localhost:80/api/Authorities?Provider=Certica
        [HttpGet]
        [Route("api/Authorities/{Provider=provider}")]  //state
        public IActionResult GetAllAuthorities()
        {
            UriHandler(true);
            string responseJson = helper.ApiCallRedirector("authorities", _urlKeyValues["provider"], _urlKeyValues["guidString"]);
            return Ok(JToken.Parse(responseJson)); ;
        }

        //Sample Call looks like : http://localhost:80/api/Publication?Provider=Certica&Guid=9127FF50-F1B9-11E5-862E-0938DC287387
        [HttpGet]
        [Route("api/Publication/{Provider=provider}/{Guid=guid}")]
        public IActionResult GetAllPublications()   // document
        {
            UriHandler(true);
            string responseJson = helper.ApiCallRedirector("publications", _urlKeyValues["provider"], _urlKeyValues["guidString"]);
            return Ok(JToken.Parse(responseJson)); ;
        }

        //Sample Call looks like : http://localhost:80/api/Document?Provider=Certica&Guid=4D7B5584-9C82-11E7-8A3F-4EABBF03DF2F
        [HttpGet]
        [Route("api/Document/{Provider=provider}/{Guid=guid}")]
        public IActionResult GetAllDocuments()  //subject or state document. state document contains the year
        {
            UriHandler(true);
            string responseJson = helper.ApiCallRedirector("documents", _urlKeyValues["provider"], _urlKeyValues["guidString"]);
            return Ok(JToken.Parse(responseJson)); ;
        }

        //Sample Call looks like : http://localhost:80/api/Section?Provider=Certica&Guid=49C1ACA6-9CC6-11E7-8E55-D1F6CCC8CA83
        [HttpGet]
        [Route("api/Section/{Provider=provider}/{Guid=guid}")]
        public IActionResult GetAllSections()
        {
            UriHandler(true);
            string responseJson = helper.ApiCallRedirector("sections", _urlKeyValues["provider"], _urlKeyValues["guidString"]);
            return Ok(JToken.Parse(responseJson)); ;
        }

        //Sample Call looks like : http://localhost:80/api/Standard?Provider=Certica&Guid=A3C7B2DE-A3B2-11E7-A191-28F8CCC8CA83
        [HttpGet]
        [Route("api/Standard/{Provider=provider}/{Guid=guid}")]
        public IActionResult GetSpecificStandard()
        {
            UriHandler(true);
            string responseJson = helper.ApiCallRedirector("standards", _urlKeyValues["provider"], _urlKeyValues["guidString"]);
            return Ok(JToken.Parse(responseJson)); ;
        }

        //Sample Call looks like : https://localhost:80/api/StandardApp?Provider=Certica&App=webcms&Guid=2F154714-22F6-11E6-9485-3FA829C466BA
        [HttpGet]
        [Route("api/StandardApp/{Provider=provider}/{App=app}/{Guid=guid}")]
        public IActionResult GetAppSpecificStandard()
        {
            UriHandler(true, true);
            string responseJson = helper.ApiCallRedirector("appstandards", _urlKeyValues["provider"], _urlKeyValues["guidString"], _urlKeyValues["application"]);
            return Ok(JToken.Parse(responseJson)); ;
        }
    }
}
