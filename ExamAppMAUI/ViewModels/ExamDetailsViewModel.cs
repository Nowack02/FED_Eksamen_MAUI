using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExamAppMAUI.Data;
using ExamAppMAUI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ExamAppMAUI.ViewModels;

[QueryProperty(nameof(ExamId), "ExamId")]
public partial class ExamDetailsViewModel : ObservableObject
{
    private readonly ApplicationDbContext _context;

    [ObservableProperty]
    private int _examId;

    // Egenskab til at vise eksamenens navn i sidens titel
    [ObservableProperty]
    private string _examCourseTitle;

    // Liste over alle de studerendes resultater for denne eksamen
    [ObservableProperty]
    private ObservableCollection<StudentExam> _examResults;

    public ExamDetailsViewModel(ApplicationDbContext context)
    {
        _context = context;
        ExamResults = new ObservableCollection<StudentExam>();
    }

    [RelayCommand]
    private async Task LoadDetailsAsync()
    {
        // Hent den specifikke eksamen og alle dens resultater
        var exam = await _context.Exams
            .Include(e => e.StudentExams)
            .ThenInclude(se => se.Student)
            .FirstOrDefaultAsync(e => e.Id == ExamId);

        if (exam == null) return;

        ExamCourseTitle = exam.Course; // Sæt titlen

        // Fyld listen med resultater
        ExamResults.Clear();
        foreach (var result in exam.StudentExams)
        {
            ExamResults.Add(result);
        }
    }
    
    // Kommando til "Tilbage"-knappen
    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
