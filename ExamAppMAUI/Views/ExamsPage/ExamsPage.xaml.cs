using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class ExamsPage : ContentPage
{
	public ExamsPage(ExamsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ExamsViewModel vm && vm.LoadExamsCommand.CanExecute(null))
		{
			await vm.LoadExamsCommand.ExecuteAsync(null);
		}
	}
}
