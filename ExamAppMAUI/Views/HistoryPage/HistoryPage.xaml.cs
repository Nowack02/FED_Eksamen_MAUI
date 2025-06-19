using ExamAppMAUI.ViewModels;
namespace ExamAppMAUI.Views;

public partial class HistoryPage : ContentPage
{
	public HistoryPage(HistoryViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is HistoryViewModel vm && vm.LoadCompletedExamsCommand.CanExecute(null))
		{
			await vm.LoadCompletedExamsCommand.ExecuteAsync(null);
		}
	}
}