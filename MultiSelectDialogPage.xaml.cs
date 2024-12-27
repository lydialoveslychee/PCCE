using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PCCE
{
    public partial class MultiSelectDialogPage : ContentPage
    {
        public List<string> SelectedExclusiveTypes { get; private set; } = new List<string>();

        public MultiSelectDialogPage()
        {
            InitializeComponent();
        }

        private void OnSelectAllButtonClicked(object sender, EventArgs e)
        {
            bool selectAll = SelectAllButton.Text == "Select All (404)";

            ChocolateBarCheckBox.IsChecked = selectAll;
            EeveeCheckBox.IsChecked = selectAll;
            EndOfServiceCheckBox.IsChecked = selectAll;
            GlowingToyGiftsCheckBox.IsChecked = selectAll;
            GoogleEventCheckBox.IsChecked = selectAll;
            OtherExclusivesCheckBox.IsChecked = selectAll;
            SanrioCheckBox.IsChecked = selectAll;
            SeasonGreetingCheckBox.IsChecked = selectAll;
            SousouCheckBox.IsChecked = selectAll;
            Splatoon2CheckBox.IsChecked = selectAll;
            SuperMarioCheckBox.IsChecked = selectAll;
            NyoCheckBox.IsChecked = selectAll;

            SelectAllButton.Text = selectAll ? "Unselect All" : "Select All (404)";
            SelectAllButton.BackgroundColor = selectAll ? Colors.LightGrey : Colors.Blue;
        }

        private async void OnOkButtonClicked(object sender, EventArgs e)
        {
            if (ChocolateBarCheckBox.IsChecked) SelectedExclusiveTypes.Add("ChocolateBar");
            if (EeveeCheckBox.IsChecked) SelectedExclusiveTypes.Add("Eevee");
            if (EndOfServiceCheckBox.IsChecked) SelectedExclusiveTypes.Add("EndOfService");
            if (GlowingToyGiftsCheckBox.IsChecked) SelectedExclusiveTypes.Add("GlowingToyGifts");
            if (GoogleEventCheckBox.IsChecked) SelectedExclusiveTypes.Add("GoogleEvent");
            if (OtherExclusivesCheckBox.IsChecked) SelectedExclusiveTypes.Add("OtherExclusives");
            if (SanrioCheckBox.IsChecked) SelectedExclusiveTypes.Add("Sanrio");
            if (SeasonGreetingCheckBox.IsChecked) SelectedExclusiveTypes.Add("SeasonGreeting");
            if (SousouCheckBox.IsChecked) SelectedExclusiveTypes.Add("Sousou");
            if (Splatoon2CheckBox.IsChecked) SelectedExclusiveTypes.Add("Splatoon2");
            if (SuperMarioCheckBox.IsChecked) SelectedExclusiveTypes.Add("SuperMario");
            if (NyoCheckBox.IsChecked) SelectedExclusiveTypes.Add("NewYearOmikugi");

            await Navigation.PopModalAsync();
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}