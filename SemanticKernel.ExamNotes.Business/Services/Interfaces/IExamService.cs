using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Business.Services.Interfaces
{
    public interface IExamService
    {
        Task<(int score, string feedback)> EvaluateExamAsync(Exam exam, CancellationToken cancellationToken);
    }
}
