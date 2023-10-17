using System;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace PDFRenamer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Deze tool leest alle PDF bestanden uit een map, zoekt naar tekst tussen 2 opgegeven woorden en hernoemt de PDF file naar het resultaat hiervan");
            Console.WriteLine("*****************************************************");
            Console.WriteLine();

            Console.WriteLine(@"Geef het pad van de map waar alle PDF bestanden staan (bv C:\Users\JohnDoe\Desktop\PDFFiles) : ");
            string folder = Console.ReadLine();

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("Error! De map die u opgaf bestaat niet. Druk op enter om af te sluiten...");
                Console.ReadLine();
                return;
            }

            Console.Write("Geef het eerste woord in: ");
            string firstWord = Console.ReadLine();

            Console.Write("Geef het tweede woord in: ");
            string secondWord = Console.ReadLine();
            Console.WriteLine();

            var allFiles = GetAllPDFFiles(folder);
            string fileContent;
            string newFileName;
            foreach (var pdfFile in allFiles)
            {
                fileContent = ReadPDF(pdfFile.FullName);
                Console.WriteLine(fileContent);
                newFileName = GetTextBetween(firstWord, secondWord, fileContent);
                if (string.IsNullOrEmpty(newFileName)) 
                    Console.WriteLine("Geen match gevonden in " + pdfFile.Name);
                else
                {
                    newFileName += ".pdf";
                    System.IO.File.Move(pdfFile.FullName, pdfFile.DirectoryName + "/" + newFileName);
                    Console.WriteLine("'" + pdfFile.Name + " hernoemd naar '" + newFileName + "'");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Druk op enter om te stoppen...");
            Console.ReadLine();
        }

        private static string GetTextBetween(string firstWord, string secondWord, string text)
        {
            string pattern = @"(?<=" + firstWord + @")(\s*.*\s*)(?=" + secondWord + ")";
            Regex reg = new Regex(pattern, RegexOptions.None);

            MatchCollection matches = reg.Matches(text);
            if (matches.Count == 0)
            {
                return string.Empty;
            }
            foreach (Match match in matches)
            {
                return match.Value.Trim().Replace(" ", "");
            }
            return string.Empty;
        }

        private static string ReadPDF(string path)
        {
            StringBuilder sb = new StringBuilder();

            using (PdfReader reader = new PdfReader(path))
            {
                for (int pageNo = 1; pageNo <= reader.NumberOfPages; pageNo++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string text = PdfTextExtractor.GetTextFromPage(reader, pageNo, strategy);
                    //text = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text)));
                    sb.Append(text);
                }
            }
            return sb.ToString();
        }

        private static FileInfo[] GetAllPDFFiles(string folder)
        {
            DirectoryInfo d = new DirectoryInfo(folder);
            return d.GetFiles("*.pdf");
        }
    }
}
