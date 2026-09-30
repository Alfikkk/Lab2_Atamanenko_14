using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using StudentCabinet.Models;

namespace StudentCabinet.ViewModels;

public class StudentDetailViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private Student _student = new();

    public string FullName
    {
        get => _student.FullName;
        set
        {
            if (_student.FullName != value)
            {
                _student.FullName = value;
                OnPropertyChanged();
            }
        }
    }

    public string Group
    {
        get => _student.Group;
        set
        {
            if (_student.Group != value)
            {
                _student.Group = value;
                OnPropertyChanged();
            }
        }
    }

    public double AverageScore
    {
        get => _student.AverageScore;
        set
        {
            if (_student.AverageScore != value)
            {
                _student.AverageScore = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand GoBackCommand { get; }
    public ICommand SaveCommand { get; }

    public StudentDetailViewModel()
    {
        GoBackCommand = new Command(async () =>
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Shell.Current.GoToAsync("..")));

        SaveCommand = new Command(async () =>
        {
            var parameters = new Dictionary<string, object>
            {
                { "UpdatedStudent", _student }
            };

            await MainThread.InvokeOnMainThreadAsync(async () =>
                await Shell.Current.GoToAsync("..", parameters));
        });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SelectedStudent", out var value)
            && value is Student student)
        {
            _student = student;
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Group));
            OnPropertyChanged(nameof(AverageScore));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
