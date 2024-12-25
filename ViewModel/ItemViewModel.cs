using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCCE.Model;
using System.Collections.ObjectModel;

namespace PCCE.ViewModel
{
    [QueryProperty("InputItemId", "ID")]
    public partial class ItemViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<InventoryItem> inventoryItems;

        [ObservableProperty]
        public InventoryItem? itemSelected;

        [ObservableProperty]
        string? inputItemId;

        [ObservableProperty]
        string? inputItemNum;

        public ItemViewModel()
        {
            inventoryItems = [];
            Utilities.SetPlayerItemViewModel(this);
            Utilities.DeployPlayerItem();
        }

        public void ClearItem()
        {
            InventoryItems.Clear();
            ItemSelected = null;
        }

        [RelayCommand]
        public void Add(InventoryItem item)
        {
            InventoryItems?.Add(item);
        }

        [RelayCommand]
        public async Task Set()
        {
            if (ItemSelected == null) return;
            if (ItemSelected.ItemID == 7000002)
            {
                await Shell.Current.DisplayAlert("Locked!", "Complete Ticket is locked in Item View.", "OK");
                return;
            }
            else if (ItemSelected.ItemID == 1301048)
            {
                await Shell.Current.DisplayAlert("Locked!", "Gold Treat is locked in Item View.", "OK");
                return;
            }

            if (ItemSelected.PutPlace != 0)
            {
                await Shell.Current.DisplayAlert("Locked!", "Item in use!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InputItemId) || string.IsNullOrWhiteSpace(InputItemNum)) return;

            ItemSelected.ItemID = Utilities.ConvertToUint(InputItemId);
            ItemSelected.ItemNum = Utilities.ConvertToUshort(InputItemNum);

            ItemSelected.ItemIName = Utilities.GetItemIName(ItemSelected.ItemID);
            ItemSelected.ItemDisplayName = Utilities.GetItemDisplayName(ItemSelected.ItemID);
            ItemSelected.SetImage();
            ItemSelected.HasChanged = true;

        }

        [RelayCommand]
        async Task Lookup()
        {
            if (string.IsNullOrEmpty(InputItemId))
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    await Shell.Current.GoToAsync(nameof(LookupPageAndroid));
                }
                else
                {
                    await Shell.Current.GoToAsync(nameof(LookupPage));
                }
            }
            else
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    await Shell.Current.GoToAsync($"{nameof(LookupPageAndroid)}?ID={InputItemId}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(LookupPage)}?ID={InputItemId}");
                }
            }
        }
    }
}
