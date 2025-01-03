﻿using CommunityToolkit.Maui.Storage;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PCCE
{
    public partial class MainPage : ContentPage
    {
        readonly string WindowsVersion = "v3.4";
        readonly string AndroidVersion = "v3.0";

        readonly Utilities U = new();
        readonly IFileSaver fileSaver;
        readonly CancellationTokenSource cancellationTokenSource = new();
        readonly string SaveFileName = "458ed7a124b23c5066398a3d366bc066e219f4353611cbe00c8d6b57cdef79ab";
        //readonly Timer UpdateTimer;

        private async Task LoadFileAsync(FileResult result)
        {
            using var FileStream = await result.OpenReadAsync();
            using var reader = new BinaryReader(FileStream);
            byte[] fileBytes = reader.ReadBytes((int)FileStream.Length);

            AnimatedProgressBar.IsVisible = true;

#if ANDROID
            _ = DisplayAlert("Now Loading", "Processing save file. Please wait.\n" +
                             "Larger save file might take up to a minute!", "OK");
#endif

            await U.Decrypt(fileBytes);

            //UpdateTimer.Stop();

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

            if (U.BellsLocation > 0)
            {
                BellsEntry.IsEnabled = true;
                BellsEntry.Text = U.Bells.ToString();
                if (BellsLabel.Text.Contains('\n'))
                {
                    int index = BellsLabel.Text.IndexOf('\n');
                    BellsLabel.Text = BellsLabel.Text.Substring(0, index);
                }
            }
            else
            {
                BellsEntry.Text = "";
                BellsEntry.IsEnabled = false;
                if (!BellsLabel.Text.Contains('\n'))
                {
                    BellsLabel.Text += "\n(Absolute Zero)";
                }
            }

            if (U.NewLeafTicketsLocation > 0)
            {
                LeafTokensEntry.IsEnabled = true;
                LeafTokensEntry.Text = U.NewLeafTickets.ToString();
                if (LeafTokensLabel.Text.Contains('\n'))
                {
                    int index = LeafTokensLabel.Text.IndexOf('\n');
                    LeafTokensLabel.Text = LeafTokensLabel.Text.Substring(0,index);
                }
            }
            else
            {
                LeafTokensEntry.Text = "";
                LeafTokensEntry.IsEnabled = false;
                if (!LeafTokensLabel.Text.Contains('\n'))
                {
                    LeafTokensLabel.Text += "\n(Absolute Zero)";
                }
            }


            if (U.CompleteTicketsLocation > 0)
            {
                CompleteTicketsEntry.IsEnabled = true;
                CompleteTicketsEntry.Text = U.CompleteTickets.ToString();
                if (CompleteTicketsLabel.Text.Contains('\n'))
                {
                    int index = CompleteTicketsLabel.Text.IndexOf('\n');
                    CompleteTicketsLabel.Text = CompleteTicketsLabel.Text.Substring(0, index);
                }
            }
            else
            {
                CompleteTicketsEntry.Text = "";
                CompleteTicketsEntry.IsEnabled = false;
                if (!CompleteTicketsLabel.Text.Contains('\n'))
                {
                    CompleteTicketsLabel.Text += "\n(Missing Item)";
                }
            }

            if (U.GoldTreatsLocation > 0)
            {
                GoldTreatsEntry.IsEnabled = true;
                GoldTreatsEntry.Text = U.GoldTreats.ToString();
                if (GoldTreatsLabel.Text.Contains('\n'))
                {
                    int index = GoldTreatsLabel.Text.IndexOf('\n');
                    GoldTreatsLabel.Text = GoldTreatsLabel.Text.Substring(0, index);
                }
            }
            else
            {
                GoldTreatsEntry.Text = "";
                GoldTreatsEntry.IsEnabled = false;
                if (!GoldTreatsLabel.Text.Contains('\n'))
                {
                    GoldTreatsLabel.Text += "\n(Missing Item)";
                }
            }

            AnimatedProgressBar.IsVisible = false;
            MoreButton.IsEnabled = true;
            ChangeAllBtn.IsEnabled = true;
        }

        private async Task SaveFileAsync(bool skipEncrypt)
        {
            if (string.IsNullOrWhiteSpace(BellsEntry.Text) && BellsEntry.IsEnabled)
            {
                await DisplayAlert("OMG", "Captain Sum Ting Wong!", "Nani?");
                return;
            }

            bool EncryptResult = await U.Encrypt(skipEncrypt);

            if (!EncryptResult || U.encryptBytes == null)
            {
                await DisplayAlert("OMG", "Captain Sum Ting Wong!", "Nani?");
                return;
            }

            using var stream = new MemoryStream(U.encryptBytes);
            var FileSaveResult = await fileSaver.SaveAsync(SaveFileName, stream, cancellationTokenSource.Token);

            if (FileSaveResult.IsSuccessful)
            {
                await DisplayAlert("Success", "File Saved!", "OK");
            }
            else
            {
                await DisplayAlert("Task failed successfully", "Something else on your mind?", "OK");
            }

            LoadBtn.IsEnabled = true;
            SaveBtn.IsEnabled = true;

            //BellsEntry.IsEnabled = true;
            //LeafTokensEntry.IsEnabled = true;
            //CompleteTicketsEntry.IsEnabled = true;
            //GoldTreatsEntry.IsEnabled = true;

            AnimatedProgressBar.IsVisible = false;
        }

        private const string LowerKey = "lower";
        private const string UpperKey = "upper";

        public MainPage(IFileSaver fileSaver)
        {
            InitializeComponent();
            this.fileSaver = fileSaver;
            BindingContext = this;

            /*
            UpdateTimer = new()
            {
                Interval = 1000
            };
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            */
            _ = AskForFilePermission();

            var lowerAnimation = new Animation(v => AnimatedProgressBar.LowerRangeValue = (float)v, -0.4, 1.0);
            var upperAnimation = new Animation(v => AnimatedProgressBar.UpperRangeValue = (float)v, 0.0, 1.4);

            lowerAnimation.Commit(this, LowerKey, length: 1000, easing: Easing.CubicInOut, repeat: () => true);
            upperAnimation.Commit(this, UpperKey, length: 1000, easing: Easing.CubicInOut, repeat: () => true);

#if DEBUG
            DecryptBtn.IsEnabled = true;
            EncryptBtn.IsEnabled = true;
            DecryptBtn.IsVisible = true;
            EncryptBtn.IsVisible = true;
#endif
            VersionLabel.Text = WindowsVersion;
#if ANDROID
            VersionLabel.Text = AndroidVersion;
#endif
        }

        public static async Task AskForFilePermission()
        {
            _ = await Permissions.RequestAsync<Permissions.StorageRead>();
            _ = await Permissions.RequestAsync<Permissions.StorageWrite>();
        }

        private void UpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (U.LoadProgress > 1)
                    AnimatedProgressBar.Progress = 1;
                else if (U.LoadProgress <= 0)
                    AnimatedProgressBar.Progress = 0;
                else
                    AnimatedProgressBar.Progress = U.LoadProgress;
                //Debug.Print(U.LoadProgress.ToString());
            });
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Save File" });

            if (result == null) { return; }

            //UpdateTimer.Start();
            LoadBtn.IsEnabled = false;
            SaveBtn.IsEnabled = false;
            BellsEntry.IsEnabled = false;
            LeafTokensEntry.IsEnabled = false;
            CompleteTicketsEntry.IsEnabled = false;
            GoldTreatsEntry.IsEnabled = false;
            MoreButton.IsEnabled = false;
            ChangeAllBtn.IsEnabled = false;
            //AnimatedProgressBar.Progress = 0;

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

            LoadBtn.IsEnabled = false;
            SaveBtn.IsEnabled = false;
            BellsEntry.IsEnabled = false;
            LeafTokensEntry.IsEnabled = false;
            CompleteTicketsEntry.IsEnabled = false;
            GoldTreatsEntry.IsEnabled = false;
            MoreButton.IsEnabled = false;
            ChangeAllBtn.IsEnabled = false;

            AnimatedProgressBar.IsVisible = true;

            await SaveFileAsync(false);
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

        private async void MoreButton_Clicked(object sender, EventArgs e)
        {
            LanguagePicker.IsEnabled = false;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Shell.Current.GoToAsync(nameof(ItemPageAndroid));
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(ItemPage));
            }
        }

        private void LanguagePicker_Loaded(object sender, EventArgs e)
        {
            var value = Preferences.Get("Language", 0);
            var picker = (Picker)sender;
            picker.SelectedIndex = value;
        }

        private async void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex > 0)
            {
                Utilities.language = selectedIndex;
                Preferences.Set("Language", selectedIndex);
            }
            else
            {
                Utilities.language = 0;
                Preferences.Set("Language", 0);
            }

            await Utilities.BuildDictionary();
        }

        private async void DecryptBtn_Clicked(object sender, EventArgs e)
        {
            if (BellsEntry.IsEnabled && !string.IsNullOrEmpty(BellsEntry.Text))
                U.Bells = Utilities.ConvertToUint(BellsEntry.Text);

            if (LeafTokensEntry.IsEnabled && !string.IsNullOrEmpty(LeafTokensEntry.Text))
                U.NewLeafTickets = Utilities.ConvertToUint(LeafTokensEntry.Text);

            if (CompleteTicketsEntry.IsEnabled && !string.IsNullOrEmpty(CompleteTicketsEntry.Text))
                U.CompleteTickets = (ushort)Utilities.ConvertToUint(CompleteTicketsEntry.Text);

            if (GoldTreatsEntry.IsEnabled && !string.IsNullOrEmpty(GoldTreatsEntry.Text))
                U.GoldTreats = (ushort)Utilities.ConvertToUint(GoldTreatsEntry.Text);

            await SaveFileAsync(true);
        }

        private async void EncryptBtn_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions { PickerTitle = "Save File" });

            if (result == null) { return; }

            using var FileStream = await result.OpenReadAsync();
            using var reader = new BinaryReader(FileStream);
            byte[] fileBytes = reader.ReadBytes((int)FileStream.Length);

            U.EncryptOnly(fileBytes);

            await SaveFileAsync(false);
        }

        private async void OnChangeAllClicked(object sender, EventArgs e)
        {
            var changedNum = 0;
            var existedStickersNum = 0;
            try
            {
                var multiSelectDialogPage = new MultiSelectDialogPage();
                await Navigation.PushModalAsync(multiSelectDialogPage);

                // Wait for the modal page to be dismissed
                var tcs = new TaskCompletionSource<bool>();
                multiSelectDialogPage.Disappearing += (s, e) => tcs.SetResult(true);
                await tcs.Task;

                var selectedExclusive = multiSelectDialogPage.SelectedExclusive;
                var selectedStickers = multiSelectDialogPage.SelectedStickers;

                if (selectedExclusive.Count == 0 && selectedStickers.Count == 0)
                {
                    await DisplayAlert("Nothing Happened", "No items changed", "OK");
                    return;
                }

                var answer = await DisplayAlert("Attention", "You are going to change " + selectedExclusive.Count + " items and " + selectedStickers.Count + " stickers. Please make sure that you have enough unwanted items to replace.", "I have enough items", "Cancel");
                if (!answer)
                {
                    return;
                }

                if (Utilities.inventoryItemList == null)
                {
                    throw new Exception("InventoryItems is null");
                }
                List<Model.InventoryItem> sorted = Utilities.inventoryItemList.OrderByDescending(d => d.ItemDate).ToList();
                List<string> newStickers = new();
                for (int i = 0; i < selectedStickers.Count; i++)
                {
                    List<Model.InventoryItem> existedStickers = sorted.Where(x => x.ItemID == Utilities.ConvertToUint(selectedStickers[i])).ToList();
                    if (existedStickers.Count > 0)
                    {
                        existedStickers[0].ItemNum += 99;
                        existedStickers[0].HasChanged = true;
                        existedStickersNum++;
                        continue;
                    } 
                    else
                    {
                        newStickers.Add(selectedStickers[i]);
                    }
                }

                int sortListPos = 0;
                for (int i = 0; i < selectedExclusive.Count; i++)
                {
                    while (sorted.Count > sortListPos && sorted[sortListPos].HasChanged)
                    {
                        sortListPos++;
                    }
                    if (sorted.Count <= sortListPos)
                    {
                        throw new Exception("No enough unchanged inventory items to change (please buy more!)");
                    }
                    if (sorted[sortListPos] == null)
                    {
                        throw new Exception("An item in InventoryItems is null");
                    }
                    sorted[sortListPos].ItemID = Utilities.ConvertToUint(selectedExclusive[i]);
                    sorted[sortListPos].ItemNum = 1;
                    sorted[sortListPos].ItemIName = Utilities.GetItemIName(sorted[sortListPos].ItemID);   
                    sorted[sortListPos].ItemDisplayName = Utilities.GetItemDisplayName(sorted[sortListPos].ItemID);   
                    sorted[sortListPos].SetImage();
                    sorted[sortListPos].HasChanged = true;
                    sortListPos++;
                    changedNum++;
                }

                for (int i = 0; i < newStickers.Count; i++)
                {
                    while (sorted.Count > sortListPos && sorted[sortListPos].HasChanged)
                    {
                        sortListPos++;
                    }
                    if (sorted.Count <= sortListPos)
                    {
                        throw new Exception("No enough unchanged inventory items to change (please buy more!)");
                    }
                    if (sorted[sortListPos] == null)
                    {
                        throw new Exception("An item in InventoryItems is null");
                    }
                    sorted[sortListPos].ItemID = Utilities.ConvertToUint(newStickers[i]);
                    sorted[sortListPos].ItemNum = 99;
                    sorted[sortListPos].ItemIName = Utilities.GetItemIName(sorted[sortListPos].ItemID);   
                    sorted[sortListPos].ItemDisplayName = Utilities.GetItemDisplayName(sorted[sortListPos].ItemID);   
                    sorted[sortListPos].SetImage();
                    sorted[sortListPos].HasChanged = true;
                    sortListPos++;
                    changedNum++;
                }
            
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            await DisplayAlert("Success", changedNum + " items have been changed and " + existedStickersNum + " existed stickers have been refilled!", "OK");
        }
    }

}
