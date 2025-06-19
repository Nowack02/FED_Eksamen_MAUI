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
    public partial class ProcessExamViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _context;
        private List<StudentExam> _studentQueue;
        private StudentExam _activeStudentExamModel;
        private IDispatcherTimer _countdownTimer;

        [ObservableProperty]
        private int _examId;

        [ObservableProperty]
        private ObservableCollection<StudentExam> _upcomingStudents;

        [ObservableProperty]
        private string _activeStudentName;

        [ObservableProperty]
        private string _activeStudentNumber;

        [ObservableProperty]
        private int? _uiSelectedQuestionNumber;

        [ObservableProperty]
        private DateTime? _uiStartTime;

        [ObservableProperty]
        private DateTime? _uiEndTime;

        [ObservableProperty]
        private TimeSpan? _uiActualExaminationDuration;

        [ObservableProperty]
        private string _uiNotes;

        [ObservableProperty]
        private int? _uiSelectedGrade;

        [ObservableProperty]
        private string _countdownText;

        [ObservableProperty]
        private bool _isStudentActive;

        [ObservableProperty]
        private bool _canDrawNumber;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTimerVivible))]
        private bool _isTimerRunning;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTimerVivible))]
        private bool _canGradeStudent;

        [ObservableProperty]
        private bool _isExamCompleted;

        public bool IsTimerVivible => CanDrawNumber || CanGradeStudent;

        public List<int> AllGrades => new List<int> { -3, 00, 02, 4, 7, 10, 12 };

        public ProcessExamViewModel(ApplicationDbContext context)
        {
            _context = context;
            _studentQueue = new List<StudentExam>();
            UpcomingStudents = new ObservableCollection<StudentExam>();
        }


        [RelayCommand]
        private async Task LoadExamProcessAsync()
        {
            var exam = await _context.Exams
                .Include(e => e.StudentExams).ThenInclude(se => se.Student)
                .FirstOrDefaultAsync(e => e.Id == ExamId);

            if (exam == null) return;

            // Sorter studerende efter rækkefølge og læg dem i køen
            _studentQueue = exam.StudentExams.OrderBy(se => se.ExaminationOrder).ToList();
            UpcomingStudents = new ObservableCollection<StudentExam>(_studentQueue);

            IsExamCompleted = false;
            LoadNextStudent(); // Indlæs den første studerende i køen
        }

        [RelayCommand]
        private async Task DrawNumberAsync()
        {
            if (_activeStudentExamModel == null) return;

            UiStartTime = DateTime.Now;
            _activeStudentExamModel.StartTime = UiStartTime;

            var exam = await _context.Exams.FindAsync(ExamId);
            int numberOfQuestions = exam?.NumberOfQuestions ?? 1;

            // Træk et tilfældigt spørgsmål
            UiSelectedQuestionNumber = new Random().Next(1, numberOfQuestions + 1);
            _activeStudentExamModel.SelectedQuestionNumber = UiSelectedQuestionNumber;

            // Gem det trukne nummer
            _context.Update(_activeStudentExamModel);
            await _context.SaveChangesAsync();

            // Start nedtællingstimeren
            StartCountdown();

            // Opdater UI-tilstand: skjul "Træk nummer", vis "Afslut" sektionen
            CanDrawNumber = false;
            IsTimerRunning = true;
        }

        [RelayCommand]
        private void StopTime()
        {
            _countdownTimer?.Stop();
            EnterGradingState();
        }

        [RelayCommand]
        private async Task EndExamAsync()
        {
            if (_activeStudentExamModel == null) return;

            UiEndTime = DateTime.Now;
            if (UiStartTime.HasValue) { UiActualExaminationDuration = UiEndTime - UiStartTime; }
            _activeStudentExamModel.EndTime = UiEndTime;
            _activeStudentExamModel.ActualExaminationDuration = UiActualExaminationDuration;
            _activeStudentExamModel.Grade = UiSelectedGrade;
            _activeStudentExamModel.Notes = UiNotes;
            _context.Update(_activeStudentExamModel);
            await _context.SaveChangesAsync();

            CanGradeStudent = false; // Skjul bedømmelses-sektionen igen
            LoadNextStudent();
        }

        private void LoadNextStudent()
        {
            _countdownTimer?.Stop();
            CountdownText = "";

            var nextStudent = _studentQueue.FirstOrDefault(s => s.EndTime == null);

            if (nextStudent != null)
            {
                _activeStudentExamModel = nextStudent;
                // ... data kopieres som før ...
                ActiveStudentName = _activeStudentExamModel.Student.Name;
                ActiveStudentNumber = _activeStudentExamModel.Student.StudentNumber;
                UiStartTime = _activeStudentExamModel.StartTime;
                UiEndTime = _activeStudentExamModel.EndTime;
                UiSelectedQuestionNumber = _activeStudentExamModel.SelectedQuestionNumber;
                UiActualExaminationDuration = _activeStudentExamModel.ActualExaminationDuration;
                UiNotes = _activeStudentExamModel.Notes;
                UiSelectedGrade = _activeStudentExamModel.Grade;

                // Nulstil UI-tilstand for den nye studerende
                IsStudentActive = true;
                bool isStarted = _activeStudentExamModel.StartTime.HasValue;

                CanDrawNumber = !isStarted;
                IsTimerRunning = isStarted; // Hvis nummer er trukket, kører timeren
                CanGradeStudent = false; // Bedømmelse er altid skjult i starten

                if (isStarted)
                {
                    StartCountdown();
                }
                UpcomingStudents.Remove(nextStudent);
            }
            else
            {
                IsStudentActive = false;
                IsExamCompleted = true;
            }
        }

        private void StartCountdown()
        {
            var exam = _context.Exams.Find(ExamId);
            if (exam == null) return;

            var elapsedTime = (UiStartTime.HasValue) ? DateTime.Now - UiStartTime.Value : TimeSpan.Zero;
            var remainingTime = exam.ExaminationDuration - elapsedTime;

            _countdownTimer?.Stop();
            _countdownTimer = App.Current.Dispatcher.CreateTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += (s, e) =>
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                if (remainingTime.TotalSeconds > 0)
                {
                    CountdownText = remainingTime.ToString(@"mm\:ss");
                }
                else
                {
                    CountdownText = "TIDEN ER UDLØBET";
                    _countdownTimer.Stop();
                    EnterGradingState(); // Gå til bedømmelses-tilstand når tiden udløber
                }
            };
            _countdownTimer.Start();
        }
        
        private void EnterGradingState()
        {
            IsTimerRunning = false;
            CanGradeStudent = true;
        }
    }
}