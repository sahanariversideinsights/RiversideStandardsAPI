using System;
using System.Collections.Generic;

namespace CerticaStandardsAPI.Models.Common
{
    public class Regions
    {
        public class RegionJson
        {          
            public Links links { get; set; }
            public Meta meta { get; set; }
        }
        public class Links
        {
            public string self { get; set; }
            public string next { get; set; }
            public string last { get; set; }
        }

        public class Data
        {
            public string id { get; set; }
            public string type { get; set; }
        }

        public class Parent
        {
            public Data data { get; set; }
        }

        public class Relationships
        {
            public Parent parent { get; set; }
        }

        public class Grade
        {
            public string guid { get; set; }
            public string code { get; set; }
            public int seq { get; set; }
            public string descr { get; set; }
        }

        public class EducationLevels
        {
            public List<Grade> grades { get; set; }
        }

        public class Statement
        {
            public List<object> addendums { get; set; }
            public string descr { get; set; }
        }

        public class Number
        {
            public string enhanced { get; set; }
            public string raw { get; set; }
        }

        public class PrimarySubject
        {
            public string descr { get; set; }
            public string guid { get; set; }
            public string code { get; set; }
        }

        public class Disciplines
        {
            public PrimarySubject primary_subject { get; set; }
        }

        public class Section
        {
            public string adopt_year { get; set; }
            public string descr { get; set; }
            public int seq { get; set; }
            public Disciplines disciplines { get; set; }
            public string guid { get; set; }
        }

        public class Subject
        {
            public string descr { get; set; }
            public string code { get; set; }
            public string guid { get; set; }
        }

        public class Disciplines2
        {
            public List<object> strands { get; set; }
            public List<Subject> subjects { get; set; }
        }

        public class PrimarySubject2
        {
            public string descr { get; set; }
            public string code { get; set; }
            public string guid { get; set; }
        }

        public class Disciplines3
        {
            public PrimarySubject2 primary_subject { get; set; }
        }

        public class Region
        {
            public string code { get; set; }
            public string guid { get; set; }
            public string type { get; set; }
            public string descr { get; set; }
        }

        public class Authority
        {
            public string guid { get; set; }
            public object acronym { get; set; }
            public string descr { get; set; }
        }

        public class Publication
        {
            public List<Region> regions { get; set; }
            public string guid { get; set; }
            public List<Authority> authorities { get; set; }
            public string acronym { get; set; }
            public string descr { get; set; }
        }

        public class Document
        {
            public string guid { get; set; }
            public Disciplines3 disciplines { get; set; }
            public Publication publication { get; set; }
            public string descr { get; set; }
            public string adopt_year { get; set; }
        }

        public class Attributes
        {
            public EducationLevels education_levels { get; set; }
            public Statement statement { get; set; }
            public Number number { get; set; }
            public int level { get; set; }
            public string label { get; set; }
            public Section section { get; set; }
            public Disciplines2 disciplines { get; set; }
            public List<object> utilizations { get; set; }
            public string status { get; set; }
            public int seq { get; set; }
            public string guid { get; set; }
            public Document document { get; set; }
        }

        public class Datum
        {
            public Relationships relationships { get; set; }
            public Attributes attributes { get; set; }
            public string id { get; set; }
            public string type { get; set; }
        }

        public class RegionData
        {
            public string guid { get; set; }
            public string code { get; set; }
            public string descr { get; set; }
            public string type { get; set; }
        }

        public class Detail
        {
            public int count { get; set; }
            public RegionData data { get; set; }
        }

        public class Facet
        {
            public int count { get; set; }
            public string facet { get; set; }
            public List<Detail> details { get; set; }
        }

        public class Meta
        {
            public int limit { get; set; }
            public List<Facet> facets { get; set; }
            public int offset { get; set; }
            public int took { get; set; }
            public int count { get; set; }
        }

        public class RootObject
        {
            public Links links { get; set; }
            public List<Datum> data { get; set; }
            public Meta meta { get; set; }
        }

        public class FinalData
        {
            public string descr { get; set; }
            public string guid { get; set; }
        }
    }
}