using System;

namespace ExamAppMAUI.Models
{
    public class StudentExam
    {
        public int Id { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int ExaminationOrder { get; set; }

        public int? SelectedQuestionNumber { get; set; }
        public TimeSpan? ActualExaminationDuration { get; set; }
        public string Notes { get; set; }
        public Grade? Grade { get; set; }
    }

    public enum Grade
    {
        NotSet,
        Minus3,
        ZeroZero,
        ZeroTwo,
        Four,
        Seven,
        Ten,
        Twelve
    }
}