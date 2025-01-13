using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Data.PrompTypes.FewShot
{
    public static class FewShotExam
    {
        public static string GetFewShotFinanceExamPrompt(Exam exam)
        {
            return $@"
                            You are an expert professor in the subject of {exam.Subject}.
                            Your task is to evaluate the following student answers and provide a score (0-100) and brief feedback.

                            Examples of evaluation:
                            Question: What is Finance?
                            Student Answer: Finance is the study of money management.
                            Score: 85
                            Feedback: Good definition, but it could include examples for more depth.

                            Question: The role of Finance in an Organization?
                            Student Answer: It helps in budgeting and investment decisions.
                            Score: 90
                            Feedback: Strong answer with relevant points.

                            Now evaluate these questions:
                            {string.Join("\n", exam.Questions.Select(q =>
                               $"- Question: {q.QuestionText}\n  Student Answer: {q.StudentAnswer}"))}

                            Provide the results in JSON format:
                            {{
                                ""Score"": (overall rating between 0 and 100),
                                ""Feedback"": ""brief feedback for the student summarizing their performance""
                            }}
                        ";
        }
    }
}
