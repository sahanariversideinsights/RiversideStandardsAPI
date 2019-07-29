using System;
using System.Collections.Generic;

namespace CerticaStandardsAPI.Models.Common
{

    public class StandardSetWebCMS
    {
        private List<LevelDict> _levelDict;
        public string standardSetName { get; set; }
        public string state { get; set; }           
        public List<LevelDict> webCMSLevels
        {
            get
            {
                if (_levelDict == null)
                    _levelDict = new List<LevelDict>();
                return _levelDict;
            }
            set { _levelDict = value; }
        }
    }
    
    public class LevelDict
    {
        public int levelNumber { get; set; }
        public string standardNumber { get; set; }
        public string state { get; set; }
        public string source { get; set; }
        public string subject { get; set; }
        public string fromGrade { get; set; }
        public string toGrade { get; set; }
        public Guid standardGuid { get; set; }
        public string standardText { get; set; }
        public int? contentId { get; set; }
        public int? contentFieldId { get; set; }
    }
}
