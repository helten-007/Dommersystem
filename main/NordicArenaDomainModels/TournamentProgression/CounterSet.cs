using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NordicArenaDomainModels.TournamentProgression
{
    /// <summary>
    /// Represents a set of chained counters with arbitrary boundaries.
    /// Eg. from "1:1:1" to "5:5:5"
    /// </summary>
    public class CounterSet
    {
        public List<Counter> _list = new List<Counter>();

        public List<Counter> Counters { get { return _list; } }

        /// <summary>
        /// Adds a counter at the end of the set
        /// </summary>
        /// <param name="counter"></param>
        public void AddCounter(Counter counter) 
        {
            if (_list.Count > 0) counter.PrevNode = _list[_list.Count - 1];
            _list.Add(counter);
        }

        public void Inc() 
        {
            _list[_list.Count - 1].Inc();
        }

        public void Dec()
        {
            _list[_list.Count - 1].Dec();     
        }

        public bool IsAtMin() 
        {
            return _list.All(p => p.IsAtMin());
        }

        public bool IsAtMax() 
        {
            return _list.All(p => p.IsAtMax());
        }

        public override string ToString()
        {
            if (_list.Count == 0) return String.Empty;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var elem in _list) 
            {
                sb.Append(elem.Value);
                if (++i != _list.Count) sb.Append(":");
            }
            return sb.ToString();
        }

        public void SetValuesFromString(String s)
        {
            var stringValues = s.Split(':');
            int[] iVals = new int[stringValues.Length];
            for (int i = 0; i < stringValues.Length; i++) 
            {
                iVals[i] = Convert.ToInt32(stringValues[i]);
            }
            SetValue(iVals);
        }

        public void SetValue(params int[] val) {
            for (int i = 0; i < val.Length; i++)
                _list[i].Value = val[i];
        }
    }

    /// <summary>
    /// A bounded counter
    /// </summary>
    public class Counter
    {
        private int _value;
        private int _min;
        private int _max;
        private Func<int> _getMaxFunc;
        public Counter PrevNode { get; set; }

        public int Value
        {
            get { return _value; }
            set
            {
                if (value > GetMax()) throw new InvalidOperationException(String.Format("{0} is above max value {1}", value, GetMax()));
                if (value < GetMin()) throw new InvalidOperationException(String.Format("{0} below min value {1}", value, GetMin()));
                _value = value;
            }
        }
 
        /// <summary>
        /// Initializes with a counter starting at 1 ending at max
        /// </summary>
        public Counter(int max) 
        {
            _value = 1;
            _min = 1;
            _max = max;
        }
        
        /// <summary>
        /// Initializes with a counter starting at min ending at max
        /// </summary>
        public Counter(int min, int max) 
        {
            _value = min;
            _min = min;
            _max = max;
        }
        
        /// <summary>
        /// Initializes with a counter starting, ending at value given by a function.
        /// </summary>
        public Counter(int min, Func<int> getMax) 
        {
            _min = min;
            _getMaxFunc = getMax;
        }

        /// <summary>
        /// Initializes with a counter starting at 1
        /// </summary>
        /// <param name="getMax"></param>
        public Counter(Func<int> getMax) 
        {
            _min = 1;
            _value = 1;
            _getMaxFunc = getMax;
        }

        public bool IsAtMax() 
        {
            return _value == GetMax();
        }

        public bool IsAtMin() 
        {
            return _value == GetMin();
        }

        public int GetMin() {
            return _min;
        }

        public int GetMax() {
            if (_getMaxFunc != null) return _getMaxFunc.Invoke();
            return _max;

        }

        public void Inc() {
            if (!IsAtMax()) _value++;
            else if (PrevNode != null) {
                PrevNode.Inc();
                _value = GetMin();
            }
            else throw new InvalidOperationException("At max");
        }

        public void Dec() {
            if (!IsAtMin()) _value--;
            else if (PrevNode != null)
            {
                PrevNode.Dec();
                _value = GetMax();
            }
            else throw new InvalidOperationException("At min");
        }
    }
}