using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public struct Condition
    {
        public string TargetName;
        public object TargetValue;

        public Condition(string targetName, object targetValue)
        {
            TargetName = targetName;
            TargetValue = targetName;
        }

        public bool IsEqual(object value)
        {
            return TargetValue.Equals(value);
        }
    }


    public class Conditions : IEnumerable
    {
        private List<Condition> conditions = new List<Condition>();

        public Conditions(params Condition[] Conditions)
        {
            conditions = Conditions.ToList();
        }

        public void AddCondition(Condition condition)
        {
            conditions.Add(condition);
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)conditions).GetEnumerator();
        }

        public void RemoveCondition(Condition condition)
        {
            conditions.Remove(condition);
        }

        public List<Condition> Element => conditions;
    }
}
