using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monitor;
using System.Collections.Generic;

namespace MonitorTests
{
    [TestClass]
    public class TimeListTests
    {
        [TestMethod]
        public void TimeList_CorrectCount()
        {
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>();

            Assert.AreEqual(list.Count, 0);

            list.Add(0);
            Assert.AreEqual(list.Count, 1);

            list.Add(0);
            Assert.AreEqual(list.Count, 1);

            list.Add(1);
            Assert.AreEqual(list.Count, 2);

            list.Add(1);
            Assert.AreEqual(list.Count, 2);

            list.Add(2);
            Assert.AreEqual(list.Count, 3);
        }

        [TestMethod]
        public void TimeList_CorrectValues()
        {
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>();

            list.Add(11);
            list.Add(11);
            list.Add(22);
            list.Add(22);
            list.Add(22);
            list.Add(33);
            list.Add(44);
            list.Add(55);
            list.Add(55);
            list.Add(55);
            list.Add(66);
            list.Add(66);
            list.Add(11);

            Assert.AreEqual(list[0], 11);
            Assert.AreEqual(list[1], 22);
            Assert.AreEqual(list[2], 33);
            Assert.AreEqual(list[3], 44);
            Assert.AreEqual(list[4], 55);
            Assert.AreEqual(list[5], 66);
            Assert.AreEqual(list[6], 11);
        }


        [TestMethod]
        public void TimeList_CorrectTimeSimple()
        {

            DateTime now = new DateTime(2000, 3, 3, 3, 3, 3);
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(now);            

            list.Add(11, now);
            Assert.AreEqual(list.SecondsAt(0), 0);

            list.Add(22, now.AddSeconds(3));
            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 0);

            list.Add(33, now.AddSeconds(7));
            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 0);

            list.Add(44, now.AddSeconds(17));
            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 10);
            Assert.AreEqual(list.SecondsAt(3), 0);

            list.Add(55, now.AddSeconds(18));
            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 10);
            Assert.AreEqual(list.SecondsAt(3), 1);
            Assert.AreEqual(list.SecondsAt(4), 0);

            list.Add(66, now.AddSeconds(21));
            Assert.AreEqual(list.SecondsAt(0), 3);
            Assert.AreEqual(list.SecondsAt(1), 4);
            Assert.AreEqual(list.SecondsAt(2), 10);
            Assert.AreEqual(list.SecondsAt(3), 1);
            Assert.AreEqual(list.SecondsAt(4), 3);
            Assert.AreEqual(list.SecondsAt(5), 0);

            Assert.AreEqual(list.Count, 6);
        }

        [TestMethod]
        public void TimeList_TimeUpdate()
        {
            DateTime now = new DateTime(2000, 3, 3, 3, 3, 3);
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(now);

            list.Add(11, now);
            Assert.AreEqual(list.SecondsAt(0), 0);

            list.Add(11, now.AddSeconds(5));
            Assert.AreEqual(list.SecondsAt(0), 5);

            list.Add(11, now.AddSeconds(6));
            Assert.AreEqual(list.SecondsAt(0), 6);

            list.Add(11, now.AddSeconds(9));
            Assert.AreEqual(list.SecondsAt(0), 9);

            list.Add(22, now.AddSeconds(15));
            Assert.AreEqual(list.SecondsAt(0), 15);
            Assert.AreEqual(list.SecondsAt(1), 0);

            list.Add(22, now.AddSeconds(16));
            Assert.AreEqual(list.SecondsAt(0), 15);
            Assert.AreEqual(list.SecondsAt(1), 1);

            list.Add(22, now.AddSeconds(20));
            Assert.AreEqual(list.SecondsAt(0), 15);
            Assert.AreEqual(list.SecondsAt(1), 5);

            list.Add(33, now.AddSeconds(26));
            Assert.AreEqual(list.SecondsAt(0), 15);
            Assert.AreEqual(list.SecondsAt(1), 11);
            Assert.AreEqual(list.SecondsAt(2), 0);
        }

        [TestMethod]
        public void TimeList_TimeUpdateStart_NotFromZero()
        {
            DateTime now = new DateTime(2000, 3, 3, 3, 3, 3);
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(now);

            list.Add(11, now.AddSeconds(10));
            Assert.AreEqual(list.SecondsAt(0), 0);

            list.Add(22, now.AddSeconds(14));
            Assert.AreEqual(list.SecondsAt(0), 4);
            Assert.AreEqual(list.SecondsAt(1), 0);

            list.Add(22, now.AddSeconds(15));
            Assert.AreEqual(list.SecondsAt(0), 4);
            Assert.AreEqual(list.SecondsAt(1), 1);

            list.Add(33, now.AddSeconds(17));
            Assert.AreEqual(list.SecondsAt(0), 4);
            Assert.AreEqual(list.SecondsAt(1), 3);
            Assert.AreEqual(list.SecondsAt(2), 0);
        }



        [TestMethod]
        public void TimeList_GetSecondsList()
        {
            DateTime now = new DateTime(2000, 3, 3, 3, 3, 3);
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(now);

            list.Add(11, now.AddSeconds(5));
            list.Add(22, now.AddSeconds(6));
            list.Add(33, now.AddSeconds(11));           

            List<long> seconds = list.GetSecondsList(now.AddSeconds(17));

            Assert.AreEqual(seconds[0], 1);
            Assert.AreEqual(seconds[1], 5);
            Assert.AreEqual(seconds[2], 6);
        }

        [TestMethod]
        public void TimeList_GetSecondsList2()
        {
            DateTime now = new DateTime(2000, 3, 3, 3, 3, 3);
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>(now);

            list.Add(11, now.AddSeconds(5));
            list.Add(22, now.AddSeconds(6));
            list.Add(33, now.AddSeconds(11));
            list.Add(33, now.AddSeconds(16));

            List<long> seconds = list.GetSecondsList(now.AddSeconds(17));

            Assert.AreEqual(seconds[0], 1);
            Assert.AreEqual(seconds[1], 5);
            Assert.AreEqual(seconds[2], 6);
        }


        [TestMethod]
        public void TimeList_AdjacentDifferent()
        {
            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>();

            Random r = new Random();

            for (int i = 0; i < 10000; i++)
            {
                int n = r.Next(1, 10);
                list.Add(n);
            }

            Assert.IsTrue(list.ValidateAdjacentDifferent());
        }

        [TestMethod]
        public void TimeList_AddBoolean()
        {

            TimeList<Nullable<int>> list = new TimeList<Nullable<int>>();

            Assert.IsTrue(list.Add(11));
            Assert.IsFalse(list.Add(11));
            Assert.IsTrue(list.Add(22));
            Assert.IsFalse(list.Add(22));
            Assert.IsFalse(list.Add(22));
            Assert.IsTrue(list.Add(33));
            Assert.IsTrue(list.Add(44));
            Assert.IsTrue(list.Add(55));
            Assert.IsFalse(list.Add(55));

        }

    }
}

