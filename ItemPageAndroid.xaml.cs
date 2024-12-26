using PCCE.Model;
using PCCE.ViewModel;
using System.Collections.ObjectModel;

namespace PCCE;

public partial class ItemPageAndroid : ContentPage
{
    readonly ItemViewModel iv;
    public ItemPageAndroid(ItemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        iv = vm;
    }

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var cv = (CollectionView)sender;
        if (cv.SelectedItem == null)
            return;
        InventoryItem selected = (InventoryItem)cv.SelectedItem;
        ItemIDEntry.Text = selected.ItemID.ToString();
        ItemNumberEntry.Text = selected.ItemNum.ToString();
    }

    private void ItemIDEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue != null)
        {
            if (!int.TryParse(e.NewTextValue, out _))
            {
                ItemIDEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }

            if (!string.IsNullOrEmpty(ItemIDEntry.Text))
            {
                HintIName.Text = Utilities.GetItemIName(Utilities.ConvertToUint(ItemIDEntry.Text));
                HintDisplayName.Text = Utilities.GetItemDisplayName(Utilities.ConvertToUint(ItemIDEntry.Text));
            }
        }
    }

    private void ItemNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(e.NewTextValue, out _))
        {
            ItemNumberEntry.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        }
    }

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        /*
        var searchTerm = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = string.Empty;
        }

        searchTerm = searchTerm.ToLowerInvariant();

        var filteredItem = iv.SourceItem.Where(item => item.ItemID.ToString().Contains(searchTerm) ||
                                                        item.ItemIName.ToLowerInvariant().Contains(searchTerm) ||
                                                        item.ItemDisplayName.ToLowerInvariant().Contains(searchTerm)
                                                        ).ToList();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredItem = iv.SourceItem.ToList();
        }

        foreach (var item in iv.SourceItem)
        {
            if (!filteredItem.Contains(item))
            {
                iv.InventoryItems.Remove(item);
            }
            else if (!iv.InventoryItems.Contains(item))
            {
                iv.InventoryItems.Add(item);
            }
        }

        //Still lag as f

        */

        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            ItemCollectionView.ItemsSource = iv.InventoryItems;
        }
        else
        {
            ItemCollectionView.ItemsSource = iv.InventoryItems.Where(i => i.ItemID.ToString().Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) ||
                                                                          i.ItemIName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) ||
                                                                          i.ItemDisplayName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    private void SetBtn_Clicked(object sender, EventArgs e)
    {
        //ItemCollectionView.ScrollTo(ItemCollectionView.SelectedItem, null, ScrollToPosition.Center, false);
    }

    private async void OnChangeAllClicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Change All", "Are you sure you want to change all items?", "Yes", "No");
        if (answer)
        {
            using var exclusiveItemIDsStream = await FileSystem.OpenAppPackageFileAsync("List/ExclusiveItemIDs.txt");
            using var exclusiveItemIDsReader = new StreamReader(exclusiveItemIDsStream);
            foreach (var item in iv.InventoryItems)
            {
                if (!exclusiveItemIDsReader.EndOfStream)
                {
                    string? exclusiveItemID = exclusiveItemIDsReader.ReadLine();
                    if (exclusiveItemID != null)
                    {
                        item.ItemID = Utilities.ConvertToUint(exclusiveItemID);
                    }
                    continue;    
                }
                exclusiveItemIDsReader.Close();
                exclusiveItemIDsStream.Close();
                break;
            }
        }
    }
}