using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClassifier
{
    class Category
    {
        private Dictionary<string, int> _parameters;

        public Category(string name, int trainingSetCount, Dictionary<string, int> parameters)
        {
            Name = name;
            TrainingSetCount = trainingSetCount;
            _parameters = parameters;
        }

        public string Name { get; }
        public int TrainingSetCount { get; set; }
        public IReadOnlyDictionary<string, int> Parameters => _parameters;

        public void AddParameterValue(string nameParameter, int value)
        {
            _parameters[nameParameter] = _parameters[nameParameter] + value;
        }
    }
}
