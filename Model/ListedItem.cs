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
        public string itemDisplayName;

        public ListedItem()
        {
            itemID = 0;
            itemIName = "";
            itemDisplayName = "";
            ImagePath = "";
        }

        public void SetImage()
        {
            ImagePath = "Images\\" + ItemIName + ".png";
            if (File.Exists(ImagePath))
            {
                ItemImage = ImageSource.FromFile("Images\\" + ItemIName + ".png");
            }
            else
            {
                ItemImage = ImageSource.FromFile("cookie_ticket.png");
            }
        }
    }
}
