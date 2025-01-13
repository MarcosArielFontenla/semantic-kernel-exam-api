namespace SemanticKernel.ExamNotes.Data.Models
{
    public class Exam
    {
        public string Subject { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
