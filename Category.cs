using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class Category
    {
        private Dictionary<string, int> _pharses;
        private int _totalTrainingSetCount;

        public Category(Dictionary<string, int> pharses, string name)
        {
            _pharses = pharses;
            Name = name;
        }

        public Category(string name)
        {
            _pharses = new Dictionary<string, int>();
            Name = name;
        }

        public string Name { get; }
        public int TotalTrainingSetCount 
        {
            get 
            {
                var sum = _pharses.Values.Sum();
                _totalTrainingSetCount = sum > 0 ? sum : 1;

                return _totalTrainingSetCount;
            }
        }
        public Dictionary<string, int> Pharses => _pharses;
    }
}
