using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class AddStudentsToExamPage : ContentPage
{
	public AddStudentsToExamPage(AddStudentsToExamViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is AddStudentsToExamViewModel vm)
		{
			await vm.LoadAssignedStudentsCommand.ExecuteAsync(null);
		}
	}
}
