// Fil: Views/HistoryPage/ExamDetailsPage.xaml.cs
using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class ExamDetailsPage : ContentPage
{
    public ExamDetailsPage(ExamDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ExamDetailsViewModel vm)
        {
            await vm.LoadDetailsCommand.ExecuteAsync(null);
        }
    }
}
