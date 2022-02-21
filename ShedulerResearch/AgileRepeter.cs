using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShedulerResearch
{
    public class AgileRepeater
    {
        public RepeaterUnit Unit { get; set; }
        public RepeaterLimit Limit { get; set; }
        public RepeaterPattern Pattern { get; set; }

        public DateTime GetNextOccurrence(DateTime startTime)
        {
            return GetNextOccurrences(startTime).First();
        }

        public IEnumerable<DateTime> GetNextOccurrences(DateTime startTime)
        {
            DateTime position = startTime;
            switch (Unit)
            {
                case RepeaterUnit.Minute:
                    {
                        position = new DateTime(position.Year, position.Month, position.Day, position.Hour, position.Minute,
                            0);
                        if (position < startTime)
                        {
                            position = position.AddMinutes(1);
                        }
                        break;
                    }
            }

            switch (Unit)
            {
                case RepeaterUnit.Minute:
                    {
                        switch (Limit)
                        {
                            case RepeaterLimit.Hour:
                                {
                                    var limit = new DateTime(position.Year, position.Month, position.Day, position.Hour, 0, 0);
                                    var offset = Pattern.Offset;
                                    var runer = limit.AddMinutes(offset);
                                    limit = limit.AddHours(1);
                                    while (true)
                                    {
                                        foreach (var patternIndex in Pattern.Indexes)
                                        {
                                            var result = runer.AddMinutes(patternIndex);
                                            if (result>=startTime)
                                            {
                                                yield return result;
                                                startTime = result;
                                            }
                                        }

                                        if (Pattern.Period == null)
                                        {
                                            yield break;
                                        }
                                        runer = runer.AddMinutes(Pattern.Period.Value);
                                        if (runer > limit)
                                        {
                                            runer = limit;
                                            limit = limit.AddHours(1);
                                        }
                                    }
                                }
                        }
                        break;
                    }
            }
        }
    }

    public class RepeaterPattern
    {
        public List<int> Indexes { get; set; }
        public int? Period { get; set; }
        public int Offset { get; set; }
    }

    public enum RepeaterUnit
    {
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year
    }

    public enum RepeaterLimit
    {
        Hour,
        Day,
        Week,
        Month,
        Year,
        Unlimit
    }
}
