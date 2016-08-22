using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegExPG
{
    public class FindInFiles
    {
        private List<string> _filesToSearch;
        private string _searchString;
        private enum SearchMode
        {
            Text,
            Regexp
        } 

        public FindInFiles()
        {
            
        }

        public List<string> FilesToSearch
        {
            get
            {
                return _filesToSearch;
            }

            set
            {
                _filesToSearch = value;
            }
        }
    }
}
