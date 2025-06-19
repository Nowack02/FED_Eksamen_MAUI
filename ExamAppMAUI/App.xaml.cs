using Microsoft.EntityFrameworkCore;
using ExamAppMAUI.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;


namespace ExamAppMAUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		MainPage = new AppShell();

		Task.Run(async () => await ApplyMigration());
	}

	private async Task ApplyMigration()
	{
		using var scope = MauiProgram.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Database migrationer anvendt succesfuldt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl under anvendelse af database migrationer: {ex.Message}");
        }
	}

	// protected override Window CreateWindow(IActivationState? activationState)
	// {
	// 	return new Window(new AppShell());
	// }
}