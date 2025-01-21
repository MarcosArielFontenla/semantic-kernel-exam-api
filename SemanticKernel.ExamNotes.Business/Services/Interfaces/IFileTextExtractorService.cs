namespace SemanticKernel.ExamNotes.Business.Services.Interfaces
{
    public interface IFileTextExtractorService
    {
        string ExtractTextFromPdf(string filePath);
    }
}
