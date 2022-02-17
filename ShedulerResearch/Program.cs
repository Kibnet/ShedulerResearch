// See https://aka.ms/new-console-template for more information

using CronExpressionDescriptor;
using Cronos;

void GetCronDescription(string description, string pattern)
{
    //Перевод в человекочитаемый вид
    var output = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(pattern, new Options { Locale = "ru" });
    Console.WriteLine($"{description} -> {pattern} -> {output}");
    CronExpression expression = CronExpression.Parse(pattern);

    //Получение следующей даты срабатывания
    DateTime? nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);
    Console.WriteLine($"Next occurrence - {nextUtc}");

    //Получение дат срабатывания в период времени
    var list = expression.GetOccurrences(DateTime.Today, DateTime.Today.AddMonths(1));
    Console.WriteLine($"Period occurrences - {string.Join(",", list)}"); 

    //Шаблон расписания, требующий уточнения
}


GetCronDescription("Каждый четверг","* * * * *");
GetCronDescription("Первый день месяца", "* * * * *");
GetCronDescription("Последний день месяца", "* * * * *");
GetCronDescription("раз в год", "* * * * *");
GetCronDescription("раз в сутки", "* * * * *");
GetCronDescription("раз в неделю", "* * * * *");
GetCronDescription("раз в месяц", "* * * * *");
GetCronDescription("1 раз в два месяца", "* * * * *");
GetCronDescription("1 раз в две недели", "* * * * *");
GetCronDescription("1 раз в 4 дня", "* * * * *");
GetCronDescription("1 раз в смену", "* * * * *");
GetCronDescription("два раза в месяц", "* * * * *");
GetCronDescription("два раза в неделю(три раза и тд)", "* * * * *");
GetCronDescription("два раза в месяц", "* * * * *");
GetCronDescription("четная/нечетная неделя/месяц/число", "* * * * *");
GetCronDescription("только рабочие дни", "* * * * *");