namespace PeriodicDates;

public class UnlimiteRepeaterGenerator : IPeriodGenerator
{
    public IPeriodGenerator SourceGenerator { get; }
    public DateTime StartDate { get; }
    public RepeaterPattern Pattern { get; }

    public UnlimiteRepeaterGenerator(IPeriodGenerator generator, RepeaterPattern pattern, DateTime startDate)
    {
        SourceGenerator = generator;
        Pattern = pattern;
        StartDate = startDate;
    }

    public Period GetCurrentPeriod(DateTime startTime)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Period> GetNextPeriods(DateTime startTime)
    {
        IEnumerable<Period> source;
        int counter = 0;
        if (Pattern.Period == null)
        {
            source = SourceGenerator.GetNextPeriods(StartDate);
            if (Pattern.Offset > 0)
            {
                source = source.Skip(Pattern.Offset);
            }
        }
        else
        {
            if (StartDate > startTime)
            {
                counter = (Pattern.Period.Value - 1) - Pattern.Offset;
                source = SourceGenerator.GetPreviousPeriods(StartDate);
                var needFinish = false;
                DateTime newStartDate = StartDate;
                foreach (var period in source)
                {
                    counter--;
                    if (counter<0)
                    {
                        counter = Pattern.Period.Value;
                    }

                    if (!needFinish)
                    {
                        if (period.End < startTime)
                        {
                            needFinish = true;
                        }
                    }

                    if (needFinish)
                    {
                        if (counter == 0)
                        {
                            newStartDate = period.Begin;
                            break;
                        }
                    }
                }
                source = SourceGenerator.GetNextPeriods(newStartDate);
            }
            else
            {
                source = SourceGenerator.GetNextPeriods(StartDate);
                if (Pattern.Offset > 0)
                {
                    source = source.Skip(Pattern.Offset);
                }
            }
        }

        //проверить что если период не задан, не могут быть выбраны отрицательные индексы
        var patternIndices = Pattern.Indexes.OrderBy(i => i).ToList();
        if (Pattern.Period.HasValue)
        {
            var buffer = new List<Period>(Pattern.Period.Value);
            foreach (var period in source)
            {
                buffer.Add(period);
                counter++;
                if (counter >= Pattern.Period.Value)
                {
                    foreach (var index in patternIndices)
                    {
                        if (buffer[index].Begin>=startTime)
                        {
                            yield return buffer[index];
                        }
                    }

                    counter = 0;
                    buffer.Clear();
                    continue;
                }
            }
        }
        else
        {
            int patternIndex = 0;
            foreach (var period in source)
            {
                if (counter >= 0)
                {
                    if (patternIndex >= patternIndices.Count)
                    {
                        yield break;
                    }

                    if (counter == patternIndices[patternIndex])
                    {
                        if (period.Begin >= startTime)
                            yield return period;
                        patternIndex++;
                    }
                    else if (counter > patternIndices[patternIndex])
                    {
                        patternIndex++;
                    }
                }

                counter++;
            }
        }
    }

    public IEnumerable<Period> GetPreviousPeriods(DateTime startTime)
    {
        throw new NotImplementedException();
    }
}
