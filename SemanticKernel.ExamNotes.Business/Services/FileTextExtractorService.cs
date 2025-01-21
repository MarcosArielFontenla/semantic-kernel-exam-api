using SemanticKernel.ExamNotes.Business.Services.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace SemanticKernel.ExamNotes.Business.Services
{
    public class FileTextExtractorService : IFileTextExtractorService
    {
        public FileTextExtractorService()
        {
            
        }

        public string ExtractTextFromPdf(string filePath)
        {
            using (PdfReader pdfReader = new PdfReader(filePath))
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
            {
                StringWriter stringWriter = new StringWriter();
                for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string pageContent = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);
                    stringWriter.Write(pageContent);
                }
                return stringWriter.ToString();
            }
        }

        public void SaveTextToFile(string text, string filePath)
        {
            File.WriteAllText(filePath, text);
        }

        public int GetCharacterCount(string text)
        {
            return text.Length;
        }

        public void ExtractAndSaveText(string filePath, string txtFilePath)
        {
            string extractedText = ExtractTextFromPdf(filePath);
            SaveTextToFile(extractedText, txtFilePath);
            int characterCount = GetCharacterCount(extractedText);
            Console.WriteLine($"Character Count: {characterCount}");
        }
    }
}
