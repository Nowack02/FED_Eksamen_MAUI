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

        public double AverageGrade
        {
            get
            {
                // Find alle de studerendes eksamener, hvor der er givet en karakter
                var gradedExams = StudentExams?
                    .Where(se => se.Grade.HasValue)
                    .Select(se => se.Grade.Value)
                    .ToList();

                // Hvis der ikke er nogen karakterer, returner 0 (eller en anden standardværdi)
                if (gradedExams == null || !gradedExams.Any())
                {
                    return 0;
                }

                // Beregn og returner gennemsnittet
                return gradedExams.Average();
            }
        }
    }
}