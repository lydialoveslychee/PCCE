using CommunityToolkit.Mvvm.ComponentModel;

namespace PCCE.Model
{
    public partial class ListedItem : ObservableObject
    {
        [ObservableProperty]
        public uint itemID;
        public string? ImagePath { get; set; }

        [ObservableProperty]
        public ImageSource? itemImage;

        [ObservableProperty]
        public string itemIName;
        [ObservableProperty]
        public string itemENName;

        public ListedItem()
        {
            itemID = 0;
            itemIName = "";
            itemENName = "";
            ImagePath = "";
        }

        public void SetImage()
        {
            ItemImage = ImageSource.FromFile("cookie_ticket.png");
        }
    }
}
