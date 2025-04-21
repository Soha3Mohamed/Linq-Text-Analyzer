using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LinqTextAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //C:\Users\soha mohamed\Downloads\linqTextANalyzer\TRY1.txt
            Console.Write("Write the path of the file you want to process: ");
            string path = Console.ReadLine();

            if (File.Exists(path))
            {
                ProcessFile(path);
            }
            else
            {
                Console.WriteLine("File not found. Please write a valid path:");
                path = Console.ReadLine();
                if (File.Exists(path))
                {
                    ProcessFile(path);
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
        }

        public static void ProcessFile(string path)
        {
            try
            {
                string fileText = File.ReadAllText(path);
                Console.WriteLine("\nFile Preview:");
                Console.WriteLine(fileText.Substring(0, Math.Min(fileText.Length, 200)) + "...\n");

                string[] fileWords = fileText
                    .ToLower()
                    .Split(new char[] { ' ', '\n', '\r', '\t', ',', '.', '!', '?', ';', ':', '-', '"', '\'' }
                        , StringSplitOptions.RemoveEmptyEntries);
                string[] fileLines = File.ReadAllLines(path);

                StringBuilder report = new StringBuilder();
                report.AppendLine("--- FILE ANALYSIS ---");
                report.AppendLine($"Number Of Words: {CountWords(fileWords)}");
                report.AppendLine($"Number Of Lines: {fileLines.Length}");
                report.AppendLine($"Number of Characters (with spaces/newlines): {fileText.Length}");
                report.AppendLine($"Number of Visible Characters (NO spaces/newlines): {CountChars(fileText)}");
                report.AppendLine($"Number of Empty lines in the File: {CountEmptyLines(fileLines)}");
                report.AppendLine($"Average Words Per Line: {CountAverageWords(fileWords, fileLines)}");

                report.AppendLine($"Top 5 Frequent Words in File: ");
                foreach (var result in MostFrequentWords(fileWords))
                {
                    report.AppendLine($"{result.Key} - {result.Value}");
                }

                report.AppendLine($"Distinct Words in File: ");
                foreach (var DistictWord in DistinctWordsInFile(fileWords))
                {
                    report.AppendLine(DistictWord);
                }

                report.AppendLine($"Unique Words in File: ");
                foreach (var UniqueWord in UniqueWordsInFile(fileWords))
                {
                    report.AppendLine(UniqueWord);
                }

                Console.WriteLine(report.ToString());

                Console.WriteLine("Enter the word you want to search for: ");
                string word = Console.ReadLine();
                bool found = WordExists(fileWords, word);
                Console.WriteLine(found ? "Yes, the word exists in the file" : "No, word not found");

                Console.WriteLine("Enter the letter you want to search with: ");
                char letter = char.Parse(Console.ReadLine());
                foreach (var w in SearchWordsByFirstLetter(fileWords, letter))
                {
                    Console.WriteLine(w);
                }

                // Save analysis to a file if user wants to
                Console.WriteLine("Do you want to save the analysis to a file? (y/n)");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    File.WriteAllText("analysis.txt", report.ToString());
                    Console.WriteLine("Analysis saved to analysis.txt");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
            }
        }

        public static int CountWords(string[] fileWords) => fileWords.Length;

        public static int CountChars(string fileText) => fileText.Count(c => !char.IsWhiteSpace(c));

        public static int CountEmptyLines(string[] fileLines) => fileLines.Count(line => string.IsNullOrWhiteSpace(line));

        public static int CountAverageWords(string[] fileWords, string[] fileLines)
            => fileWords.Length / fileLines.Length;

        public static Dictionary<string, int> MostFrequentWords(string[] fileWords)
        {
            return fileWords
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static string[] DistinctWordsInFile(string[] fileWords)
        {
            return fileWords.Distinct().ToArray();
        }

        public static string[] UniqueWordsInFile(string[] fileWords)
        {
            return fileWords
                .GroupBy(w => w)
                .Where(g => g.Count() == 1)
                .Select(g => g.Key)
                .ToArray();
        }

        public static bool WordExists(string[] fileWords, string word)
        {
            return fileWords.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
        }

        public static string[] SearchWordsByFirstLetter(string[] fileWords, char letter)
        {
            return fileWords.Where(w => w.StartsWith(letter.ToString(), StringComparison.OrdinalIgnoreCase)).ToArray();
        }
    }
}
