using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace needle {
    public class Strand{
        public string ProjectName;
        public string FullPath;

        public Strand(string strandToEvaluate){
            if(strandToEvaluate.StartsWith("$")){
                ProjectName=strandToEvaluate.Substring(2, strandToEvaluate.IndexOf(""));
            }
        }
    }
}
