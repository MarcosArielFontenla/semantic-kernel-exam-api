using Microsoft.SemanticKernel;
using SemanticKernel.ExamNotes.Business.Services.Interfaces;
using SemanticKernel.ExamNotes.Data.Models;
using SemanticKernel.ExamNotes.Data.PrompTypes.Costar;
using SemanticKernel.ExamNotes.Data.PrompTypes.CoT;
using SemanticKernel.ExamNotes.Data.PrompTypes.FewShot;
using System.Text.Json;

namespace SemanticKernel.ExamNotes.Business.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly Kernel _kernel;

        public GeminiService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<(int score, string feedback)> EvaluateExamAsync(Exam exam)
        {
            // Few-Shot Learning prompt technique to evaluate the exam
            var prompt = FewShotExam.GetFewShotFinanceExamPrompt(exam);

            // Costar Learning prompt technique to evaluate the exam
            var costarPrompt = CostarExam.GetCostarFinanceExamPrompt(exam);

            // CoT (Chain of Thought) Learning prompt technique to evaluate the exam
            var cotPrompt = CotExam.GetCotFinanceExamPrompt(exam);

            return await ProcessPromptAsync(prompt);
        }

        private async Task<(int score, string feedback)> ProcessPromptAsync(string prompt)
        {
            try
            {
                var evaluateFunction = _kernel.CreateFunctionFromPrompt(prompt);
                var result = await _kernel.InvokeAsync(evaluateFunction);
                

                var cleanJsonResponse = result.GetValue<string>()
                                              .Trim()
                                              .TrimStart('`')
                                              .TrimEnd('`')
                                              .Replace("```json", "")
                                              .Replace("```", "")
                                              .Replace("json", "")
                                              .Trim();

                if (!cleanJsonResponse.StartsWith("{"))
                {
                    cleanJsonResponse = cleanJsonResponse.Substring(cleanJsonResponse.IndexOf("{"));
                }

                var evaluationResponse = JsonSerializer.Deserialize<ExamEvaluation>(cleanJsonResponse);
                return (evaluationResponse.Score, evaluationResponse.Feedback);
            }
            catch (Exception ex)
            {
                return (0, $"Error to evaluate the exam: {ex.Message}");
            }
        }
    }
}
