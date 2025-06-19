using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ExamAppMAUI.ViewModels
{
    public partial class HistoryViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;

        [ObservableProperty]
        private ObservableCollection<Exam> _completedExams;

        public HistoryViewModel(ApplicationDbContext context)
        {
            _context = context;
            CompletedExams = new ObservableCollection<Exam>();
        }

        [RelayCommand]
        private async Task LoadCompletedExamsAsync()
        {
            CompletedExams.Clear();

            var finishedExams = await _context.Exams
                .Where(e => e.StudentExams.Any() && e.StudentExams.All(se => se.EndTime != null))
                .Include(e => e.StudentExams)
                .ThenInclude(se => se.Student)
                .ToListAsync();

            foreach (var exam in finishedExams)
            {
                CompletedExams.Add(exam);
            }
        }
    }
}