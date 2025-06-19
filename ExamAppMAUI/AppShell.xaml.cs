using ExamAppMAUI.Views;

namespace ExamAppMAUI;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(CreateExamPage), typeof(CreateExamPage));
		Routing.RegisterRoute(nameof(ExamsPage), typeof(ExamsPage));
		Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
		Routing.RegisterRoute(nameof(ProcessExamPage), typeof(ProcessExamPage));
		Routing.RegisterRoute(nameof(AddStudentsToExamPage), typeof(AddStudentsToExamPage));
	}
}
