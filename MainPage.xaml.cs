using CommunityToolkit.Maui.Storage;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PCCE
{
    public partial class MainPage : ContentPage
    {
        readonly Utilities U = new();
        readonly IFileSaver fileSaver;
        readonly CancellationTokenSource cancellationTokenSource = new();
        readonly string SaveFileName = "458ed7a124b23c5066398a3d366bc066e219f4353611cbe00c8d6b57cdef79ab";
        readonly Timer UpdateTimer;

        private async Task LoadFileAsync(FileResult result)
        {
            using var FileStream = await result.OpenReadAsync();
            using var reader = new BinaryReader(FileStream);
            byte[] fileBytes = reader.ReadBytes((int)FileStream.Length);

            AnimatedProgressBar.IsVisible = true;

            //_ = DisplayAlert("Now Loading", "Processing save file. Please wait.", "OK");

            await U.Decrypt(fileBytes);

            UpdateTimer.Stop();

            LoadBtn.IsEnabled = true;
            SaveBtn.IsEnabled = true;

            if (!U.DecryptSuccess)
            {
                await DisplayAlert("OMG", "Not a ACPC save file or decryption failed. Aborting as a precaution!", "OK");
                return;
            }
            else
            {
                await DisplayAlert("Success", "Save File Loaded!", "OK");
            }

            BellsEntry.IsEnabled = true;
            LeafTokensEntry.IsEnabled = true;
            BellsEntry.Text = U.Bells.ToString();
            LeafTokensEntry.Text = U.NewLeafTickets.ToString();

            if (U.CompleteTicketsLocation > 0)
            {
                CompleteTicketsEntry.IsEnabled = true;
                CompleteTicketsEntry.Text = U.CompleteTickets.ToString();
            }
            else
            {
                CompleteTicketsEntry.IsEnabled = false;
                CompleteTicketsEntry.Text = "";
            }

            if (U.GoldTreatsLocation > 0)
            {
                GoldTreatsEntry.IsEnabled = true;
                GoldTreatsEntry.Text = U.GoldTreats.ToString();
            }
            else
            {
                GoldTreatsEntry.IsEnabled = false;
                GoldTreatsEntry.Text = "";
            }

            AnimatedProgressBar.IsVisible = false;
            MoreButton.IsEnabled = true;
        }

        public MainPage(IFileSaver fileSaver)
        {
            InitializeComponent();
            this.fileSaver = fileSaver;
            BindingContext = this;

            UpdateTimer = new()
            {
                Interval = 200
            };
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private void UpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (U.LoadProgress > 1)
                    AnimatedProgressBar.Progress = 1;
                else
                    AnimatedProgressBar.Progress = U.LoadProgress;
                //Debug.Print(U.LoadProgress.ToString());
            });
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Save File" });

            if (result == null) { return; }

            UpdateTimer.Start();
            LoadBtn.IsEnabled = false;
            SaveBtn.IsEnabled = false;
            BellsEntry.IsEnabled = false;
            LeafTokensEntry.IsEnabled = false;
            CompleteTicketsEntry.IsEnabled = false;
            GoldTreatsEntry.IsEnabled = false;
            MoreButton.IsEnabled = false;

            await LoadFileAsync(result);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (BellsEntry.IsEnabled && !string.IsNullOrEmpty(BellsEntry.Text))
                U.Bells = Utilities.ConvertToUint(BellsEntry.Text);

            if (LeafTokensEntry.IsEnabled && !string.IsNullOrEmpty(LeafTokensEntry.Text))
                U.NewLeafTickets = Utilities.ConvertToUint(LeafTokensEntry.Text);

            if (CompleteTicketsEntry.IsEnabled && !string.IsNullOrEmpty(CompleteTicketsEntry.Text))
                U.CompleteTickets = (ushort)Utilities.ConvertToUint(CompleteTicketsEntry.Text);

            if (GoldTreatsEntry.IsEnabled && !string.IsNullOrEmpty(GoldTreatsEntry.Text))
                U.GoldTreats = (ushort)Utilities.ConvertToUint(GoldTreatsEntry.Text);

            byte[]? encryptSave = U.Encrypt();
            if (encryptSave == null || string.IsNullOrWhiteSpace(BellsEntry.Text) || string.IsNullOrWhiteSpace(LeafTokensEntry.Text))
            {
                await DisplayAlert("OMG", "Captain Sum Ting Wong!", "Nani?");
                return;
            }

            using var stream = new MemoryStream(encryptSave);
            var FileSaveResult = await fileSaver.SaveAsync(SaveFileName, stream, cancellationTokenSource.Token);

            if (FileSaveResult.IsSuccessful)
            {
                await DisplayAlert("Success", "File Saved!", "OK");
            }
            else
            {
                await DisplayAlert("Task failed successfully", "Something else on your mind?", "OK");
            }
        }

        private void BellsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out _))
            {
                BellsEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }
        }

        private void LeafTokensEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out _))
            {
                LeafTokensEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }
        }

        private void CompleteTicketsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out _))
            {
                CompleteTicketsEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }
        }

        private void GoldTreatsEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out _))
            {
                GoldTreatsEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }
        }

        private void MoreButton_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(ItemPage));
        }
    }

}
