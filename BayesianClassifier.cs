using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class BayesianClassifier
    {
        private List<Category> _categories;

        public BayesianClassifier()
        {
            _categories = new List<Category>();
        }

        public Dictionary<string, double> Classify(IEnumerable<string> phrases) 
        {
            var scores = new Dictionary<string, double>();
            var words = new Category(phrases, "test");
            return null;
        }

        public void AddCategory(Category category) 
        {
            if (_categories.Contains(category) || _categories.Find(c => c.Name == category.Name) != null) return;

            _categories.Add(category);
        }

        public Category GetCategory(string nameCategory) 
        {
            return _categories.Find(c => c.Name == nameCategory);
        }

        public bool RemoveCategory(Category category) 
        {
            return _categories.Remove(category);
        }
    }
}
