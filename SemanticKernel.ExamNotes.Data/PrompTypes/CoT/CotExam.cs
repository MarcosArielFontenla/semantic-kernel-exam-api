using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Data.PrompTypes.CoT
{
    public static class CotExam
    {
        public static string GetCotFinanceExamPrompt(Exam exam)
        {
            return $@"
                    You are an expert professor in {exam.Subject}.
                    Evaluate the student's answers step by step. For each question:

                    - Determine if the answer is relevant to the question.
                    - Assess the clarity and detail of the answer.
                    - Check if examples or additional context are provided.
                    - Assign a score for the individual question and explain why.

                    Questions and Student Answers:
                    {string.Join("\n", exam.Questions.Select(q =>
                       $"- Question: {q.QuestionText}\n  Student Answer: {q.StudentAnswer}"))}

                    Finally, provide an overall evaluation in JSON format:
                    {{
                        ""Score"": (total rating between 0 and 100),
                        ""Feedback"": ""summary of strengths and areas for improvement""
                    }}
                ";
        }
    }
}
