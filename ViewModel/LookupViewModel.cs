using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCCE.Model;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace PCCE.ViewModel
{
    [QueryProperty("IncomingItemID", "ID")]
    public partial class LookupViewModel : ObservableObject
    {
        [ObservableProperty]
        string? incomingItemID;

        [ObservableProperty]
        public ObservableCollection<ListedItem> listedItems;

        [ObservableProperty]
        public ListedItem? itemSelected;

        public LookupViewModel()
        {
            listedItems = [];
            Utilities.SetListedItemViewModel(this);
            Utilities.DeployListedItem();
        }

        [RelayCommand]
        public void Add(ListedItem item)
        {
            ListedItems?.Add(item);
        }

        [RelayCommand]
        async Task Select()
        {
            if (ItemSelected == null)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.GoToAsync($"..?ID={ItemSelected.ItemID}");
            }
        }
    }
}