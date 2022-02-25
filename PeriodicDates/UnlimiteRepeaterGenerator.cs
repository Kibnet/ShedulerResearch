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
        //if (StartDate < startTime)
        var source = SourceGenerator.GetNextPeriods(StartDate);

        //проверить что если период не задан, не могут быть выбраны отрицательные индексы
        int counter = 0 - Pattern.Offset;
        var patternIndices = Pattern.Indexes.OrderBy(i => i).ToList();
        if (Pattern.Period.HasValue)
        {
            var buffer = new List<Period>(Pattern.Period.Value);
            foreach (var period in source)
            {
                if (counter < 0)
                {
                    counter++;
                }
                else
                {
                    buffer.Add(period);
                    counter++;
                    if (counter >= Pattern.Period.Value)
                    {
                        foreach (var index in patternIndices)
                        {
                            yield return buffer[index];
                        }

                        counter = 0;
                        buffer.Clear();
                        continue;
                    }
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
                        if(period.Begin>= startTime)
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
