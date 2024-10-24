using System.Collections.ObjectModel;

namespace rem
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Reminder> reminders;

        [Obsolete]

        public MainPage()
        {
            InitializeComponent();
            reminders = new ObservableCollection<Reminder>();
            RemindersListView.ItemsSource = reminders;
            StartReminderChecking();
        }

        private async void StartReminderChecking()
        {
            while (true) // Бесконечный цикл для проверки
            {
                CheckForReminders();
                await Task.Delay(TimeSpan.FromMinutes(1)); // Задержка в 1 минуту
            }
        }

        private async void OnAddReminderClicked(object sender, EventArgs e)
        {
            var addReminderPage = new AddReminderPage(AddReminder);
            await Navigation.PushModalAsync(addReminderPage);
        }

        private void AddReminder(Reminder reminder)
        {
            reminders.Add(reminder);
        }

        private void RemindersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var selectedReminder = e.Item as Reminder;
                reminders.Remove(selectedReminder);
            }
        }

        private void CheckForReminders()
        {
            var now = DateTime.Now;

            // Проверяем напоминания и выводим сообщение
            var dueReminders = reminders.Where(r => r.ReminderTime <= now).ToList();
            foreach (var reminder in dueReminders)
            {
                DisplayReminderMessage(reminder);
                reminders.Remove(reminder); // Удаляем после отображения
            }
        }

        private async void DisplayReminderMessage(Reminder reminder)
        {
            await Application.Current.MainPage.DisplayAlert("Напоминание", reminder.Message, "OK");
        }
    }

    public class Reminder
    {
        public string Message { get; set; }
        public DateTime ReminderTime { get; set; }
    }
}
