using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using System;
using System.Threading.Tasks;

namespace ExamAppMAUI.ViewModels
{
    public partial class CreateExamViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;

        [ObservableProperty]
        private string _term;

        [ObservableProperty]
        private string _course;

        [ObservableProperty]
        private DateTime _date = DateTime.Today;

        [ObservableProperty]
        private int _numberOfQuestions;

        [ObservableProperty]
        private string _examinationDuration = "20";
        [ObservableProperty]

        private TimeSpan _startTime = TimeSpan.FromHours(9);

        [ObservableProperty]
        private string _errorMessage;

        public CreateExamViewModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Kommando til at håndtere klik på "Opret Eksamen" knappen
        [RelayCommand]
        private async Task CreateExamAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Term))
            {
                ErrorMessage = "Termin skal udfyldes.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Course))
            {
                ErrorMessage = "Kursus skal udfyldes.";
                return;
            }
            if (NumberOfQuestions <= 0)
            {
                ErrorMessage = "Antal spørgsmål skal være et positivt tal.";
                return;
            }

            int examinationMinutes;
            if(!int.TryParse(ExaminationDuration, out examinationMinutes))
            {
                ErrorMessage = "Eksamen varighed skal være et heltal i minutter.";
                return;
            }
            if (examinationMinutes <= 0)
            {
                ErrorMessage = "Eksamen varighed skal være et positivt tal i minutter.";
                return;
            }
            
            TimeSpan parsedExaminationDuration = TimeSpan.FromMinutes(examinationMinutes);

            try
            {
                var newExam = new Exam
                {
                    Term = Term,
                    Course = Course,
                    Date = Date,
                    NumberOfQuestions = NumberOfQuestions,
                    ExaminationDuration = parsedExaminationDuration,
                    StartTime = DateTime.Today.Add(StartTime)
                };

                _dbContext.Exams.Add(newExam);
                await _dbContext.SaveChangesAsync();

                // Nulstil formularfelter efter succes
                Term = string.Empty;
                Course = string.Empty;
                NumberOfQuestions = 0;
                ExaminationDuration = "20";
                StartTime = TimeSpan.FromHours(9);
                Date = DateTime.Today;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fejl ved oprettelse af eksamen: {ex.Message}";
            }
        }
    }
}