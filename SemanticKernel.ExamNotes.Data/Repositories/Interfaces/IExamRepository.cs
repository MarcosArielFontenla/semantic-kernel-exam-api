using SemanticKernel.ExamNotes.Data.Models;

namespace SemanticKernel.ExamNotes.Data.Repositories.Interfaces
{
    public interface IExamRepository
    {
        Task<int> SaveExam(Exam exam);
        Task<List<Exam>> GetExamByStudentId(int studentId);
    }
}
