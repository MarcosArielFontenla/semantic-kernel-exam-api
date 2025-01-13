using SemanticKernel.ExamNotes.Business.Services.Interfaces;
using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Business.Services
{
    public class ExamService : IExamService
    {
        private readonly IGeminiService _geminiService;

        public ExamService(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public async Task<(int score, string feedback)> EvaluateExamAsync(Exam exam)
        {
            return await _geminiService.EvaluateExamAsync(exam);
        }
    }
}
