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
            var forgettingCurveInHours = new[] { 0, 0.5, 24, 24*7*2, 24*7*4*2 };
            var startDate = DateTime.Now;

            var builder = new ScheduleBuilder();
            var actual = builder.GetSchedule(startDate).ToList();

            var ex = forgettingCurveInHours.Select(x => new ScheduleItem() { DateToShow = startDate.AddHours(x), IsShown = false, IsSuccessful = false }).ToList();
            MyAssertions.CollectionAssert(ex, actual, (e, a) => e.DateToShow == a.DateToShow && e.IsShown == a.IsShown && e.IsSuccessful == a.IsSuccessful);            
        }
    }
}
