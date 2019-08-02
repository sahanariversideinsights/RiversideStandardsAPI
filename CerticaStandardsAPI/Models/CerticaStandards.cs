using System;
using CerticaStandardsAPI.Models.Common;
using System.Collections.Generic;

namespace CerticaStandardsAPI.Models
{
    public class CerticaStandards
    {
        Helper helper = new Helper();
        IEnumerable<FinalData> result = null;

        public IEnumerable<Regions.FinalData> GetAllRegions()
        {
            IEnumerable<Regions.FinalData> result = helper.GetRegions("document.publication.regions", string.Empty);
            return result;
        }
        public IEnumerable<FinalData> GetAllAuthorities(string guidString)
        {
            result= helper.StripParentJsonForRiversideRelevantData("document.publication.authorities",guidString);
            return result;           
        }

        public IEnumerable<FinalData> GetAllPublications(string guidString)
        {
            result = helper.StripParentJsonForRiversideRelevantData("document.publication", guidString);
            return result;
        }


        public IEnumerable<FinalData> GetAllDocuments(string guidString)
        {
            result = helper.StripParentJsonForRiversideRelevantData("document", guidString);
            return result;
        }

        public IEnumerable<FinalData> GetAllSections(string guidString)
        {
            result = helper.StripParentJsonForRiversideRelevantData("section", guidString);
            return result;
        }

        public StandardSetFinal GetSpecificStandard(string guidString)
        {
            StandardSetFinal result = helper.StripStandardSetJsonForRiversideRelevantData("standardset", guidString);           
            return result;
        }

        public IEnumerable<SummaryData> GetSpecificStandardSummary(string guidString,string app)
        {
            if (app =="webcms")
            {
                IEnumerable<SummaryData> result = helper.StripParentJsonForSummaryData("standardsetsummary", guidString);
                return result;
            }
            else
                return null;
         
        }

        public IEnumerable<FinalData> GetAllSubjects(string guidString)
        {
            result = helper.StripParentJsonForRiversideRelevantData("disciplines.subjects", guidString);
            return result;
        }


        public dynamic GetAppSpecificStandard(string guidString, string app)
        {
            if (app == "webcms")
            {               
                StandardSetWebCMS result = helper.StripStandardSetJsonForWebCMSRelevantData("standardset", guidString, app);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}