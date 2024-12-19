using CommunityToolkit.Mvvm.ComponentModel;

namespace PCCE.Model
{
    public partial class InventoryItem : ObservableObject
    {
        [ObservableProperty]
        public string itemIName;
        [ObservableProperty]
        public string itemENName;
        public string? ImagePath { get; set; }
        [ObservableProperty]
        public uint itemID;
        [ObservableProperty]
        public ushort itemNum;
        [ObservableProperty]
        public ImageSource? itemImage;
        public uint ItemDate { get; set; }

        [ObservableProperty]
        public string normalDate;
        public byte IsChecked { get; set; }
        public string? GUID { get; set; }
        public byte PutPlace { get; set; }
        public byte IsLock { get; set; }

        public bool HasChanged { get; set; }
        public int IDLocation { get; set; }
        public int NumLocation { get; set; }
        public int DateLocation { get; set; }

        public InventoryItem()
        {
            itemIName = "MissingName";
            itemENName = "MissingName";
            ImagePath = "MissingPath";
            itemID = 0;
            itemNum = 0;
            ItemDate = 0;
            NormalDate = "";
            IsChecked = 0;
            GUID = "MissingGUID";
            PutPlace = 0;
            IsLock = 0;
            HasChanged = false;
            IDLocation = 0;
            NumLocation = 0;
            itemImage = null;
        }
        public void SetImage()
        {
            ItemImage = ImageSource.FromFile("cookie_ticket.png");
        }
    }
}
