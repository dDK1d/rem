namespace rem;

public partial class AddReminderPage : ContentPage
{
    private Action<Reminder> addReminderAction;
    public AddReminderPage(Action<Reminder> addReminderAction)
	{
		InitializeComponent();
        this.addReminderAction = addReminderAction;
    }

    private async void OnAddReminderClicked(object sender, EventArgs e)
    {
        DateTime reminderDate = datePicker.Date.Add(timePicker.Time);
        string message = messageEntry.Text;

        if (string.IsNullOrWhiteSpace(message))
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите сообщение напоминания.", "OK");
            return;
        }

        var reminderTimeSpan = reminderDate - DateTime.Now;

        if (reminderTimeSpan.TotalMilliseconds > 0)
        {
            var reminder = new Reminder { Message = message, ReminderTime = reminderDate };
            addReminderAction(reminder);
            await Navigation.PopModalAsync();  // Закрываем модальное окно
        }
        else
        {
            await DisplayAlert("Ошибка", "Дата и время должны быть в будущем.", "OK");
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(); // Закрывает модальное окно
    }
}