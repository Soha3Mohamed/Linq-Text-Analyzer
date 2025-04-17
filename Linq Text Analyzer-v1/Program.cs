namespace Linq_Text_Analyzer_v1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Write the path of the file you want to process: ");
            // FileStream fileStream = new FileStream("mytxt.txt")
            //C:\Users\soha mohamed\Downloads\linqTextANalyzer\TRY1.txt
           string path = Console.ReadLine();
            // string FileText = null;
            if (File.Exists(path))
            {
                ProcessFile(path);
            }
            else
            {
                Console.WriteLine("File not found. write a valid path please:");
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
                string FileText = File.ReadAllText(path);
                Console.WriteLine("\nFile Preview:");
                Console.WriteLine(FileText.Substring(0, Math.Min(FileText.Length, 200)) + "...\n");

               // string[] FileWords = FileText.Split(' ');
                string[] FileWords = FileText
                                    .ToLower()
                                    .Split(new char[] { ' ', '\n', '\r', '\t', ',', '.', '!', '?', ';', ':', '-', '"', '\'' }
                                     ,StringSplitOptions.RemoveEmptyEntries);
                string[] FileLines = File.ReadAllLines(path);

                Console.WriteLine("---FILE ANALYSIS---");
                Console.WriteLine($"Number Of Words: {CountWords(FileWords)}");
                Console.WriteLine($"Number Of Lines: {FileLines.Length}");
                Console.WriteLine($"Number of Characters (with spaces/newlines) {FileText.Length}");
                Console.WriteLine($"Number of Visible Characters (NO spaces/newlines): {CountChars(FileText)}");
                Console.WriteLine("--------------------------");
            }
            catch(Exception ex) {
            {
                Console.WriteLine("An error occurred while processing the file: " + ex.Message);
            }
        }
       
        }

        public static int CountWords(String[] fileWords)
        {
            int count = fileWords.Count();
            return count;
        }

        public static int CountChars(string fileText)
        {
            
            return fileText.Count(c=> !char.IsWhiteSpace(c));
        }
        
    }


    
}
