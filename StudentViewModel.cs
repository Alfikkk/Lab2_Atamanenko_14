using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using StudentCabinet.Models;

namespace StudentCabinet.ViewModels;

public class StudentViewModel : INotifyPropertyChanged, IQueryAttributable
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
                ((Command)AddStudentCommand).ChangeCanExecute();
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
                OnPropertyChanged(nameof(IsExcellent));
            }
        }
    }

    public bool IsExcellent => _student.AverageScore >= 4.0;

    public ObservableCollection<Student> Students { get; } = new();
    public ICommand AddStudentCommand { get; }
    public ICommand OpenDetailsCommand { get; }

    public StudentViewModel()
    {
        AddStudentCommand = new Command(AddStudent, CanAddStudent);
        OpenDetailsCommand = new Command<Student>(async (student) => await OpenDetailsAsync(student));
    }

    private void AddStudent()
    {
        Students.Add(new Student
        {
            FullName = FullName,
            Group = Group,
            AverageScore = AverageScore
        });

        FullName = string.Empty;
        Group = string.Empty;
        AverageScore = 0;
    }

    private bool CanAddStudent() => !string.IsNullOrWhiteSpace(FullName);

    private async Task OpenDetailsAsync(Student student)
    {
        if (student is null) return;

        var parameters = new Dictionary<string, object>
        {
            { "SelectedStudent", student }
        };

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await Task.Delay(50);
            await Shell.Current.GoToAsync("studentdetail", parameters);
        });
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("UpdatedStudent", out var value)
            && value is Student updated)
        {
            var existing = Students.FirstOrDefault(s =>
                s.FullName == updated.FullName && s.Group == updated.Group);

            if (existing != null)
            {
                existing.AverageScore = updated.AverageScore;

                var index = Students.IndexOf(existing);
                Students[index] = existing;
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
