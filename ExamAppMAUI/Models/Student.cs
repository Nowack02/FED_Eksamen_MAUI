using System.Collections.Generic;

namespace ExamAppMAUI.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary Key
        public required string StudentNumber { get; set; }
        public required string Name { get; set; }

        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
    }
}