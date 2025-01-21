namespace SemanticKernel.ExamNotes.Data.Models
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public string StudentAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
