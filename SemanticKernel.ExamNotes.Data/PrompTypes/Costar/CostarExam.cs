using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Data.PrompTypes.Costar
{
    public static class CostarExam
    {
        public static string GetCostarFinanceExamPrompt(Exam exam)
        {
            return $@"
                    Context:
                    You are an expert evaluator in {exam.Subject}. Your job is to assess student answers.

                    Task:
                    Evaluate the following answers for correctness, detail, and clarity. Assign a score (0-100) and provide constructive feedback.

                    Steps:
                    1. Analyze the student's response.
                    2. Compare it to an ideal answer (if necessary).
                    3. Assign a score based on relevance, clarity, and completeness.
                    4. Provide constructive feedback.

                    Questions and Student Answers:
                    {string.Join("\n", exam.Questions.Select(q =>
                       $"- Question: {q.QuestionText}\n  Student Answer: {q.StudentAnswer}"))}

                    Alternatives:
                    You can structure the response as follows:
                    {{
                        ""Score"": (number between 0 and 100),
                        ""Feedback"": ""brief explanation of the student's strengths and areas for improvement""
                    }}

                    Review:
                    Double-check your evaluation for consistency before submitting the response.
                ";
        }
    }
}
