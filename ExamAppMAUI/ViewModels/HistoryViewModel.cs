// Fil: ViewModels/HistoryViewModel.cs

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using ExamAppMAUI.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        // Kommando til at indlæse afsluttede eksamener
        [RelayCommand]
        private async Task LoadCompletedExamsAsync()
        {
            CompletedExams.Clear();

            var finishedExams = await _context.Exams
                .Include(e => e.StudentExams)
                .Where(e => e.StudentExams.Any() && e.StudentExams.All(se => se.EndTime != null))
                .ToListAsync();

            foreach (var exam in finishedExams)
            {

                CompletedExams.Add(exam);
            }
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Exam exam)
        {
            if (exam == null) return;

            await Shell.Current.GoToAsync($"{nameof(ExamDetailsPage)}?ExamId={exam.Id}");
        }
        
        [RelayCommand]
        private async Task DeleteExamAsync(Exam examToDelete)
        {
            if (examToDelete == null) return;

            bool userConfirmed = await App.Current.MainPage.DisplayAlert(
                "Slet Eksamen", 
                $"Er du sikker på, du vil slette eksamenen '{examToDelete.Course}'? Handlingen kan ikke fortrydes.", 
                "Ja, slet", 
                "Nej");

            if (!userConfirmed) return;


            _context.Exams.Remove(examToDelete);
            await _context.SaveChangesAsync();

            // Opdater brugergrænsefladen med det samme ved at fjerne den fra listen
            CompletedExams.Remove(examToDelete);
        }
    }
}