using StudentCabinet.ViewModels;

namespace StudentCabinet;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new StudentViewModel();
    }
}
