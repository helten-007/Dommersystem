using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.TournamentProgression;
using NordicArenaTournament.Common;

namespace Tests.Common
{
    [TestClass]
    public class CounterSetTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetValue_IllegalValue()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(4));
            counterSet.AddCounter(new Counter(4));
            counterSet.SetValue(5, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetValue_IllegalValue2()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(4));
            counterSet.AddCounter(new Counter(4));
            counterSet.SetValue(3, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetValue_IndexError()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(4));
            counterSet.SetValue(3, 3);
        }


        [TestMethod]
        public void SetValue()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(4));
            counterSet.AddCounter(new Counter(4));

            counterSet.SetValue(2, 3);

            Assert.AreEqual("2:3", counterSet.ToString());
        }

        [TestMethod]
        public void Inc_11initialized()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2));
            counterSet.AddCounter(new Counter(2));

            Assert.AreEqual("1:1", counterSet.ToString());
        }

        [TestMethod]
        public void Inc_22nitialized()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2,3));
            counterSet.AddCounter(new Counter(3,7));

            Assert.AreEqual("2:3", counterSet.ToString());
        }

        [TestMethod]
        public void Inc_11to12()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2));
            counterSet.AddCounter(new Counter(2));

            counterSet.Inc();

            Assert.AreEqual("1:2", counterSet.ToString());
        }

        [TestMethod]
        public void Inc_12to21()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2));
            counterSet.AddCounter(new Counter(2));

            counterSet.SetValue(1, 2);
            counterSet.Inc();

            Assert.AreEqual("2:1", counterSet.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Inc_22atMax_InvalidOp()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2));
            counterSet.AddCounter(new Counter(2));

            counterSet.SetValue(2, 2);
            counterSet.Inc();
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Inc_11atMin_InvalidOp()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2));
            counterSet.AddCounter(new Counter(2));

            counterSet.SetValue(1, 1);
            counterSet.Dec();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Inc_22atMin_InvalidOp()
        {
            var counterSet = new CounterSet();
            counterSet.AddCounter(new Counter(2, 4));
            counterSet.AddCounter(new Counter(2, 4));

            counterSet.SetValue(2, 2);

            counterSet.Dec();
        }
        
        [TestMethod]
        public void CounterSet_Functionbased()
        {
            var counterSet = new CounterSet();
            var counter1 = new Counter(2);
            var counter2 = new Counter(() => { return OneHigherThanValue(counter1); });
            counterSet.AddCounter(counter1);
            counterSet.AddCounter(counter2);
            
            int i = 0;
            List<String> perms = GetAllPermutations(counterSet);
            Assert.AreEqual("1:1", perms[0]);
            Assert.AreEqual("1:2", perms[1]);
            Assert.AreEqual("2:1", perms[2]);
            Assert.AreEqual("2:2", perms[3]);
            Assert.AreEqual("2:3", perms[4]);
            Assert.AreEqual(5, perms.Count);
        }

        private List<string> GetAllPermutations(CounterSet counterSet)
        {
            var res = new List<String>();
            do
            {
                res.Add(counterSet.ToString());
                counterSet.Inc();
            } while (!counterSet.IsAtMax());
            res.Add(counterSet.ToString());
            return res;
        }

        private int OneHigherThanValue(Counter counter)
        {
            return counter.Value + 1;
        }
    }
}
