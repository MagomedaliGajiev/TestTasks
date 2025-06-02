//Задача 1
//Дана строка, содержащая n маленьких букв латинского алфавита. Требуется реализовать алгоритм компрессии этой строки, замещающий группы последовательно идущих одинаковых букв формой "sc" (где "s" – символ, "с" – количество букв в группе), а также алгоритм декомпрессии, возвращающий исходную строку по сжатой.

//Если буква в группе всего одна – количество в сжатой строке не указываем, а пишем её как есть.

//Пример:

//Исходная строка: aaabbcccdde
//Сжатая строка: a3b2c3d2e
public class Program
{
    public static void Main()
    {
        var original = Console.ReadLine();
        var compressed = StringCompressor.Compress(original);
        var decompressed = StringCompressor.Decompress(compressed);

        Console.WriteLine("Original: " + original);
        Console.WriteLine("Compressed: " + compressed);
        Console.WriteLine("Decompressed: " + decompressed);
    }
}