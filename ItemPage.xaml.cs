using PCCE.Model;
using PCCE.ViewModel;

namespace PCCE;

public partial class ItemPage : ContentPage
{
    readonly ItemViewModel iv;
    public ItemPage(ItemViewModel vm)
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
                HintENName.Text = Utilities.GetItemENName(Utilities.ConvertToUint(ItemIDEntry.Text));
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
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            ItemCollectionView.ItemsSource = iv.InventoryItems;
        }
        else
        {
            ItemCollectionView.ItemsSource = iv.InventoryItems.Where(i => i.ItemID.ToString().Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) ||
                                                                          i.ItemIName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) ||
                                                                          i.ItemENName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    private void SetBtn_Clicked(object sender, EventArgs e)
    {
        //ItemCollectionView.ScrollTo(ItemCollectionView.SelectedItem, null, ScrollToPosition.Center, false);
    }
}