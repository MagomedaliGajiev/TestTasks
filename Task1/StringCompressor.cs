using System.Text;

public class StringCompressor
{
    public static string Compress(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        var compressed = new StringBuilder();
        var count = 1;

        for (int i = 1; i <= input.Length; i++)
        {
            if (i < input.Length && input[i] == input[i - 1])
            {
                count++;
            }
            else
            {
                compressed.Append(input[i - 1]);
                if (count > 1)
                    compressed.Append(count);
                count = 1;
            }
        }

        return compressed.ToString();
    }

    public static string Decompress(string compressed)
    {
        if (string.IsNullOrEmpty(compressed))
            return string.Empty;

        var decompressed = new StringBuilder();
        var index = 0;

        while (index < compressed.Length)
        {
            var currentChar = compressed[index++];
            if (index < compressed.Length && char.IsDigit(compressed[index]))
            {
                var numStart = index;
                while (index < compressed.Length && char.IsDigit(compressed[index]))
                    index++;

                var numStr = compressed.Substring(numStart, index - numStart);
                var count = int.Parse(numStr);
                decompressed.Append(currentChar, count);
            }
            else
            {
                decompressed.Append(currentChar);
            }
        }

        return decompressed.ToString();
    }
}
