using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ExamAppMAUI.Data;
using ExamAppMAUI.ViewModels;
using ExamAppMAUI.Views;
using System.IO;
using Microsoft.Maui.Storage;

namespace ExamAppMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
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

		builder.Services.AddTransient<CreateExamViewModel>();
		builder.Services.AddTransient<CreateExamPage>();

		builder.Services.AddTransient<StartExamPage>();
		builder.Services.AddTransient<AddStudentsPage>();
		builder.Services.AddTransient<HistoryPage>();

		return builder.Build();
	}
}
