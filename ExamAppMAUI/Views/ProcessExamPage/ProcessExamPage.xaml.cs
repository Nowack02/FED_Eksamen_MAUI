using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class ProcessExamPage : ContentPage
{
    public ProcessExamPage(ProcessExamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProcessExamViewModel vm && vm.LoadExamProcessCommand.CanExecute(null))
        {
            await vm.LoadExamProcessCommand.ExecuteAsync(null);
        }
    }
}
