using StudentCabinet.ViewModels;

namespace StudentCabinet.Views;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage()
    {
        InitializeComponent();
        BindingContext = new StudentDetailViewModel();
    }
}
