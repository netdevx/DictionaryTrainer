using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class ScheduleBuilderTests
    {
        [TestMethod]
        public void Build()
        {
            var forgettingCurveInHours = new[] { 0, 0.5, 24, 24 * 7 * 2, 24 * 7 * 4 * 2 };
            var startDate = DateTime.Now;

            var builder = new ScheduleBuilder();
            var actual = builder.GetSchedule(startDate).ToList();

            for (int i = 0; i <= actual.Count - 1; i++)
            {
                var expected = new ScheduleItem() { DateToShow = startDate.AddHours(forgettingCurveInHours[i]), IsShown = false, IsSuccessful = false };
                Assert.AreEqual(expected.DateToShow, actual[i].DateToShow);
                Assert.AreEqual(expected.IsShown, actual[i].IsShown);
                Assert.AreEqual(expected.IsSuccessful, actual[i].IsSuccessful);
            }
        }
    }
}
