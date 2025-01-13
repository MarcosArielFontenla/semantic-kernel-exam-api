using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Business.Services.Interfaces
{
    public interface IGeminiService
    {
        Task<(int score, string feedback)> EvaluateExamAsync(Exam exam);
    }
}
