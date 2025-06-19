using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class CreateExamPage : ContentPage
{
    public CreateExamPage(CreateExamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnCreateExamClicked(object sender, EventArgs e)
    {
        if (this.BindingContext is CreateExamViewModel viewModel)
        {
            await viewModel.CreateExamCommand.ExecuteAsync(null);

            if (string.IsNullOrEmpty(viewModel.ErrorMessage))
            {
                await DisplayAlert("Succes", "Eksamensoprettelse lykkedes!", "OK");
            }
        }
    }
}