using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ExamAppMAUI.Data;
using ExamAppMAUI.ViewModels;
using ExamAppMAUI.Views;
using System.IO;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;

namespace ExamAppMAUI;

public static class MauiProgram
{
	public static IServiceProvider Services { get; private set; }

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ExamApp.db");

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlite($"Filename={dbPath}"));

		// ViewModels
		builder.Services.AddTransient<CreateExamViewModel>();
		builder.Services.AddSingleton<ExamsViewModel>();
		builder.Services.AddTransient<AddStudentsToExamViewModel>();
		builder.Services.AddTransient<ProcessExamViewModel>();
		builder.Services.AddTransient<HistoryViewModel>();
		builder.Services.AddTransient<ExamDetailsViewModel>();
		
		// Views
		builder.Services.AddTransient<CreateExamPage>();
		builder.Services.AddSingleton<ExamsPage>();
		builder.Services.AddTransient<AddStudentsToExamPage>();
		builder.Services.AddTransient<ProcessExamPage>();
		builder.Services.AddTransient<HistoryPage>();
		builder.Services.AddTransient<ExamDetailsPage>();

		Services = builder.Services.BuildServiceProvider();

		return builder.Build();
	}
}
