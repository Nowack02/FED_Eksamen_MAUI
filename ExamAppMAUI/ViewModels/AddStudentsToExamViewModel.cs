// Fil: ViewModels/AddStudentsToExamViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExamAppMAUI.ViewModels
{
    [QueryProperty(nameof(ExamId), "ExamId")]
    public partial class AddStudentsToExamViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private int _examId;

        [ObservableProperty]
        private string _newStudentName;

        [ObservableProperty]
        private string _newStudentNumber;

        [ObservableProperty]
        private ObservableCollection<Student> _assignedStudents;

        public AddStudentsToExamViewModel(ApplicationDbContext context)
        {
            _context = context;
            AssignedStudents = new ObservableCollection<Student>();
        }

        [RelayCommand]
        private async Task LoadAssignedStudentsAsync()
        {
            AssignedStudents.Clear();
            var studentExams = await _context.StudentExams
                .Where(se => se.ExamId == ExamId)
                .Include(se => se.Student)
                .ToListAsync();

            foreach (var se in studentExams)
            {
                AssignedStudents.Add(se.Student);
            }
        }

        [RelayCommand]
        private async Task AddStudentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewStudentName) || string.IsNullOrWhiteSpace(NewStudentNumber))
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Udfyld venligst både navn og studienummer.", "OK");
                return;
            }

            if (AssignedStudents.Any(s => s.StudentNumber == NewStudentNumber))
            {
                await App.Current.MainPage.DisplayAlert("Fejl", "Denne studerende er allerede tilmeldt eksamenen.", "OK");
                return;
            }

            var studentToAdd = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentNumber == NewStudentNumber);

            if (studentToAdd == null)
            {
                studentToAdd = new Student
                {
                    Name = NewStudentName,
                    StudentNumber = NewStudentNumber
                };
            }
            else
            {
                studentToAdd.Name = NewStudentName;
            }

            var newStudentExam = new StudentExam
            {
                ExamId = this.ExamId,
                Student = studentToAdd
            };
            _context.StudentExams.Add(newStudentExam);

            await _context.SaveChangesAsync();

            // Opdater UI og nulstil felter
            AssignedStudents.Add(studentToAdd);
            NewStudentName = string.Empty;
            NewStudentNumber = string.Empty;
        }

        [RelayCommand]
        private async Task RemoveStudentAsync(Student studentToRemove)
        {
            if (studentToRemove == null) return;

            var studentExamToRemove = await _context.StudentExams
                .FirstOrDefaultAsync(se => se.ExamId == ExamId && se.StudentId == studentToRemove.Id);

            if (studentExamToRemove != null)
            {
                _context.StudentExams.Remove(studentExamToRemove);
                await _context.SaveChangesAsync();
                AssignedStudents.Remove(studentToRemove);

            }
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            // Navigate to ExamsPage
            await Shell.Current.GoToAsync("..");
        }
    }
}