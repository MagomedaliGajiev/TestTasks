using Microsoft.VisualBasic;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogStandardizer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: LogStandardizer <input_file> <output_file>");
                return;
            }

            var inputFile = args[0];
            var outputFile = args[1];

            try
            {
                var processedLines = new List<string>();
                var inputLines = File.ReadAllLines(inputFile);

                foreach (string line in inputLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string standardizedLine;

                    if (IsFormat1(line))
                    {
                        standardizedLine = ProcessFormat1(line);
                    }
                    else if (IsFormat2(line))
                    {
                        standardizedLine = ProcessFormat2(line);
                    }
                    else
                    {
                        continue; // Пропускаем строки неизвестного формата
                    }

                    if (standardizedLine != null)
                    {
                        processedLines.Add(standardizedLine);
                    }
                }

                File.WriteAllLines(outputFile, processedLines);
                Console.WriteLine($"Processed {processedLines.Count} lines. Output saved to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static bool IsFormat1(string line)
        {
            // Проверяем наличие даты в формате dd.MM.yyyy
            return line.Length >= 10 &&
                   char.IsDigit(line[0]) &&
                   char.IsDigit(line[1]) &&
                   line[2] == '.' &&
                   char.IsDigit(line[3]) &&
                   char.IsDigit(line[4]) &&
                   line[5] == '.' &&
                   char.IsDigit(line[6]) &&
                   char.IsDigit(line[7]) &&
                   char.IsDigit(line[8]) &&
                   char.IsDigit(line[9]);
        }

        static bool IsFormat2(string line)
        {
            // Проверяем наличие даты в формате yyyy-MM-dd
            return line.Length >= 10 &&
                   char.IsDigit(line[0]) &&
                   char.IsDigit(line[1]) &&
                   char.IsDigit(line[2]) &&
                   char.IsDigit(line[3]) &&
                   line[4] == '-' &&
                   char.IsDigit(line[5]) &&
                   char.IsDigit(line[6]) &&
                   line[7] == '-' &&
                   char.IsDigit(line[8]) &&
                   char.IsDigit(line[9]);
        }

        static string ProcessFormat1(string line)
        {
            try
            {
                // Разбиваем строку на компоненты
                var datePart = line.Substring(0, 10);
                var timePart = line.Substring(11, 12).Trim();
                var levelStart = 24;

                // Находим конец уровня логирования
                var levelEnd = line.IndexOf(' ', levelStart);
                if (levelEnd == -1) return null;

                var level = line.Substring(levelStart, levelEnd - levelStart);
                var message = line.Substring(levelEnd + 1).Trim();

                // Преобразуем дату в нужный формат
                var date = DateTime.ParseExact(datePart, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var formattedDate = date.ToString("dd-MM-yyyy");

                // Стандартизируем уровень логирования
                var standardizedLevel = StandardizeLogLevel(level);

                // Формируем выходную строку
                return $"{formattedDate}\t{timePart}\t{standardizedLevel}\t\t{message}";
            }
            catch
            {
                return null; // В случае ошибки парсинга пропускаем строку
            }
        }

        static string ProcessFormat2(string line)
        {
            try
            {
                // Разбиваем строку на компоненты
                var parts = line.Split('|', 5);
                if (parts.Length < 5) return null;

                var dateTime = parts[0].Trim();
                var level = parts[1].Trim();
                var method = parts[3].Trim();
                var message = parts[4].Trim();

                // Разделяем дату и время
                var dtParts = dateTime.Split(' ');
                if (dtParts.Length < 2) return null;

                var datePart = dtParts[0];
                var timePart = dtParts[1];

                // Преобразуем дату в нужный формат
                var date = DateTime.ParseExact(datePart, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var formattedDate = date.ToString("dd-MM-yyyy");

                // Стандартизируем уровень логирования
                var standardizedLevel = StandardizeLogLevel(level);

                // Формируем выходную строку
                return $"{formattedDate}\t{timePart}\t{standardizedLevel}\t{method}\t{message}";
            }
            catch
            {
                return null; // В случае ошибки парсинга пропускаем строку
            }
        }

        static string StandardizeLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                "INFO" => "INFO",
                "WARN" => "WARN",
                "ERROR" => "ERROR",
                "DEBUG" => "DEBUG",
                _ => level // Возвращаем исходное значение, если не распознано
            };
        }
    }
}


//Задача 3
//Консольная программа для стандартизации лог-файлов
//Эта программа предназначена для обработки лог-файлов, содержащих записи в двух разных форматах. Цель программы – привести все записи к единому, стандартному виду, упрощая анализ и обработку логов. 
//Необходимо преобразовать записи из входного лог-файла в единый формат и сохранить их в выходной файл.
//Форматы входных файлов
//Формат 1: 10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'
//Дата: 10.03.2025
//Время: 15:14:49.523
//УровеньЛогирования: INFORMATION
//Сообщение: Версия программы: ‘3.4.0.48729’

//Формат 2: 2025 - 03 - 10 15:14:51.5882 | INFO | 11 | MobileComputer.GetDeviceId | Код устройства: '@MINDEO-M40-D-410244015546'
//Дата: 2025 - 03 - 10
//Время: 15:14:51.5882
//УровеньЛогирования: INFO
//ВызвавшийМетод: MobileComputer.GetDeviceId
//Сообщение: Код устройства: ‘@MINDEO - M40 - D - 410244015546’

//Выходной формат
//Формат: Дата Время	Уровень	Логирования	ВызвавшийМетод	Сообщение
//Разделитель между полями: Табуляция.
//Дата: Формат DD-MM-YYYY (день-месяц-год).
//Время: Сохраняется в исходном формате.
//УровеньЛогирования: может иметь одно из нескольких значений:
//1)INFO(INFORMATION)
//2)WARN(WARNING)
//3)ERROR
//4)DEBUG
//Эти значения выбираются в зависимости от УровеньЛогирования входной записи. Пример:
//10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729' - УровеньЛогирования для этой входной записи INFORMATION, но на выходной записи должен быть INFO, такая же логика и у WARNING - WARN.
