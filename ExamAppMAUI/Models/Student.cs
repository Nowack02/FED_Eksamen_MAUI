using System.Collections.Generic;

namespace ExamAppMAUI.Models
{
    public class Student
    {
        public int Id { get; set; } // Primary Key
        public string StudentNumber { get; set; }
        public string Name { get; set; }

        public ICollection<StudentExam> StudentExams { get; set; } = new List<StudentExam>();
    }
}