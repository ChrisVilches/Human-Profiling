using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor;
using System.Diagnostics;

namespace MonitorTests
{

    [TestClass]
    public class TimeListTests
    {

        [TestMethod]
        public void TimeList_CorrectAddOrder()
        {
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(20);

            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);

            Assert.AreEqual(list[0], 0);
            Assert.AreEqual(list[1], 1);
            Assert.AreEqual(list[2], 2);
            Assert.AreEqual(list[3], 3);
            Assert.AreEqual(list.Count, 4);
        }


        [TestMethod]
        public void TimeList_CorrectElapsedTimes()
        {            
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(20);

            DateTime now = DateTime.Now;

            list.Add(0, now);
            list.Add(1, now.AddSeconds(3));
            list.Add(2, now.AddSeconds(7));
            list.Add(3, now.AddSeconds(11));
            list.Add(4, now.AddSeconds(21));

            Assert.AreEqual(list[0], 0);
            Assert.AreEqual(list[1], 1);
            Assert.AreEqual(list[2], 2);
            Assert.AreEqual(list[3], 3);

            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 4);
            Assert.AreEqual(list.SecondsAt(3), 10);
            Assert.AreEqual(list.Count, 4);
        }

        [TestMethod]
        public void TimeList_BackToBeginning()
        {
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(5);

            DateTime now = DateTime.Now;

            list.Add(0, now);
            list.Add(1, now.AddSeconds(3));
            list.Add(2, now.AddSeconds(7));
            list.Add(3, now.AddSeconds(11));
            list.Add(4, now.AddSeconds(21));

            Assert.IsFalse(list.ShouldFlushBeforeAdding());
            list.Add(5, now.AddSeconds(27));
            Assert.IsTrue(list.ShouldFlushBeforeAdding());
            
            Assert.AreEqual(list[0], 0);
            Assert.AreEqual(list[1], 1);
            Assert.AreEqual(list[2], 2);
            Assert.AreEqual(list[3], 3);
            Assert.AreEqual(list[4], 4);

            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 4);
            Assert.AreEqual(list.SecondsAt(3), 10);
            Assert.AreEqual(list.SecondsAt(4), 6);

            /* List is full */
            Assert.AreEqual(list.Count, 5);

            list.Clear();

            list.Add(100, now.AddSeconds(28));
            Assert.AreEqual(list.Count, 1); // [5]

            list.Add(200, now.AddSeconds(31));
            Assert.AreEqual(list.Count, 2); // [5, 100]

            list.Add(300, now.AddSeconds(37));
            Assert.AreEqual(list.Count, 3); // [5, 100, 200]


            Assert.AreEqual(list[0], 5);
            Assert.AreEqual(list[1], 100);
            Assert.AreEqual(list[2], 200);

            Assert.AreEqual(list.SecondsAt(0), 1);
            Assert.AreEqual(list.SecondsAt(1), 3);
            Assert.AreEqual(list.SecondsAt(2), 6);
        }

    }
}
