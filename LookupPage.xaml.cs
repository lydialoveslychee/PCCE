using PCCE.Model;
using PCCE.ViewModel;

namespace PCCE;

public partial class LookupPage : ContentPage
{
    readonly LookupViewModel viewModel;
    int incomingIndex = 0;
    public LookupPage(LookupViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        viewModel = vm;
    }

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            LookupCollectionView.ItemsSource = viewModel.ListedItems;
        }
        else
        {
            LookupCollectionView.ItemsSource = viewModel.ListedItems.Where(i => i.ItemID.ToString().Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) ||
                                                                                i.ItemIName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase) || 
                                                                                i.ItemDisplayName.Contains(e.NewTextValue, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    private void LookupCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void Incoming_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            for (int i = 0; i < viewModel.ListedItems.Count; i++)
            {
                if (e.NewTextValue == viewModel.ListedItems[i].ItemID.ToString())
                {
                    incomingIndex = i;
                    break;
                }
            }
        }
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        LookupCollectionView.ScrollTo(incomingIndex, -1, ScrollToPosition.Center, false);
        LookupCollectionView.SelectedItem = 0;
        viewModel.ItemSelected = null;
    }
}