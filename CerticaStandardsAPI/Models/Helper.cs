using System;
using System.Net;
using System.IO;
using CerticaStandardsAPI.Models.Common;
using System.Collections.Generic;
using System.Linq;
using StandardsApiData.Common;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace CerticaStandardsAPI.Models
{
    public class Helper
    {
        public string ReturnRelevantJsonOutput(UriBuilder requestBuilder, string guid = null)
        {
            var request = WebRequest.Create(requestBuilder.Uri);
            string response = string.Empty;
            WebResponse webResponse = request.GetResponse();

            using (Stream webStream = webResponse.GetResponseStream())
            {
                if (webStream != null)
                {
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        response = responseReader.ReadToEnd();
                    }
                }
            }
            return response;
        }

        protected internal UriBuilder BuildApiUrl(string facet, string signature, long expires, Authentication authentication, string guidString = null, string app = null, bool paging = false, string next = null)
        {
            ProviderSettings providerSettings = new ProviderSettings();
            string url = providerSettings.getConfigValue("API_URL");
            string queryString = string.Empty;
            UriBuilder requestBuilder = new UriBuilder(url);
            queryString = FormQueryString(facet, providerSettings, guidString, app, paging, next, url);

            requestBuilder.Query = string.Format(queryString +
            "&partner.id={0}&auth.signature={1}&auth.expires={2}&user.id={3}",
             WebUtility.UrlEncode(authentication.partnerID),
             WebUtility.UrlEncode(signature),
             expires,
             WebUtility.UrlEncode(authentication.userId.ToString())
           );

            return requestBuilder;
        }

        protected string FormQueryString(string facet, ProviderSettings providerSettings, string guidString = null, string app = null, bool paging = false, string next = null, string url = null)
        {
            string queryString = string.Empty;

            if (facet == "document.publication.regions" || string.IsNullOrEmpty(facet))
            {
                queryString = providerSettings.getConfigValue("COUNTRY_QUERY");
            }
            else
            {
                if (facet == "document.publication.authorities")
                {
                    queryString = providerSettings.getConfigValue("AUTHORITIES_QUERY");
                }
                else if (facet == "document.publication")
                {
                    queryString = providerSettings.getConfigValue("PUBLICATION_QUERY");
                }
                else if (facet == "document")
                {
                    queryString = providerSettings.getConfigValue("DOCUMENT_QUERY");
                }
                else if (facet == "section")
                {
                    queryString = providerSettings.getConfigValue("SECTION_QUERY");
                }
                else if (facet == "standardsetsummary")
                {
                    queryString = providerSettings.getConfigValue("SUMMARY_QUERY");
                }
                else if (facet == "disciplines.subjects")
                {
                    queryString = providerSettings.getConfigValue("SUBJECT_QUERY");
                }
                else if (facet == "grid")
                {
                    queryString = providerSettings.getConfigValue("GRID_QUERY");
                }
                else if (facet == "standardset")
                {

                    if (!string.IsNullOrEmpty(app))
                    {
                        if (app == "webcms")
                            queryString = providerSettings.getConfigValue("STANDARD_QUERY_WEBCMS");
                    }
                    else
                        queryString = providerSettings.getConfigValue("STANDARD_QUERY");

                    //if paging then append offset value
                    if (paging)
                    {
                        queryString = next.Replace(url + "?", "").Replace("&user.id=+", "");
                    }
                }
                else
                {
                    queryString = string.Empty;
                    guidString = null;
                }

                if (queryString.Contains("@Guid"))
                {
                    guidString = '\'' + guidString + '\'';
                    queryString = queryString.Replace("@Guid", guidString);
                }
            }
            return queryString;
        }

        protected internal IEnumerable<FinalData> StripParentJsonForRiversideRelevantData(string facet, string guidString)
        {
            string response = AuthenticateAndGetResponse(facet, guidString);
            GeneralJson general = new GeneralJson();
            general = JsonConvert.DeserializeObject<GeneralJson>(response);

            //retrieve the description and guid for facets from the facet details
            IEnumerable<FinalData> result = general.meta.facets
                .Where(x => x.facet == facet)
                 .SelectMany(x => x.details
                 .Select(y => new FinalData
                 {
                     descr = y.data.descr,
                     guid = y.data.guid,
                     adoptYear = y.data.adopt_year,
                     code = y.data.code
                 }));
            return result;
        }

        protected internal DataTable ConvertToDataTable(List<LevelDict> standardSetData)
        {
            DataTable standardTable = new DataTable();
            try
            {
                standardTable.TableName = "StandardSetTable";

                foreach (PropertyInfo property in standardSetData[3].GetType().GetProperties())
                {

                    standardTable.Columns.Add(new DataColumn(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
                }
                foreach (var standard in standardSetData)
                {
                    DataRow newRow = standardTable.NewRow();
                    foreach (PropertyInfo property in standard.GetType().GetProperties())
                    {
                        newRow[property.Name] = standard.GetType().GetProperty(property.Name).GetValue(standard, null);
                    }
                    standardTable.Rows.Add(newRow);
                }

                return standardTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected internal IEnumerable<SummaryData> StripParentJsonForSummaryData(string facet, string guidString)
        {
            StandardSetFinal standardSetFinal = new StandardSetFinal();
            standardSetFinal=StripStandardSetJsonForRiversideRelevantData(facet, guidString, null);
            SummaryData summaryData = new SummaryData();

            IEnumerable<SummaryData> result = standardSetFinal.data.Select
                                              (x => new SummaryData
                                              {
                                                  documentGuid = x.attributes.document.guid,
                                                  subject = x.attributes.document.disciplines.primary_subject.descr,
                                                  subjectCode = x.attributes.document.disciplines.primary_subject.code,
                                                  stateDocument = x.attributes.document.descr,
                                                  document = x.attributes.document.publication.descr,
                                                  publicationGuid = x.attributes.document.publication.guid,                                               
                                                  stateDocumentYear = x.attributes.document.adopt_year
                                              })
                                              .GroupBy(o => new { o.subjectCode, o.subject})
                                              .Select(o => o.FirstOrDefault());

            return result;
        }

        protected internal IEnumerable<Regions.FinalData> GetRegions(string facet, string guidString)
        {
            string response = AuthenticateAndGetResponse(facet, guidString);
            Regions.RootObject general = new Regions.RootObject();
            general = JsonConvert.DeserializeObject<Regions.RootObject>(response);

            //  retrieve the description and guid for region
            IEnumerable<Regions.FinalData> result = general.meta.facets
                .Where(x => x.facet == facet)
                  .SelectMany(y => y.details
                  .Where(z => z.data.type == "country")
                 .Select(z => new Regions.FinalData
                 {
                     descr = z.data.descr,
                     guid = z.data.guid
                 }));
            return result;


        }
        protected internal string AuthenticateAndGetResponse(string facet, string guidString, string app = null, bool paging = false, string next = null)
        {
            ProviderAuthentication providerAuthentication = new ProviderAuthentication();
            UriBuilder requestBuilder = providerAuthentication.AuthenticateProvider(facet, guidString, app, paging, next);
            string response = ReturnRelevantJsonOutput(requestBuilder, guidString);
            return response;
        }

        protected internal StandardSetFinal StripStandardSetJsonForRiversideRelevantData(string facet, string guidString, string app = null)
        {
            StandardSetFinal standardSetFinal = new StandardSetFinal();
            StandardSetRootObject general = new StandardSetRootObject();
            int ctr = 0;
            general = FormGeneralOutput(general, facet, guidString, app);
            standardSetFinal.data = general.data;
            string next = general.links.next;

            //do not paginate for the values displayed in the subject drop down and the summary grid
            while (!string.IsNullOrEmpty(next) && facet != "disciplines.subjects" && facet !="standardsetsummary")
            {
                general = AppendJsonData(general, facet, guidString, app, standardSetFinal);
                standardSetFinal.data.AddRange(general.data);
                next = general.links.next;
                ctr++;
            }
            return standardSetFinal;
        }

        protected internal StandardSetRootObject AppendJsonData(StandardSetRootObject general, string facet, string guidString, string app, StandardSetFinal standardSetFinal)
        {
            string next = general.links.next;
            general = FormGeneralOutput(general, facet, guidString, app, next, true);
            return general;
        }
        protected internal StandardSetRootObject FormGeneralOutput(StandardSetRootObject general, string facet, string guidString, string app = null, string next = null, bool paging = false)
        {
            string response = AuthenticateAndGetResponse(facet, guidString, app, paging, next);
            general = JsonConvert.DeserializeObject<StandardSetRootObject>(response);
            return general;
        }
        protected internal StandardSetWebCMS StripStandardSetJsonForWebCMSRelevantData(string facet, string guidString, string app = null)
        {
            StandardSetFinal standardSetFinal = StripStandardSetJsonForRiversideRelevantData(facet, guidString, app);
            StandardSetWebCMS standardSetWebCMS = ReturnWebCMSData(standardSetFinal.data, guidString);

            InsertCollectionIntoWebCMS(standardSetWebCMS);
            return standardSetWebCMS;
        }


        protected internal void InsertCollectionIntoWebCMS(StandardSetWebCMS standardSetWebCMS)
        {
            try
            {
                DataTable standardTable = ConvertToDataTable(standardSetWebCMS.webCMSLevels);
                
                AppConfiguration appConfiguration = new AppConfiguration();
                using (SqlConnection sqlcon = new SqlConnection(appConfiguration.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("InsertStandardSetIntoWebCMS", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter standardSetName = new SqlParameter();
                        standardSetName.Direction = ParameterDirection.Input;
                        standardSetName.ParameterName = "@StandardSetName";
                        standardSetName.SqlDbType = SqlDbType.VarChar;
                        standardSetName.Value = standardSetWebCMS.standardSetName;
                        cmd.Parameters.Add(standardSetName);

                        SqlParameter state = new SqlParameter();
                        state.Direction = ParameterDirection.Input;
                        state.ParameterName = "@State";
                        state.SqlDbType = SqlDbType.VarChar;
                        state.Value = standardSetWebCMS.state;
                        cmd.Parameters.Add(state);

                        SqlParameter tableParameter = new SqlParameter();
                        tableParameter.Direction = ParameterDirection.Input;
                        tableParameter.ParameterName = "@StandardSetTable";
                        tableParameter.SqlDbType = SqlDbType.Structured;
                        tableParameter.Value = standardTable;
                        cmd.Parameters.Add(tableParameter);
                        cmd.CommandTimeout = 0;

                        sqlcon.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            { 
            }
        } 
        protected internal string GetState(Datum item)
        {             
            string state = Convert.ToString(item.attributes.document.publication.regions.Where(x => x.type.ToLower() == "other" || x.type.ToLower() == "state")
                               .Select(x => x.code).FirstOrDefault());
            
            return state;
        }
        protected internal string FormStandardSetName(string state, string level1, string level2,string level3)
        {
            string standardSetName = state + " - " + level1 + " " + level2 + " "+ level3;
            if(standardSetName.Contains("CC - NGA Center/CCSSO Common Core State Standards"))
            {
                standardSetName = standardSetName.Replace("CC - NGA Center/CCSSO Common Core State Standards", "National Common Core State Standards");
            }
            return standardSetName;
        }

        protected internal LevelDict AssignLevelInformation(int levelNo, Datum item, Guid standardGuid, string standardText,string content)
        {
            LevelDict levelDict = new LevelDict();
            levelDict.levelNumber = levelNo;
            levelDict.standardGuid = standardGuid;
            levelDict.standardText = standardText;
            KeyValuePair<int?, int?> contentPair=GetContent(content);
            levelDict.contentId = contentPair.Key;
            levelDict.contentFieldId = contentPair.Value;

            //common values
            levelDict.state = GetState(item);
            if (levelNo > 3)
            {
                levelDict.standardNumber = item.attributes.number.enhanced;
                levelDict.source = item.attributes.document.publication.descr;
                levelDict.subject = item.attributes.section.descr;
                levelDict.fromGrade = item.attributes.education_levels.grades.OrderBy(x => x.seq).Select(f => f.code).First();
                levelDict.toGrade = item.attributes.education_levels.grades.OrderByDescending(x => x.seq).Select(t => t.code).First();                
            }

            return levelDict;
        }

        protected internal List<LevelDict> GetLevel1(Datum item, List<LevelDict> level1, LevelDict levelDict,string content)
        {
           
            levelDict = AssignLevelInformation(1, item, Guid.Parse(item.attributes.document.publication.authorities[0].guid), item.attributes.document.publication.authorities[0].descr, content);

            if (!CheckIfValueExists(level1,levelDict.standardGuid))           
            {
                level1.Add(levelDict);
            }
            return level1;
        }

        protected internal List<LevelDict> GetLevel4(Datum item, List<LevelDict> level4, LevelDict levelDict, string content)
        {
            string fromGrade = item.attributes.education_levels.grades.OrderBy(x => x.seq).Select(f => f.code).First();
            string toGrade = item.attributes.education_levels.grades.OrderByDescending(x => x.seq).Select(t => t.code).First();
            string standardText;

            standardText = fromGrade;
            
            if((fromGrade.ToLower().Trim()=="9") || (fromGrade.ToLower().Trim() == "10") || (fromGrade.ToLower().Trim() == "11") || (fromGrade.ToLower().Trim() == "12"))
            {
                if (fromGrade.ToLower().Trim() != toGrade.ToLower().Trim())               
                {
                    standardText = fromGrade + "-" + toGrade;
                }
            }
            else
            {
                standardText = fromGrade;
            }

            levelDict = AssignLevelInformation(4, item,Guid.Empty,standardText, content);

            if (!CheckIfGradeExists(level4,standardText))
            {
                if ((!levelDict.subject.ToLower().Contains("introduction")) && (!levelDict.subject.ToLower().Contains("practices")))
                    level4.Add(levelDict);
            }
            return level4;
        }
        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        protected internal bool CheckIfValueExists(List<LevelDict> levelList,Guid guidToFind)
        {
            bool hasGuid=  (from level in levelList
             where level.standardGuid == guidToFind
             select level.standardGuid).Any();
            return hasGuid;
        }

        protected internal bool CheckIfGradeExists(List<LevelDict> levelList, string GradeToFind)
        {
            bool hasGrade = (from level in levelList
                            where level.standardText == GradeToFind
                            select level.standardText).Any();
            return hasGrade;
        }
        protected internal List<LevelDict> GetLevel2(Datum item, List<LevelDict> level2, LevelDict levelDict,string content)
        {
            levelDict = AssignLevelInformation(2, item, Guid.Parse(item.attributes.document.publication.guid), item.attributes.document.publication.descr, content);
            if (!CheckIfValueExists(level2, levelDict.standardGuid))
            {
                level2.Add(levelDict);
            }
            return level2;
        }

        protected internal List<LevelDict> GetLevel3(Datum item, List<LevelDict> level3, LevelDict levelDict,string content)
        {            
            levelDict = AssignLevelInformation(3, item, Guid.Parse(item.attributes.section.disciplines.primary_subject.guid), item.attributes.section.disciplines.primary_subject.code + " (" + item.attributes.section.adopt_year + ")", content);
            if (!CheckIfValueExists(level3, levelDict.standardGuid))
            {
                level3.Add(levelDict);
            }
            return level3;
        }


        protected internal List<LevelDict> GetLevel(Datum item, List<LevelDict> levelList, LevelDict levelDict,string content)
        {
            //Get level1
            levelList = GetLevel1(item, levelList, levelDict,content);
            //Get level2
            levelList = GetLevel2(item, levelList, levelDict, content);
            //Get level3
            levelList = GetLevel3(item, levelList, levelDict, content);
            //Get level4
            levelList = GetLevel4(item, levelList, levelDict, content);

            if (item.attributes.level >=1 && item.attributes.level<=4)
            {
                //add levels 5-10 and their relevant key value pairs
                string enhanced = GetEnhanced(item.attributes.number.enhanced,item);
                string standardText;                              
                standardText = enhanced + " " + item.attributes.statement.descr;
                
                levelDict = AssignLevelInformation(item.attributes.level + 4, item, Guid.Parse(item.attributes.guid??""),standardText, content);

                //Ignore introduction and practices
                if((!levelDict.subject.ToLower().Contains("introduction")) && (!levelDict.subject.ToLower().Contains("practices")))
                    levelList.Add(levelDict);              
            }
           
            return levelList;
        }
                
       protected internal string GetEnhanced(string enhanced,Datum item)
        {
            if (!string.IsNullOrEmpty(enhanced))
            {
                int index = GetNthIndex(enhanced, '.', 3) + 1;
                if (index > 0)
                {
                    enhanced = enhanced.Substring(index, enhanced.Length - index);
                }

                string fromGrade = item.attributes.education_levels.grades.OrderBy(x => x.seq).Select(f => f.code).FirstOrDefault();
                string toGrade = item.attributes.education_levels.grades.OrderByDescending(x => x.seq).Select(t => t.code).FirstOrDefault();

                //for highschool
                if ((fromGrade.ToLower().Trim() == "9") || (fromGrade.ToLower().Trim() == "10") || (fromGrade.ToLower().Trim() == "11") || (fromGrade.ToLower().Trim() == "12"))
                {
                    if (enhanced.StartsWith("HS"))
                    {
                       enhanced= enhanced.Replace("HS", "");
                    }
                }
            }
            
            return enhanced;
        }
        protected internal StandardSetWebCMS ReturnWebCMSData(List<Datum> result, string guidString)
        {
            StandardSetWebCMS standardSetWebCMS = new StandardSetWebCMS();            
            string standardSetName=string.Empty,content = string.Empty;
            List<LevelDict> levelList = new List<LevelDict>();            
            LevelDict levelDict = new LevelDict();
            string state = string.Empty;
            foreach (var item in result)
            {
                //Get values for all levels from 1-9
                content = item.attributes.document.descr;
                levelList =GetLevel(item, levelList, levelDict,content);
                state= levelList.Select(x => x.state).First();
                
                if (string.IsNullOrEmpty(standardSetName))
                {
                    string level1=levelList.Where(x => x.levelNumber == 1).Select(y => y.standardText).First();
                    string level2 = levelList.Where(x => x.levelNumber == 2).Select(y => y.standardText).First();
                    string level3 = levelList.Where(x => x.levelNumber == 3).Select(y => y.standardText).First();
                    standardSetName = FormStandardSetName(state, level1,level2,level3);                   
                }               
            }
         
            standardSetWebCMS.standardSetName = standardSetName;

            if (state == "CC")
                state = "NT";
            
            standardSetWebCMS.state = state;        
            standardSetWebCMS.webCMSLevels.AddRange(levelList);           
            return standardSetWebCMS;
        }

       protected internal KeyValuePair<int?,int?> GetContent(string content)
        {
            int? contentId = null, contentFieldId = null;
            if (!string.IsNullOrEmpty(content))
            {
                if (content.Equals("English Language Arts"))
                {
                    content = "LANGUAGE ARTS";
                }
                
                switch (content.ToUpper())
                {
                    case "LANGUAGE ARTS":
                        contentFieldId = 18;
                        contentId = 1;
                        break;
                    case "MATH":
                        contentFieldId = 19;
                        contentId = 2;
                        break;
                    case "SOCIAL STUDIES":
                        contentFieldId = 20;
                        contentId = 4;
                        break;
                    case "SCIENCE":
                        contentFieldId = 21;
                        contentId = 3;
                        break;
                    case "READING":
                        contentFieldId = 22;
                        contentId = 5;
                        break;
                    case "N/A":
                        contentFieldId = 23;
                        contentId = 6;
                        break;
                    case "ARTS":
                        contentFieldId = 24;
                        contentId = 12;
                        break;
                    default:
                        contentFieldId = 23;
                        contentId = 6;
                        break;
                }
            }
            return new KeyValuePair<int?, int?>(contentId, contentFieldId);
        }
    }
}