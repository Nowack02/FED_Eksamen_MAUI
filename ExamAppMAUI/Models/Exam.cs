using System;
using System.Collections.Generic;

namespace ExamAppMAUI.Models
{
    public class Exam
    {
        public int Id { get; set; } // Primary Key
        public required string Term { get; set; }
        public required string Course { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfQuestions { get; set; }
        public TimeSpan ExaminationDuration { get; set; }
        public DateTime StartTime { get; set; }

        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
    }
}