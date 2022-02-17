// See https://aka.ms/new-console-template for more information

using CronExpressionDescriptor;

void GetCronDescription(string description, string pattern)
{
    var output = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(pattern, new Options { Locale = "ru" });
    Console.WriteLine($"{description} -> {pattern} -> {output}");
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