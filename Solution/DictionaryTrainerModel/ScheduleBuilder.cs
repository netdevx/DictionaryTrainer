using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class ScheduleBuilder: IScheduleBuilder
    {
        private readonly IEnumerable<double> forgettingCurveInHours = new[] { 0, 0.5, 24, 24 * 7 * 2, 24 * 7 * 4 * 2 };

        public IEnumerable<ScheduleItem> GetSchedule(DateTime fromDate)
        {
            return forgettingCurveInHours.Select(interval => new ScheduleItem() { DateToShow = fromDate.AddHours(interval), IsShown = false, IsSuccessful = false });
        }
    }
}
