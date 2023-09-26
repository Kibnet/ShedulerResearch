// See https://aka.ms/new-console-template for more information

using CronExpressionDescriptor;
using Cronos;

//Пример интерфейса для настройки https://www.freeformatter.com/cron-expression-generator-quartz.html

//cron4j - сочетание нескольких паттернов через |
//паттерн должен быть в состоянии вычислить следующую дату запуска основываясь только на своих данных
//нужно решить проблему границ месяцев и годов, для этого нужен признак и стартовая дата
//нужно добавить задание года

void GetCronDescription(string description, string pattern, string comment = "ничего")
{
    //Перевод в человекочитаемый вид
    var output = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(pattern, new Options { Locale = "ru" });
    Console.WriteLine($"{description} -> {pattern} -> {output}");
    CronExpression expression = CronExpression.Parse(pattern);
    
    Console.WriteLine($"Чтобы заработало нужно уточнить {comment}");

    //Получение следующей даты срабатывания
    DateTime? nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);
    Console.WriteLine($"Следующее срабатывание - {nextUtc}");

    //Получение дат срабатывания в период времени
    var list = expression.GetOccurrences(new DateTime(2022,1,1,0,0,0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Take(5).ToList();
    Console.WriteLine($"Первые 5 срабатываний с начала 2022 года:");
    foreach (var dateTime in list)
    {
        Console.WriteLine(dateTime);
    }
    Console.WriteLine();
}


GetCronDescription("Каждый четверг", "0 10 * * 4", "время");
GetCronDescription("Первый день месяца", "0 10 1 * *", "время");
GetCronDescription("Последний день месяца", "0 10 L * *", "время");
GetCronDescription("раз в год", "0 10 1 6 *", "время, месяц и число месяца");
GetCronDescription("раз в сутки", "0 10 * * *", "время");
GetCronDescription("раз в неделю", "0 10 * * 1", "время и день недели");
GetCronDescription("раз в месяц", "0 10 15 * *", "время и число месяца");
GetCronDescription("1 раз в два месяца", "0 10 15 1/2 *", "время, с какого месяца начать и число месяца");
GetCronDescription("1 раз в две недели", "0 10 2/14 * *", "время и день месяца. Не совсем корректно, на границе месяца");
GetCronDescription("1 раз в 4 дня", "0 10 */4 * *", "время и день месяца. Не совсем корректно, на границе месяца");
GetCronDescription("1 раз в смену", "0 2/8 * * *", "время с начала смены и длительность смены");
GetCronDescription("два раза в месяц", "0 10 1,15 * *", "дни месяца");
GetCronDescription("два раза в неделю", "0 10 * * 1,4", "дни недели");
GetCronDescription("три раза и тд", "0 10 * * 1,3,5", "дни недели");
GetCronDescription("чётная/нечётная месяц", "0 10 * 1/2 *", "время и чётность");
GetCronDescription("чётная/нечётная число", "0 10 1/2 * *", "время и чётность");
GetCronDescription("чётная/нечётная неделя", "0 10 * * 3", "время и день недели. Нужна фильтрация по номеру недели");
GetCronDescription("только будние дни", "0 10 * * 1-5");
GetCronDescription("только выходные дни", "0 10 * * 6-7");
GetCronDescription("только рабочие дни", "0 10 * * *", "ничего. Нужна фильтрация по производственному календарю");//