using System;
using System.Collections.Generic;
 
namespace CerticaStandardsAPI.Models.Common
{
    public class StandardSetLinks
    {
        public string next { get; set; }
        public string self { get; set; }
        public string last { get; set; }
    }

    public class StandardSetData
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Parent
    {
        public StandardSetData data { get; set; }
    }

    public class Relationships
    {
        public Parent parent { get; set; }
    }

    public class Strand
    {
        public string descr { get; set; }
        public string guid { get; set; }
    }

    public class Subject
    {
        public string descr { get; set; }
        public string code { get; set; }
        public string guid { get; set; }
    }

    public class Disciplines
    {
        public List<Strand> strands { get; set; }
        public List<Subject> subjects { get; set; }
    }

    public class PrimarySubject
    {
        public string code { get; set; }
        public string guid { get; set; }
        public string descr { get; set; }
    }

    public class SectionDiscipline
    {
        public PrimarySubject primary_subject { get; set; }
    }

    public class Section
    {
        public string guid { get; set; }
        public SectionDiscipline disciplines { get; set; }
        public int seq { get; set; }
        public string descr { get; set; }
        public string adopt_year { get; set; }
    }

    public class Authority
    {
        public string guid { get; set; }
        public string acronym { get; set; }
        public string descr { get; set; }
    }

    public class Region
    {
        public string code { get; set; }
        public string guid { get; set; }
        public string type { get; set; }
        public string descr { get; set; }
    }

    public class Publication
    {
        public string descr { get; set; }
        public string acronym { get; set; }
        public List<Authority> authorities { get; set; }
        public List<Region> regions { get; set; }
        public string guid { get; set; }
    }

    public class DocumentSubject
    {
        public string descr { get; set; }
        public string code { get; set; }
        public string guid { get; set; }
    }

    public class DocumentDisciplines
    {
        public DocumentSubject primary_subject { get; set; }
    }

    public class Document
    {
        public Publication publication { get; set; }
        public DocumentDisciplines disciplines { get; set; }
        public string descr { get; set; }
        public string adopt_year { get; set; }
        public string guid { get; set; }
    }

    public class Number
    {
        public string enhanced { get; set; }
        public string raw { get; set; }
    }

    public class Statement
    {
        public List<object> addendums { get; set; }
        public string descr { get; set; }
    }

    public class Grade
    {
        public string guid { get; set; }
        public string code { get; set; }
        public string descr { get; set; }
        public int seq { get; set; }
    }

    public class EducationLevels
    {
        public List<Grade> grades { get; set; }
    }

    public class Attributes
    {
        public int seq { get; set; }
        public string status { get; set; }
        public List<object> utilizations { get; set; }
        public Disciplines disciplines { get; set; }
        public Section section { get; set; }
        public Document document { get; set; }
        public string guid { get; set; }
        public Number number { get; set; }
        public Statement statement { get; set; }
        public EducationLevels education_levels { get; set; }
        public string label { get; set; }
        public int level { get; set; }
    }

    public class Datum
    {
        public Relationships relationships { get; set; }
        public Attributes attributes { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }

    public class StandardSetFacet
    {
        public string facet { get; set; }
        public int count { get; set; }
    }

    public class StandardSetMeta
    {
        public int count { get; set; }
        public int offset { get; set; }
        public int took { get; set; }
        public List<Facet> facets { get; set; }
        public int limit { get; set; }
    }

    public class StandardSetRootObject
    {
        public StandardSetLinks links { get; set; }
        public List<Datum> data { get; set; }
        public StandardSetMeta meta { get; set; }
    }

    public class StandardSetFinal
    {
        public List<Datum> data { get; set; }
    }

    public class SummaryData
    {
        public string subjectCode{ get; set; }
        public string subject { get; set; }
        public string documentGuid { get; set; }
        public string document { get; set; }
        public string publicationGuid { get; set; }
        public string stateDocument { get; set; }
        public string stateDocumentYear { get; set; }

    }
}