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

        public async Task<(int score, string feedback)> EvaluateExamAsync(Exam exam, CancellationToken cancelationToken)
        {
            int totalLocalScore = 0;
            List<string> localFeedbacks = new List<string>();

            foreach (var question in exam.Questions)
            {
                var (score, feedback) = EvaluateQuestion(question);
                totalLocalScore += score;
                localFeedbacks.Add($"Question: {question.QuestionText} - Feedback: {feedback}");
            }
            int averageLocalScore = totalLocalScore / exam.Questions.Count;
            
            // Few-Shot Learning prompt technique to evaluate the exam
            var prompt = FewShotExam.GetFewShotFinanceExamPrompt(exam);
            var iaEvaluation = await ProcessPromptAsync(prompt);
            int finalScore = (int)Math.Round((averageLocalScore * 0.5) + (iaEvaluation.score * 0.5));
            string combinedFeedback = $"Local Feedback:\n{string.Join("\n", localFeedbacks)}\n\nAI Feedback:\n{iaEvaluation.feedback}";

            return (finalScore, combinedFeedback);
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

        // Extract claims from the text
        private List<string> ExtractClaims(string text)
        {
            var claims = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                             .Select(c => c.Trim())
                             .ToList();

            return claims;
        }

        // Compare claims from student answer and correct answer
        private int CompareClaims(string studentAnswer, string correctAnswer)
        {
            var studentClaims = ExtractClaims(studentAnswer);
            var correctClaims = ExtractClaims(correctAnswer);

            int matches = 0;

            foreach (var correctClaim in correctClaims)
            {
                if (studentClaims.Any(sc => AreClaimsSimilar(sc, correctClaim)))
                {
                    matches++;
                }
            }
            return matches;
        }

        // Compare claims similarity
        private bool AreClaimsSimilar(string claim1, string claim2)
        {
            return claim1.Equals(claim2, StringComparison.OrdinalIgnoreCase);
        }

        // Integration of scoring based in claims similarity
        private (int score, string feedback) EvaluateQuestion(Question question)
        {
            int matches = CompareClaims(question.StudentAnswer, question.CorrectAnswer);
            int totalClaims = ExtractClaims(question.CorrectAnswer).Count;
            double score = (double)matches / totalClaims * 100;

            string feedback = matches == totalClaims ? "Excellent! You covered all key points." : $"You missed {totalClaims - matches} key points.";

            return ((int)Math.Round(score), feedback);
        }

        // search the correct answer in the PrinciplesofFinance-WEB.pdf file to compare with the student answer.

    }
}
