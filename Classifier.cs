using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class Classifier
    {
        private List<Category> _categories;
        private List<string> _nameParameters;

        public Classifier(Dictionary<string, int[]> allValuesParameters, Dictionary<string, int> categories)
        {
            _categories = new List<Category>();
            _nameParameters = allValuesParameters.Keys.ToList();

            int categoryNum = 0;
            foreach (var c in categories) 
            {
                if (categoryNum == categories.Count) break;

                string name = c.Key;
                int totalTrainingSet = c.Value;
                Dictionary<string, int> parameters = new Dictionary<string, int>();
                foreach (var valuesParameter in allValuesParameters)
                {
                    if (valuesParameter.Value.Length != categories.Count) throw new ArgumentException("Несовпадение количества категорий с количеством значений параметров");
                    parameters.Add(valuesParameter.Key, valuesParameter.Value[categoryNum]);
                }
                Category category = new Category(name, totalTrainingSet, parameters);
                _categories.Add(category);
                categoryNum++;
            }

        }

        public IReadOnlyCollection<Category> Categories => _categories;

        private bool CheckParameters(IEnumerable<string> parameters) 
        {
            foreach (var p in parameters)
            {
                if (!_nameParameters.Contains(p))
                    return false;
            }
            return true;
        }
        
        public Dictionary<string, double> BayesianClassify(IEnumerable<string> parameters) 
        {
            if(!CheckParameters(parameters))
                throw new ArgumentException("В категориях классификации отстутствуют параметры с заданным именем");

            var scores = new Dictionary<string, double>();
            int allSetsCount = _categories.Sum(c => c.TrainingSetCount);

            foreach (Category c in _categories)
            {
                double result = c.TrainingSetCount / allSetsCount; //Нахождение априорной вероятности
                foreach (var parameter in parameters)
                {
                    //Нахождение апостериорной вероятности
                    int count = c.Parameters[parameter];
                    result = result * count / c.TrainingSetCount;
                }
                scores.Add(c.Name, result);
            }
            
            return scores;
        }

        public Dictionary<string, double> LinearClassify(IEnumerable<string> parameters)
        {
            if (!CheckParameters(parameters))
                throw new ArgumentException("В категориях классификации отстутствуют параметры с заданным именем");

            var scores = new Dictionary<string, double>();
            int countAllSets = _categories.Sum(c => c.TrainingSetCount);

            foreach (Category c in _categories)
            {
                double result = 0;
                foreach (var parameter in parameters)
                {
                    int count = c.Parameters[parameter];
                    result += count / c.TrainingSetCount;
                }
                scores.Add(c.Name, result);
            }

            return scores;
        }

        public void AddCategory(Category category) 
        {
            if(!_nameParameters.SequenceEqual(category.Parameters.Keys))
                throw new ArgumentException("В добавляемой категории отсутствуют требуемые параметры для классификации");

            _categories.Add(category);
        }

        public Category GetCategory(string name) 
        {
            return _categories.Where(c => c.Name == name).First();
        }

        public bool RemoveCategory(string name) 
        {
            Category category = _categories.Where(c => c.Name == name).First();
            return _categories.Remove(category);
        }

        public void Train(string name, Dictionary<string, int> trainingSetParameters, int trainingSetCount) 
        {
            if (!CheckParameters(trainingSetParameters.Keys))
                throw new ArgumentException("В категориях классификации отстутствуют параметры с заданным именем");

            Category category = GetCategory(name);
            if (category != null) 
            {
                category.TrainingSetCount += trainingSetCount;
                foreach (var set in trainingSetParameters)
                {
                    category.AddParameterValue(set.Key, set.Value);
                }
            }
        }
    }
}
