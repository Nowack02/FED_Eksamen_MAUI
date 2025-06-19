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

    // Sørger for at indlæse data, når siden vises
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ExamDetailsViewModel vm)
        {
            await vm.LoadDetailsCommand.ExecuteAsync(null);
        }
    }
}
