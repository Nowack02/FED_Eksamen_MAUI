using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using ExamAppMAUI.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ExamAppMAUI.ViewModels
{
    public partial class ExamsViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Exam> _exams;

        public ExamsViewModel(ApplicationDbContext context)
        {
            _context = context;
            Exams = new ObservableCollection<Exam>();
        }

        [RelayCommand]
        private async Task LoadExamsAsync()
        {
            Exams.Clear();
            var activeExams = await _context.Exams
                .Where(e => !e.StudentExams.Any() || e.StudentExams.Any(se => se.EndTime == null))
                .ToListAsync();

            foreach (var exam in activeExams)
            {
                Exams.Add(exam);
            }
        }

        [RelayCommand]
        private async Task GoToAddStudentsToExamAsync(Exam exam)
        {
            if (exam == null) return;
            await Shell.Current.GoToAsync($"{nameof(AddStudentsToExamPage)}?ExamId={exam.Id}");
        }

        // Kommando for knappen "Start eksamen"
        [RelayCommand]
        private async Task GoToProcessExamAsync(Exam exam)
        {
            if (exam == null) return;
            await Shell.Current.GoToAsync($"{nameof(ProcessExamPage)}?ExamId={exam.Id}");
        }
    }
}