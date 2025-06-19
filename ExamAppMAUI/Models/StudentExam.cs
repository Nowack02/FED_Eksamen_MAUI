using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace ExamAppMAUI.Models
{
    public partial class StudentExam : ObservableObject
    {
        public int Id { get; set; }

        public int ExamId { get; set; }
        public Exam? Exam { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int ExaminationOrder { get; set; }
        public int? SelectedQuestionNumber { get; set; }
        public TimeSpan? ActualExaminationDuration { get; set; }
        public string? Notes { get; set; }
        public int? Grade { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}