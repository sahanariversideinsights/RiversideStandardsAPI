using System;
using System.Collections.Generic;

namespace CerticaStandardsAPI.Models.Common
{
    public class GeneralJson
    {
       public Links links { get; set; }
      public  Meta meta { get; set; }
    }
    public class Links
    {
        public string self { get; set; }
    }

    public class FinalData
    {
        public string descr { get; set; }
        public string guid { get; set; }
        public string adoptYear { get; set; }
    }

    public class Meta
    {
        public List<Facet> facets { get; set; }
        public int limit { get; set; }
        public int took { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
    }

    public class RootObject
    {
        public Links links { get; set; }
        public List<object> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Detail
    {
        public int count { get; set; }
        public Data data { get; set; }
    }

    public class Facet
    {
        public int count { get; set; }
        public string facet { get; set; }
        public List<Detail> details { get; set; }
    }

    public class Data
    {
        public string adopt_year { get; set; }
        public string acronym { get; set; }
        public string title { get; set; }
        public string guid { get; set; }
        public string descr { get; set; }
        public int seq { get; set; }
    }
    
}