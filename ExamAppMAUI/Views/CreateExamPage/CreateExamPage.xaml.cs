using ExamAppMAUI.ViewModels;

namespace ExamAppMAUI.Views;

public partial class CreateExamPage : ContentPage
{
    public CreateExamPage(CreateExamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}