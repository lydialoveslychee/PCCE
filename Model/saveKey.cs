using CommunityToolkit.Mvvm.ComponentModel;

namespace PCCE.Model
{
    public partial class saveKey : ObservableObject
    {
        public uint keyNum;

        public uint keyLength;

        public string keyName;

        public ulong binSize;

        public ulong binOffset;
    }
}
