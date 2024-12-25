using PCCE.Model;
using PCCE.ViewModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace PCCE
{
    class Utilities
    {
        private static readonly byte[] xorKey = [ 0x78, 0xAD, 0x33, 0x1C, 0x16, 0x9A, 0xB0, 0x96, 0xC9, 0x95, 0x44, 0xB3, 0x24, 0x91, 0x10, 0x66, 0x35, 0xB5, 0xF2, 0x3B,
                    0xF6, 0x68, 0xEA, 0x1B, 0x95, 0xBB, 0x98, 0xB9, 0x39, 0x05, 0xB8, 0x48, 0xA3, 0xBB, 0xB1, 0x79, 0x12, 0x32, 0xAF, 0x27,
                    0x5B, 0x05, 0x83, 0x88, 0x4F, 0xD6, 0xB7, 0x58, 0x31, 0x98, 0xF7, 0xA2, 0xB6, 0x54, 0x2D, 0x21, 0xC1, 0x3A, 0xE1, 0xA0,
                    0x3B, 0xAC, 0xE2, 0xA8, 0xCD, 0x7F, 0x27, 0x98, 0xDA, 0x55, 0x26, 0x33, 0xCC, 0x74, 0xEB, 0x38, 0x90, 0x91, 0xE9, 0xBC,
                    0xA4, 0xE6, 0x5F, 0xD1, 0xD7, 0xF1, 0x91, 0xC0, 0xBC, 0xB7, 0x8D, 0x2D, 0xA3, 0x74, 0x4F, 0x0E, 0x35, 0xFA, 0x94, 0x66,
                    0x6E, 0xDE, 0x7C, 0x88, 0x6E, 0x00, 0x4A, 0x9B, 0x6F, 0x0D, 0x74, 0x6A, 0x8E, 0xB5, 0x84, 0x49, 0x13, 0x12, 0x93, 0x2B,
                    0x7F, 0x2A, 0x57, 0x40, 0x7B, 0x80, 0xD2, 0xEB, 0x59, 0x35, 0x86, 0x1E, 0xDE, 0x39, 0xCD, 0xD8, 0xF3, 0x84, 0x11, 0x08,
                    0x8E, 0xEE, 0x92, 0x19, 0x7B, 0x1E, 0xF0, 0xAC, 0x6B, 0x59, 0xA5, 0x1A, 0xB8, 0x86, 0xF5, 0x28, 0x42, 0x02, 0xC6, 0x7B,
                    0xCE, 0x15, 0xC5, 0x07, 0x68, 0xC2, 0xD8, 0xAA, 0xE7, 0x62, 0x4F, 0x2F, 0xB3, 0x07, 0x7C, 0xA6, 0xD3, 0x5A, 0xAA, 0x3C,
                    0xAD, 0x23, 0x3F, 0x75, 0x58, 0xCD, 0x06, 0xC9, 0xD0, 0xA7, 0xA3, 0x3A, 0xD6, 0xAA, 0xF8, 0x58, 0x62, 0xC6, 0xAD, 0x6F,
                    0x52, 0x0D, 0x14, 0xB0, 0xBF, 0x5E, 0xD3, 0xB8, 0x3E, 0xB7, 0x94, 0x33, 0x69, 0x4D, 0xEB, 0x5F, 0x53, 0xDF, 0xDA, 0x52,
                    0x1A, 0x55, 0x00, 0x82, 0x98, 0xC1, 0x19, 0xF7, 0x9E, 0xD0, 0x0E, 0xA3, 0x33, 0x70, 0x5A, 0x61, 0x81, 0x93, 0x1D, 0x9D,
                    0x11, 0x0D, 0xA2, 0xF9, 0x0F, 0xB6, 0x13, 0x6D, 0xBE, 0xA4, 0xE3, 0xA8, 0xA7, 0xC2, 0x7E, 0x14, 0xDB, 0xA4, 0x1A, 0x7F,
                    0x64, 0x36, 0x57, 0x1D, 0xFD, 0x16, 0x41, 0x90, 0x49, 0x90, 0xB2, 0xF8, 0xB2, 0x1C, 0x2E, 0x57, 0x64, 0x44, 0x09, 0xB6,
                    0x2B, 0x11, 0xDE, 0x6F, 0xE7, 0xDA, 0xAE, 0x87, 0xB4, 0xE7, 0x4A, 0xBD, 0xF0, 0xAA, 0xEC, 0x2E, 0x55, 0xED, 0xB6, 0xF2,
                    0x67, 0x71, 0x84, 0x5E, 0xE9, 0x4D, 0x98, 0xAF, 0x4E, 0x21, 0xD2, 0x34, 0x76, 0x43, 0xA7, 0x24, 0x9D, 0xFB, 0x8E, 0xC6,
                    0x18, 0xF6, 0xF5, 0x36, 0x58, 0x06, 0x34, 0x6E, 0x33, 0x17, 0xC2, 0xCD, 0x72, 0xDB, 0xF7, 0xD7, 0xDD, 0x2B, 0xE3, 0xFF,
                    0xB4, 0xE7, 0x19, 0x70, 0x39, 0x6B, 0xA4, 0x8B, 0x94, 0xA3, 0x89, 0x3E, 0x16, 0x15, 0xB4, 0x4E, 0x9A, 0xB5, 0x03, 0x65,
                    0xFE, 0x9E, 0xA3, 0x44, 0x76, 0xFC, 0x2F, 0xD0, 0xCD, 0xF0, 0x52, 0x85, 0xFF, 0x6C, 0xCE, 0x75, 0xF4, 0xE3, 0xD6, 0x5F,
                    0x63, 0xF6, 0xE5, 0xBC, 0x2C, 0xDD, 0xF7, 0x3D, 0xB5, 0x5B, 0x8B, 0xB5, 0x5F, 0xE0, 0xE2, 0x6F, 0x8B, 0xEF, 0x37, 0x2A,
                    0x48, 0x48, 0x5D, 0x75, 0x21, 0x27, 0x71, 0xEA, 0x79, 0x8C, 0x1C, 0x3B, 0x8B, 0x28, 0x22, 0x47, 0x81, 0x81, 0x65, 0xA2,
                    0x9B, 0x33, 0xFC, 0x59, 0x82, 0x47, 0x46, 0xC0, 0x48, 0xB7, 0x76, 0xD5, 0xC2, 0x91, 0xA6, 0x14, 0xC6, 0x44, 0x5B, 0x2B,
                    0x7E, 0x8D, 0x74, 0x65, 0x8D, 0x7F, 0xA4, 0xBF, 0xC2, 0x91, 0x12, 0x02, 0x26, 0xE8, 0x94, 0x78, 0x33, 0xD3, 0xE2, 0x9C,
                    0x43, 0xF9, 0xE2, 0x05, 0x09, 0xB2, 0xE9, 0x7A, 0x6B, 0x9B, 0xF7, 0x60, 0xC7, 0xF0, 0x3F, 0x8D, 0x4B, 0x95, 0x04, 0xBC,
                    0xD5, 0xF2, 0xCD, 0x8D, 0x61, 0x66, 0x00, 0xA4, 0x27, 0x2C, 0x57, 0x52, 0xC4, 0x7F, 0x07, 0xEF, 0x87, 0xA2, 0x24, 0x48,
                    0xD6, 0x90, 0x77, 0x38, 0x93, 0x43, 0x3C, 0x7F, 0xAF, 0x3F, 0x11, 0x87, 0x39, 0x29, 0xE0, 0x4C, 0x6C, 0x55, 0x79, 0xE6,
                    0xE6, 0x7F, 0x10, 0xD7, 0xB1, 0x9B, 0x96, 0xB6, 0xC7, 0xC6, 0x2B, 0x49, 0x00, 0x9B, 0xE0, 0x1E, 0x1E, 0x7C, 0xB1, 0x72,
                    0x73, 0x3F, 0xBA, 0x2D, 0x85, 0x55, 0x4B, 0x60, 0x5A, 0x11, 0xD0, 0xC4, 0xA1, 0x1D, 0x0D, 0xD5, 0x2F, 0x7D, 0x25, 0x09,
                    0x00, 0x5C, 0x65, 0xCD, 0xD4, 0x1F, 0x02, 0xF2, 0x9D, 0x25, 0xCE, 0x01, 0x5B, 0xB6, 0xE5, 0xC1, 0xB8, 0x27, 0x91, 0x77,
                    0x94, 0x03, 0x1C, 0x13, 0x27, 0xE5, 0x9D, 0x54, 0xCB, 0x91, 0xEC, 0x84, 0x4F, 0xB9, 0x70, 0xA8, 0xC3, 0xD6, 0x1E, 0xC8,
                    0xC8, 0xD2, 0x34, 0xDB, 0x8C, 0x63, 0x14, 0x89, 0x08, 0xDB, 0x09, 0x9D, 0x7B, 0x88, 0x76, 0x10, 0x22, 0xA5, 0xE1, 0x56,
                    0xB6, 0xF6, 0x7E, 0xD7, 0x66, 0xB1, 0x1E, 0x75, 0x5E, 0xB1, 0xDC, 0x45, 0x53, 0x13, 0x29, 0x94, 0x56, 0x16, 0xE5, 0xE7,
                    0xB8, 0xBA, 0x1B, 0xE6, 0x68, 0x39, 0xAB, 0x40, 0xA2, 0xAC, 0x70, 0x5A, 0x43, 0x74, 0x80, 0x35, 0x2B, 0x79, 0xC6, 0x96,
                    0x8E, 0xBF, 0x0B, 0x12, 0x4F, 0x50, 0x94, 0x3F, 0xA8, 0xC2, 0xC0, 0xC8, 0x4D, 0x6A, 0xEE, 0x30, 0x3B, 0xFB, 0x9B, 0x14,
                    0xF0, 0xC1, 0x01, 0x38, 0x4E, 0xBB, 0x5B, 0x68, 0x54, 0xBA, 0x27, 0xE1, 0xB3, 0x7E, 0x35, 0x59, 0x4B, 0x4E, 0x2A, 0xCF,
                    0xDF, 0xBA, 0x58, 0xD5, 0x5A, 0xBA, 0xE9, 0xEC, 0x18, 0x2C, 0xCB, 0x1F, 0x22, 0x0E, 0x82, 0x87, 0x2F, 0x7F, 0x0A, 0xE3,
                    0xA2, 0x07, 0x73, 0x69, 0xDC, 0x0D, 0x20, 0x05, 0x8A, 0x41, 0x9B, 0xB9, 0x5D, 0xBE, 0xB0, 0x00, 0xC4, 0xBB, 0x1C, 0xE5,
                    0x71, 0x03, 0x01, 0x7F, 0xF7, 0x19, 0xC0, 0xB5, 0x5F, 0x8F, 0x50, 0xE6, 0x43, 0xCF, 0x33, 0xC6, 0x2A, 0xAE, 0xCA, 0x39,
                    0x13, 0x8B, 0xBB, 0xB1, 0x36, 0xA6, 0xD1, 0x46, 0x0C, 0xCD, 0x85, 0x57, 0x50, 0x6E, 0x79, 0xF5, 0x1E, 0x76, 0x16, 0x7A,
                    0x1E, 0x5A, 0xD8, 0x13, 0x7E, 0xDD, 0x01, 0xB2, 0x5D, 0x5F, 0x7E, 0x4D, 0xF3, 0x34, 0x76, 0x5A, 0x3B, 0x66, 0xB2, 0x6F,
                    0xAD, 0x20, 0xF4, 0x20, 0x0B, 0x2B, 0x5A, 0xDF, 0x5B, 0xFD, 0x7B, 0x46, 0x55, 0x06, 0x4F, 0x9C, 0xFF, 0x2D, 0x30, 0xE8,
                    0xAE, 0x81, 0x70, 0x70, 0x1E, 0x16, 0x09, 0xAD, 0x79, 0x0B, 0xDF, 0x5C, 0xEF, 0x37, 0x0A, 0xE5, 0x4A, 0x90, 0xE5, 0x83,
                    0xE4, 0x65, 0xD6, 0x1D, 0xC3, 0xB3, 0x04, 0xF2, 0x94, 0x84, 0x5E, 0x43, 0x59, 0xF5, 0x0D, 0xAD, 0x93, 0xEA, 0xED, 0xB0,
                    0xC8, 0x25, 0xFF, 0xAB, 0x7E, 0xE5, 0xD6, 0xAA, 0x8F, 0x8C, 0x24, 0xBE, 0x4B, 0xCF, 0xDF, 0x92, 0xAC, 0xF0, 0xA9, 0xE7,
                    0xFC, 0xF0, 0x6A, 0x55, 0x12, 0xB7, 0x08, 0xA1, 0x27, 0xB1, 0x76, 0x2A, 0x0E, 0x27, 0x5C, 0xAA, 0x71, 0xFA, 0x3D, 0xF9,
                    0xDB, 0xA2, 0xC1, 0x67, 0x77, 0x0E, 0xFC, 0x1F, 0x43, 0xF8, 0x3C, 0x58, 0xE3, 0x4D, 0xD4, 0x2B, 0x2E, 0x29, 0x98, 0xDC,
                    0x2E, 0xC7, 0x77, 0x02, 0xAC, 0x5F, 0x94, 0x7E, 0x21, 0x3E, 0xE1, 0x52, 0x43, 0xB3, 0x72, 0xD1, 0x56, 0xD9, 0x62, 0xC2,
                    0x06, 0x75, 0xF0, 0x03, 0xD0, 0x97, 0xB5, 0xEE, 0x08, 0x6A, 0x70, 0xD7, 0x95, 0x38, 0xCC, 0x6A, 0x3C, 0x64, 0xC3, 0x26,
                    0x51, 0x31, 0x39, 0x81, 0x0A, 0x86, 0x17, 0xB8, 0x59, 0x8E, 0xD1, 0x1E, 0x43, 0xE9, 0x54, 0xFA, 0x9B, 0xCC, 0x90, 0x37,
                    0xA1, 0xCD, 0x6F, 0x21, 0x8E, 0xE4, 0x5C, 0x27, 0x87, 0xA3, 0x87, 0x85, 0x78, 0x11, 0xF2, 0x6E, 0xB2, 0x6F, 0x2E, 0x2A ];

        //private static uint xorKey_len = 1000;

        private byte[]? decryptBytes = null;
        public byte[]? encryptBytes = null;

        public uint UserID = 0;
        public uint Birthday = 0;
        public uint Bells = 0;
        public uint Exp = 0;
        public uint RegisteredAt = 0;
        public uint DealerCarLimit = 0;
        public uint MagicNumber = 0;
        public uint NewLeafTickets = 0;
        public ushort CompleteTickets = 0;
        public ushort GoldTreats = 0;

        public int BellsLocation = 0;
        public int NewLeafTicketsLocation = 0;
        public int CompleteTicketsLocation = 0;
        public int GoldTreatsLocation = 0;

        public bool DecryptSuccess = false;
        public float LoadProgress = 0;

        public static ItemViewModel? PlayerItemView = null;
        public static LookupViewModel? ListedItemView = null;
        public static List<InventoryItem>? inventoryItemList = [];

        public static List<ListedItem>? listedItemList = [];
        public static Dictionary<uint, string> DisplayNameDict = [];
        public static Dictionary<uint, string> iNameDict = [];

        public static int language = 0;

        public Utilities()
        {

        }

        public static async Task BuildDictionary()
        {
            listedItemList?.Clear();
            DisplayNameDict.Clear();
            iNameDict.Clear();

            using var iNameStream = await FileSystem.OpenAppPackageFileAsync("List/ItemID.csv");
            using var iNameReader = new StreamReader(iNameStream);
            while (!iNameReader.EndOfStream)
            {
                ListedItem listedItem = new();
                string? line = iNameReader.ReadLine();
                if (line != null)
                {
                    string[] parts = line.Split([','], StringSplitOptions.RemoveEmptyEntries);
                    iNameDict.Add(ConvertToUint(parts[0]), parts[1]);

                    listedItem.ItemID = ConvertToUint(parts[0]);
                    listedItem.ItemIName = parts[1];
                    listedItem.SetImage();
                    listedItemList?.Add(listedItem);
                }
            }
            iNameReader.Close();
            iNameStream.Close();

            string languagePath;
            if (language == 0) languagePath = "Eng";
            else if (language == 1) languagePath = "Ger";
            else if (language == 2) languagePath = "SpaEU";
            else if (language == 3) languagePath = "SpaUS";
            else if (language == 4) languagePath = "FreEU";
            else if (language == 5) languagePath = "FreUS";
            else if (language == 6) languagePath = "Ita";
            else if (language == 7) languagePath = "Jap";
            else if (language == 8) languagePath = "Kor";
            else if (language == 9) languagePath = "Chi";
            else languagePath = "Eng";

            using var NameStream = await FileSystem.OpenAppPackageFileAsync("List/" + languagePath + ".txt");
            using var NameReader = new StreamReader(NameStream);
            while (!NameReader.EndOfStream)
            {
                string? line = NameReader.ReadLine();
                if (line != null)
                {
                    string[] parts = line.Split([" ; "], StringSplitOptions.RemoveEmptyEntries);
                    
                    if (parts.Length > 1)
                    {
                        DisplayNameDict.Add(ConvertToUint(parts[0]), parts[1]);
                    }
                }
            }
            NameReader.Close();
            NameStream.Close();

            if (listedItemList != null)
            {
                for (int i = 0; i < listedItemList.Count; i++)
                {
                    /*
                    string DisplayName;
                    if (ChiDict.TryGetValue(listedItemList[i].ItemIName, out string? DisplayValue))
                    {
                        DisplayName = DisplayValue;
                    }
                    else
                    {
                        DisplayName = "";
                    }
                    WriteTextToFile(listedItemList[i].ItemID.ToString() + " ; " + DisplayName, "List/Chi.txt"); 
                    */
                    if (DisplayNameDict.TryGetValue(listedItemList[i].ItemID, out string? value))
                    {
                        listedItemList[i].ItemDisplayName = value;
                    }
                }
            }
        }

        public static async void WriteTextToFile(string text, string targetFileName)
        {
            string targetFile = System.IO.Path.Combine(targetFileName);
            using StreamWriter streamWriter = new StreamWriter(targetFile, append: true);
            await streamWriter.WriteLineAsync(text);
        }
        public static string GetItemIName(uint itemID)
        {
            if (iNameDict.TryGetValue(itemID, out string? value))
            {
                return value;
            }
            else
            {
                return "Missing_Name";
            }
        }

        public static string GetItemDisplayName(uint itemID)
        {
            if (DisplayNameDict.TryGetValue(itemID, out string? value))
            {
                return value;
            }
            else
            {
                return "";
            }
        }

        public static void SetPlayerItemViewModel(ItemViewModel? itemViewModel)
        {
            PlayerItemView = itemViewModel;
        }
        public static void DeployPlayerItem()
        {
            if (PlayerItemView == null) return;
            if (inventoryItemList == null) return;

            List<InventoryItem> sorted = inventoryItemList.OrderByDescending(d => d.ItemDate).ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                PlayerItemView.Add(sorted[i]);
            }
        }

        public static void SetListedItemViewModel(LookupViewModel? LookupViewModel)
        {
            ListedItemView = LookupViewModel;
        }
        public static void DeployListedItem()
        {
            if (ListedItemView == null) return;
            if (listedItemList == null) return;

            for (int i = 0; i < listedItemList.Count; i++)
            {
                ListedItemView.Add(listedItemList[i]);
            }
        }

        public static uint RotateRight(uint x, int c)
        {
            return ((x << (32 - c)) | ((x & 0xFFFFFFFF) >> c));
        }
        public static uint ConvertToUint(string numberText)
        {
            if (uint.TryParse(numberText, out uint number))
            {
                return number;
            }
            else
            {
                throw new FormatException("Invalid number format");
            }
        }
        public static ushort ConvertToUshort(string numberText)
        {
            if (ushort.TryParse(numberText, out ushort number))
            {
                return number;
            }
            else
            {
                throw new FormatException("Invalid number format");
            }
        }
        static void Sha256_add64(uint[] hash, byte[] data)
        {
            uint[] w = new uint[64];

            for (uint i = 0; i < 16; ++i)
            {
                uint value = 0;
                for (uint j = 0; j < 4; ++j)
                {
                    value = (value << 8) | data[i * 4 + j];
                }
                w[i] = value;
            }

            /*
            for (uint i = 0; i < 16; ++i)
            {
                w[i] = (uint)(data[(i << 2) + 3] | (data[(i << 2) + 2] << 8) |
                              (data[(i << 2) + 1] << 16) | (data[(i << 2) + 0] << 24));
            }
            */

            for (uint i = 16; i < 64; ++i)
            {
                uint s0 = ((w[i - 15] << (32 - 7)) | ((w[i - 15] & 0xFFFFFFFF) >> 7)) ^
                          ((w[i - 15] << (32 - 18)) | ((w[i - 15] & 0xFFFFFFFF) >> 18)) ^
                          (w[i - 15] >> 3);
                uint s1 = ((w[i - 2] << (32 - 17)) | ((w[i - 2] & 0xFFFFFFFF) >> 17)) ^
                          ((w[i - 2] << (32 - 19)) | ((w[i - 2] & 0xFFFFFFFF) >> 19)) ^
                          (w[i - 2] >> 10);
                w[i] = w[i - 16] + s0 + w[i - 7] + s1;
            }

            /*
            for (uint i = 16; i < 64; ++i)
            {
                uint s0 = RotateRight(w[i - 15], 7) ^ RotateRight(w[i - 15], 18) ^ (w[i - 15] >> 3);
                uint s1 = RotateRight(w[i - 2], 17) ^ RotateRight(w[i - 2], 19) ^ (w[i - 2] >> 10);
                w[i] = w[i - 16] + s0 + w[i - 7] + s1;
            }
            */

            uint[] k = [
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1,
            0x923f82a4, 0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
            0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786,
            0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147,
            0x06ca6351, 0x14292967, 0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
            0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b,
            0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a,
            0x5b9cca4f, 0x682e6ff3, 0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
            0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
            ];

        uint a = hash[0];
            uint b = hash[1];
            uint c = hash[2];
            uint d = hash[3];
            uint e = hash[4];
            uint f = hash[5];
            uint g = hash[6];
            uint h = hash[7];

            for (uint i = 0; i < 64; ++i)
            {
                uint temp1 = h + (
                    ((e << (32 - 6)) | ((e & 0xFFFFFFFF) >> 6)) ^
                    ((e << (32 - 11)) | ((e & 0xFFFFFFFF) >> 11)) ^
                    ((e << (32 - 25)) | ((e & 0xFFFFFFFF) >> 25))
                ) + (g ^ (e & (f ^ g))) + k[i] + w[i];
                uint temp2 = (
                    ((a << (32 - 2)) | ((a & 0xFFFFFFFF) >> 2)) ^
                    ((a << (32 - 13)) | ((a & 0xFFFFFFFF) >> 13)) ^
                    ((a << (32 - 22)) | ((a & 0xFFFFFFFF) >> 22))
                ) + ((a & b) | (c & (a | b)));
                uint temp = a;
                a = temp1 + temp2;
                h = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = temp;
            }

            /*
            for (uint i = 0; i < 64; ++i)
            {
                uint temp1 = h + (
                    ((e << (32 - 6)) | ((e & 0xFFFFFFFF) >> 6)) ^
                    ((e << (32 - 11)) | ((e & 0xFFFFFFFF) >> 11)) ^
                    ((e << (32 - 25)) | ((e & 0xFFFFFFFF) >> 25))
                ) + (g ^ (e & (f ^ g))) + sha256k[i] + w[i];
                uint temp2 = (
                    ((a << (32 - 2)) | ((a & 0xFFFFFFFF) >> 2)) ^
                    ((a << (32 - 13)) | ((a & 0xFFFFFFFF) >> 13)) ^
                    ((a << (32 - 22)) | ((a & 0xFFFFFFFF) >> 22))
                ) + ((a & b) | (c & (a | b)));
                h = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = a;
                a = temp1 + temp2;
            }
            */

            /*
            for (uint i = 0; i < 64; ++i)
            {
                uint temp1 = h + (RotateRight(e, 6) ^ RotateRight(e, 11) ^ RotateRight(e, 25)) + (g ^ (e & (f ^ g))) + sha256k[i] + w[i];
                uint temp2 = (RotateRight(a, 2) ^ RotateRight(a, 13) ^ RotateRight(a, 22)) + ((a & b) | (c & (a | b)));
                h = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = a;
                a = temp1 + temp2;
            }
            */

            hash[0] += a;
            hash[1] += b;
            hash[2] += c;
            hash[3] += d;
            hash[4] += e;
            hash[5] += f;
            hash[6] += g;
            hash[7] += h;
            
        }
        static void Sha256bin(byte[] input, uint in_len, byte[] output)
        {
            uint[] hash = { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
                         0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };

            uint offset = 0;

            while (offset + 64 <= in_len)
            {
                Sha256_add64(hash, input.Skip((int)offset).Take(64).ToArray());
                offset += 64;
            }

            byte[] buffer = new byte[64];
            Array.Copy(input, offset, buffer, 0, in_len - offset);

            offset = in_len - offset;
            buffer[offset] = 0x80;
            Array.Clear(buffer, (int)offset + 1, 64 - (int)offset - 1);

            if (offset > 55)
            {
                Sha256_add64(hash, buffer);
                Array.Clear(buffer, 0, 64);
            }

            ulong bit_len = in_len << 3;
            buffer[56] = (byte)((bit_len >> 54) & 0xff);
            buffer[57] = (byte)((bit_len >> 48) & 0xff);
            buffer[58] = (byte)((bit_len >> 40) & 0xff);
            buffer[59] = (byte)((bit_len >> 32) & 0xff);
            buffer[60] = (byte)((bit_len >> 24) & 0xff);
            buffer[61] = (byte)((bit_len >> 16) & 0xff);
            buffer[62] = (byte)((bit_len >> 8) & 0xff);
            buffer[63] = (byte)((bit_len >> 0) & 0xff);

            Sha256_add64(hash, buffer);

            output[3] = (byte)(hash[0] & 0xff);
            output[2] = (byte)((hash[0] >> 8) & 0xff);
            output[1] = (byte)((hash[0] >> 16) & 0xff);
            output[0] = (byte)((hash[0] >> 24) & 0xff);
            output[7] = (byte)(hash[1] & 0xff);
            output[6] = (byte)((hash[1] >> 8) & 0xff);
            output[5] = (byte)((hash[1] >> 16) & 0xff);
            output[4] = (byte)((hash[1] >> 24) & 0xff);
            output[11] = (byte)(hash[2] & 0xff);
            output[10] = (byte)((hash[2] >> 8) & 0xff);
            output[9] = (byte)((hash[2] >> 16) & 0xff);
            output[8] = (byte)((hash[2] >> 24) & 0xff);
            output[15] = (byte)(hash[3] & 0xff);
            output[14] = (byte)((hash[3] >> 8) & 0xff);
            output[13] = (byte)((hash[3] >> 16) & 0xff);
            output[12] = (byte)((hash[3] >> 24) & 0xff);
            output[19] = (byte)(hash[4] & 0xff);
            output[18] = (byte)((hash[4] >> 8) & 0xff);
            output[17] = (byte)((hash[4] >> 16) & 0xff);
            output[16] = (byte)((hash[4] >> 24) & 0xff);
            output[23] = (byte)(hash[5] & 0xff);
            output[22] = (byte)((hash[5] >> 8) & 0xff);
            output[21] = (byte)((hash[5] >> 16) & 0xff);
            output[20] = (byte)((hash[5] >> 24) & 0xff);
            output[27] = (byte)(hash[6] & 0xff);
            output[26] = (byte)((hash[6] >> 8) & 0xff);
            output[25] = (byte)((hash[6] >> 16) & 0xff);
            output[24] = (byte)((hash[6] >> 24) & 0xff);
            output[31] = (byte)(hash[7] & 0xff);
            output[30] = (byte)((hash[7] >> 8) & 0xff);
            output[29] = (byte)((hash[7] >> 16) & 0xff);
            output[28] = (byte)((hash[7] >> 24) & 0xff);
        }
        static string Sha256(byte[] input, uint in_len)
        {
            byte[] output = new byte[32];
            Sha256bin(input, in_len, output);
            StringBuilder outStr = new();
            for (uint i = 0; i < 32; ++i)
            {
                outStr.Append(output[i].ToString("x2"));
            }
            return outStr.ToString();
        }
        static uint Ntohl(uint input)
        {
            byte[] bytes = BitConverter.GetBytes(input);
            return (uint)(bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3]);
        }
        static ulong Ntohll(ulong input)
        {
            byte[] bytes = BitConverter.GetBytes(input);
            return (ulong)Ntohl(BitConverter.ToUInt32(bytes, 0)) << 32 | Ntohl(BitConverter.ToUInt32(bytes, 4));
        }
        public static ushort[] ToUshorts(byte[] bytes, int startIndex)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
            {
                throw new ArgumentException("Invalid start index.");
            }

            if ((bytes.Length - startIndex) % 2 != 0)
            {
                throw new ArgumentException("Input must be a non-null, even-length byte array starting at the specified index.");
            }

            int length = (bytes.Length - startIndex) / 2;
            ushort[] ushorts = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                ushorts[i] = (ushort)((bytes[startIndex + i * 2] << 8) | bytes[startIndex + i * 2 + 1]);
            }

            return ushorts;
        }
        public static uint[] ToUints(byte[] bytes, int startIndex)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
            {
                throw new ArgumentException("Invalid start index.");
            }

            if ((bytes.Length - startIndex) % 4 != 0)
            {
                throw new ArgumentException("Input must be a non-null, 4-byte aligned byte array starting at the specified index.");
            }

            int length = (bytes.Length - startIndex) / 4;
            uint[] uints = new uint[length];
            for (int i = 0; i < length; i++)
            {
                uints[i] = (uint)((bytes[startIndex + i * 4] << 24) | (bytes[startIndex + i * 4 + 1] << 16) | (bytes[startIndex + i * 4 + 2] << 8) | bytes[startIndex + i * 4 + 3]);
            }

            return uints;
        }
        private static ushort[] BuildvTable(byte[] bytes, int length)
        {
            ushort[] ushorts = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                ushorts[i] = (ushort)((bytes[i * 2 + 1] << 8) | bytes[i * 2]);
            }
            return ushorts;
        }
        public static byte[] UintToBytes(uint value)
        {
            byte[] bytes =
            [
                (byte)(value & 0xFF),
                (byte)((value >> 8) & 0xFF),
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 24) & 0xFF),
            ];
            return bytes;
        }
        public static byte[] UShortToBytes(ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }
        static string GetFBStr(byte[] ptr)
        {
            uint offset = BitConverter.ToUInt32(ptr, 0);
            return Encoding.ASCII.GetString(ptr, (int)offset + 4, (int)BitConverter.ToUInt32(ptr, (int)offset));
        }
        public static void PrintBytesInHex(byte[] bytes, int length)
        {
            if (length > bytes.Length)
            {
                throw new ArgumentException("Length cannot be greater than the array length.");
            }

            string full = "";
            for (int i = 0; i < length; i++)
            {
                string hex = bytes[i].ToString("X2");
                full += hex + " ";
            }
            Debug.Print(full);
        }

        public static void PrintHex(uint[] array)
        {
            foreach (var value in array)
            {
                Debug.Print($"0x{value:X8} ");
            }
            Console.WriteLine();
        }

        public static string UnixTimestampToDateTime(long unixTimestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string CalculateHash(byte[] input)
        {
            var hash = SHA256.HashData(input);
            return Convert.ToHexStringLower(hash);
        }

        public void EncryptOnly(byte[] fileBytes)
        {
            decryptBytes = new byte[fileBytes.Length];
            Array.Copy(fileBytes, decryptBytes, fileBytes.Length);
        }


        public async Task Decrypt(byte[] fileBytes)
        {
            Bells = 0;
            Exp = 0;
            RegisteredAt = 0;
            DealerCarLimit = 0;
            MagicNumber = 0;
            NewLeafTickets = 0;
            CompleteTickets = 0;
            GoldTreats = 0;

            BellsLocation = 0;
            NewLeafTicketsLocation = 0;
            CompleteTicketsLocation = 0;
            GoldTreatsLocation = 0;
            inventoryItemList?.Clear();
            encryptBytes = null;

            decryptBytes = new byte[fileBytes.Length];

            for (int i = 0; i < fileBytes.Length; i++)
            {
                decryptBytes[i] = (byte)(fileBytes[i] ^ xorKey[i % 999]);
            }

            if (Encoding.ASCII.GetString(decryptBytes, 0, 4) != "C4FH")
            {
                return;
            }

            await Task.Run(() =>
            {

                //string calcedHash = Sha256(decryptBytes.Skip(68).ToArray(), (uint)(decryptBytes.Length - 68));
                string calcedHash = CalculateHash(decryptBytes.Skip(68).ToArray());
                if (Encoding.ASCII.GetString(decryptBytes, 4, 64) != calcedHash)
                {
                    //Debug.Print("Hash mismatch! Aborting as a precaution.");
                    //Debug.Print("Hash       = {0}", Encoding.ASCII.GetString(decryptBytes, 4, 64));
                    //Debug.Print("Calculated = {0}", calcedHash);
                    return;
                }

                DecryptSuccess = true;

                //Debug.Print("Version: {0}", ntohl(BitConverter.ToUInt32(decryptBytes, 72)));
                //Debug.Print("# of keys: {0}", ntohl(BitConverter.ToUInt32(decryptBytes, 76)));

                ulong profileSize = 0, profileOffset = 0;

                int offset = 80;
                int k = 0;
                while (offset + 24 <= decryptBytes.Length)
                {
                    //LoadProgress = (float)(offset / (decryptBytes.Length * 0.15) - 0.4);

                    if (Encoding.ASCII.GetString(decryptBytes, offset, 4) != "C4SI") break;
                    int keyLen = (int)Ntohl(BitConverter.ToUInt32(decryptBytes, offset + 4));
                    if (offset + 24 + keyLen > decryptBytes.Length) break;
                    string keyName = Encoding.ASCII.GetString(decryptBytes, offset + 8, keyLen);
                    ulong binSize = Ntohll(BitConverter.ToUInt64(decryptBytes, offset + 8 + keyLen));
                    ulong binOffset = Ntohll(BitConverter.ToUInt64(decryptBytes, offset + 8 + keyLen + 8));
                    //Debug.Print("Key #{0}: {1} is {2}b at offset {3}", k, keyName, binSize, binOffset);

                    if (keyName == "UserProfile_1")
                    {
                        profileSize = binSize;
                        profileOffset = binOffset;
                    }


                    if (keyName.Length >= 23 && keyName.Substring(0, 23) == "UserItemStockOneSlotSet")
                    {

                        byte[] theBin = decryptBytes.Skip((int)binOffset).Take((int)binSize).ToArray();

                        //PrintBytesInHex(theBin, (int)binSize);

                        int rootTableLocation = (int)BitConverter.ToUInt32(theBin, 0);
                        int vTableLocation = rootTableLocation - theBin[rootTableLocation];
                        int entryCount = (theBin[vTableLocation] / 2) - 2;
                        byte[] vTableBytes = theBin.Skip(vTableLocation).ToArray();
                        ushort[] vTable = Utilities.BuildvTable(vTableBytes, entryCount + 2);

                        byte[] rootTable = theBin.Skip((int)BitConverter.ToUInt32(theBin, 0)).ToArray();

                        //ushort[] vTable = ToUshorts(rootTable, rootTable.Length - BitConverter.ToInt32(rootTable, 0));
                        //int entryCount = (vTable[0] / 2) - 2;
                        //if (entryCount >= 1 && vTable[2] != 0) Debug.Print("  ID = {0}", theBin[rootTableLocation + vTable[2]]);
                        if (entryCount >= 2 && vTable[3] != 0)
                        {
                            int entryVectorLocation = rootTableLocation + vTable[3] + theBin[rootTableLocation + vTable[3]];

                            //Debug.Print("  {0} entries:", theBin[entryVectorLocation]);
                            for (int e = 0; e < theBin[entryVectorLocation]; ++e)
                            {
                                InventoryItem item = new();

                                int eTableLocation = entryVectorLocation + (e + 1) * 4 + (int)BitConverter.ToUInt32(theBin, entryVectorLocation + (e + 1) * 4);
                                uint eVTableLocation = (uint)(eTableLocation - BitConverter.ToUInt32(theBin.Skip(eTableLocation).ToArray(), 0));
                                byte[] eVTableBytes = theBin.Skip((int)eVTableLocation).ToArray();
                                int eeCount = (theBin[eVTableLocation] / 2) - 2;
                                ushort[] eVTable = Utilities.BuildvTable(eVTableBytes, eeCount + 2);

                                //byte[] eTable = rootTable.Skip((int)(entryVector[e + 1] + entryVector[e + 1])).ToArray();

                                //ushort[] eVTable = ToUshorts(eTable, eTable.Length - BitConverter.ToInt32(eTable, 0));

                                //Debug.Print("   Entry {0}:", (e + 1));
                                //if (eeCount >= 1 && eVTable[2] != 0)
                                //{
                                //    Debug.Print("    ID = {0}", BitConverter.ToUInt32(theBin.Skip(eTableLocation + eVTable[2]).ToArray(), 0));
                                //}
                                if (eeCount >= 2 && eVTable[3] != 0)
                                {
                                    uint itemID = BitConverter.ToUInt32(theBin.Skip(eTableLocation + eVTable[3]).ToArray(), 0);
                                    item.ItemID = itemID;
                                    item.IDLocation = (int)binOffset + eTableLocation + eVTable[3];
                                    item.ItemIName = GetItemIName(itemID);
                                    item.ItemDisplayName = GetItemDisplayName(itemID);
                                    item.SetImage();

                                    if (itemID == 7000002)
                                    {
                                        CompleteTickets = BitConverter.ToUInt16(theBin.Skip(eTableLocation + eVTable[6]).ToArray(), 0);
                                        CompleteTicketsLocation = (int)binOffset + eTableLocation + eVTable[6];
                                    }
                                    else if (itemID == 1301048)
                                    {
                                        GoldTreats = BitConverter.ToUInt16(theBin.Skip(eTableLocation + eVTable[6]).ToArray(), 0);
                                        GoldTreatsLocation = (int)binOffset + eTableLocation + eVTable[6];
                                    }
                                    //Debug.Print("    Item ID = {0}", itemID);
                                }
                                if (eeCount >= 3 && eVTable[4] != 0)
                                {
                                    uint date = BitConverter.ToUInt32(theBin.Skip(eTableLocation + eVTable[4]).ToArray(), 0);
                                    item.ItemDate = date;
                                    item.NormalDate = UnixTimestampToDateTime(date);
                                    item.DateLocation = (int)binOffset + eTableLocation + eVTable[4];
                                    //Debug.Print("    Date = {0}", BitConverter.ToUInt32(theBin.Skip(eTableLocation + eVTable[4]).ToArray(), 0));
                                }
                                if (eeCount >= 4 && eVTable[5] != 0)
                                {
                                    byte check = theBin[eTableLocation + eVTable[5]];
                                    item.IsChecked = check;
                                    //Debug.Print("    Is Checked = {0}", theBin[eTableLocation + eVTable[5]]);
                                }
                                if (eeCount >= 5 && eVTable[6] != 0)
                                {
                                    ushort number = BitConverter.ToUInt16(theBin.Skip(eTableLocation + eVTable[6]).ToArray(), 0);
                                    item.ItemNum = number;
                                    item.NumLocation = (int)binOffset + eTableLocation + eVTable[6];
                                    //Debug.Print("    Num = {0}", BitConverter.ToUInt16(theBin.Skip(eTableLocation + eVTable[6]).ToArray(), 0));
                                }
                                if (eeCount >= 6 && eVTable[7] != 0)
                                {
                                    string guid = GetFBStr(theBin.Skip(eTableLocation + eVTable[7]).ToArray());
                                    item.GUID = guid;
                                    //Debug.Print("    GUID = {0}", getFBStr(theBin.Skip(eTableLocation + eVTable[7]).ToArray()));
                                }
                                if (eeCount >= 7 && eVTable[8] != 0)
                                {
                                    byte place = theBin[eTableLocation + eVTable[8]];
                                    item.PutPlace = place;
                                    //Debug.Print("    Put place = {0}", theBin[eTableLocation + eVTable[8]]); 
                                }
                                if (eeCount >= 8 && eVTable[9] != 0)
                                {
                                    byte islock = theBin[eTableLocation + eVTable[9]];
                                    item.IsLock = islock;
                                    //Debug.Print("    Is lock = {0}", theBin[eTableLocation + eVTable[9]]); 
                                }
                                inventoryItemList?.Add(item);
                            }
                        }
                    }


                    /*
                    if (keyName.Length >= 25 && keyName.Substring(0, 25) == "UserPresentBalloonOneSlot")
                    {
                        byte[] binPtr = dec.Skip((int)binOffset).ToArray();
                        byte[] rootTable = binPtr.Skip((int)BitConverter.ToUInt32(binPtr, 0)).ToArray();
                        ushort[] vTable = BitConverter.ToUInt16s(rootTable, rootTable.Length - BitConverter.ToInt32(rootTable, 0));
                        int entryCount = (vTable[0] / 2) - 2;
                        if (entryCount >= 1 && vTable[2] != 0) Debug.Print("  ID = {0}", BitConverter.ToUInt32(rootTable, vTable[2]));
                        if (entryCount >= 2 && vTable[3] != 0) Debug.Print("  Enabled = {0}", rootTable[vTable[3]]);
                        if (entryCount >= 3 && vTable[4] != 0) Debug.Print("  NPC Label = {0}", getFBStr(rootTable.Skip(vTable[4]).ToArray()));
                        if (entryCount >= 4 && vTable[5] != 0) Debug.Print("  Rarity = {0}", BitConverter.ToUInt32(rootTable, vTable[5]));
                        if (entryCount >= 5 && vTable[6] != 0) Debug.Print("  Position ID = {0}", rootTable[vTable[6]]);
                        if (entryCount >= 6 && vTable[7] != 0)
                        {
                            uint[] entryVector = BitConverter.ToUInt32s(rootTable, vTable[7] + BitConverter.ToUInt32(rootTable, vTable[7]));
                            Debug.Print("  {0} entries:", entryVector[0]);
                            for (int e = 0; e < entryVector[0]; ++e)
                            {
                                byte[] eTable = rootTable.Skip((int)(entryVector[e + 1] + entryVector[e + 1])).ToArray();
                                ushort[] eVTable = BitConverter.ToUInt16s(eTable, eTable.Length - BitConverter.ToInt32(eTable, 0));
                                int eeCount = (eVTable[0] / 2) - 2;
                                Debug.Print("   Entry {0}:", (e + 1));
                                if (eeCount >= 1 && eVTable[2] != 0) Debug.Print("    Label = {0}", getFBStr(eTable.Skip(eVTable[2]).ToArray()));
                                if (eeCount >= 2 && eVTable[3] != 0) Debug.Print("    Num = {0}", BitConverter.ToUInt16(eTable, eVTable[3]));
                                if (eeCount >= 3 && eVTable[4] != 0) Debug.Print("    Campaign ID = {0}", BitConverter.ToUInt32(eTable, eVTable[4]));
                            }
                        }
                    }

                    if (keyName.Length >= 18 && keyName.Substring(0, 18) == "UserPresentBoxData")
                    {
                        byte[] binPtr = dec.Skip((int)binOffset).ToArray();
                        byte[] rootTable = binPtr.Skip((int)BitConverter.ToUInt32(binPtr, 0)).ToArray();
                        ushort[] vTable = BitConverter.ToUInt16s(rootTable, rootTable.Length - BitConverter.ToInt32(rootTable, 0));
                        int entryCount = (vTable[0] / 2) - 2;
                        if (entryCount >= 1 && vTable[2] != 0) Debug.Print("  ID = {0}", BitConverter.ToUInt32(rootTable, vTable[2]));
                        if (entryCount >= 2 && vTable[3] != 0) Debug.Print("  Expired at = {0}", BitConverter.ToUInt32(rootTable, vTable[3]));
                        if (entryCount >= 3 && vTable[4] != 0) Debug.Print("  Inventory Data ID = {0}", getFBStr(rootTable.Skip(vTable[4]).ToArray()));
                        if (entryCount >= 4 && vTable[5] != 0) Debug.Print("  Opened at = {0}", BitConverter.ToUInt32(rootTable, vTable[5]));
                        if (entryCount >= 5 && vTable[6] != 0) Debug.Print("  Route = {0}", BitConverter.ToUInt32(rootTable, vTable[6]));
                        if (entryCount >= 6 && vTable[7] != 0) Debug.Print("  Route Option = {0}", getFBStr(rootTable.Skip(vTable[7]).ToArray()));
                        if (entryCount >= 7 && vTable[8] != 0) Debug.Print("  Item Label = {0}", getFBStr(rootTable.Skip(vTable[8]).ToArray()));
                        if (entryCount >= 8 && vTable[9] != 0) Debug.Print("  Item Amount = {0}", BitConverter.ToUInt32(rootTable, vTable[9]));
                        if (entryCount >= 9 && vTable[10] != 0) Debug.Print("  Item Option = {0}", getFBStr(rootTable.Skip(vTable[10]).ToArray()));
                    }
                    */

                    offset += 24 + keyLen;
                    ++k;
                }

                if (profileSize != 0)
                {
                    //Debug.Print("User Profile: ");

                    /*
                        byte[] theBin = decryptBytes.Skip((int)binOffset).ToArray();
                        int rootTableLocation = (int)BitConverter.ToUInt32(theBin, 0);
                        int vTableLocation = rootTableLocation - theBin[rootTableLocation];
                        int entryCount = (theBin[vTableLocation] / 2) - 2;
                        byte[] vTableBytes = theBin.Skip(vTableLocation).ToArray();
                        ushort[] vTable = buildvTable(vTableBytes, entryCount + 2);
                     */

                    byte[] theProfile = decryptBytes.Skip((int)profileOffset).ToArray();
                    int rootTableLocation = (int)BitConverter.ToUInt32(theProfile, 0);
                    int vTableLocation = rootTableLocation - theProfile[rootTableLocation];
                    byte[] vTableBytes = theProfile.Skip(vTableLocation).ToArray();
                    int entryCount = (theProfile[vTableLocation] / 2) - 2;
                    ushort[] vTable = Utilities.BuildvTable(vTableBytes, entryCount + 2);
                    if (entryCount >= 1 && vTable[2] != 0)
                    {
                        UserID = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[2]).ToArray(), 0);
                        Debug.Print("  ID = {0}", UserID);
                    }
                    if (entryCount >= 2 && vTable[3] != 0)
                    {
                        Birthday = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[3]).ToArray(), 0);
                        Debug.Print("  Birthday = {0}", Birthday);
                    }
                    if (entryCount >= 3 && vTable[4] != 0)
                    {
                        BellsLocation = (int)profileOffset + rootTableLocation + vTable[4];
                        Bells = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[4]).ToArray(), 0);
                        Debug.Print("  Bells = {0}", Bells);
                    }
                    if (entryCount >= 4 && vTable[5] != 0)
                    {
                        Exp = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[5]).ToArray(), 0);
                        Debug.Print("  Exp = {0}", Exp); 
                    }
                    if (entryCount >= 5 && vTable[6] != 0)
                    {
                        RegisteredAt = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[6]).ToArray(), 0);
                        Debug.Print("  RegisteredAt = {0}", RegisteredAt);
                    }
                    if (entryCount >= 6 && vTable[7] != 0)
                    {
                        DealerCarLimit = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[7]).ToArray(), 0);
                        Debug.Print("  DealerCarLimit = {0}", DealerCarLimit);
                    }
                    if (entryCount >= 7 && vTable[8] != 0)
                    {
                        MagicNumber = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[8]).ToArray(), 0);
                        Debug.Print("  MagicNumber = {0}", MagicNumber); 
                    }
                    if (entryCount >= 8 && vTable[9] != 0)
                    {
                        NewLeafTicketsLocation = (int)profileOffset + rootTableLocation + vTable[9];
                        NewLeafTickets = BitConverter.ToUInt32(theProfile.Skip(rootTableLocation + vTable[9]).ToArray(), 0);
                        Debug.Print("  NewLeafTickets = {0}", NewLeafTickets);
                    }

                    if (PlayerItemView != null)
                    {
                        PlayerItemView.ClearItem();
                        DeployPlayerItem();
                    }
                }
            });
        }

        public async Task<bool> Encrypt(bool skipEncrypt)
        {
            if (decryptBytes == null) return false;

            
            if (BellsLocation != 0)
            {
                byte[] bellsByte = Utilities.UintToBytes(Bells);
                Array.Copy(bellsByte, 0, decryptBytes, BellsLocation, 4);
            }
            if (NewLeafTicketsLocation != 0)
            {
                byte[] newLeafTicketByte = Utilities.UintToBytes(NewLeafTickets);
                Array.Copy(newLeafTicketByte, 0, decryptBytes, NewLeafTicketsLocation, 4);
            }

            if (CompleteTicketsLocation != 0)
            {
                byte[] completeTicketByte = UShortToBytes(CompleteTickets);
                Array.Copy(completeTicketByte, 0, decryptBytes, CompleteTicketsLocation, 2);
            }
            if (GoldTreatsLocation != 0)
            {
                byte[] goldTreatsByte = UShortToBytes(GoldTreats);
                Array.Copy(goldTreatsByte, 0, decryptBytes, GoldTreatsLocation, 2);
            }
            

            encryptBytes = new byte[decryptBytes.Length];

            await Task.Run(() =>
            {
                if (inventoryItemList != null)
                {
                    for (int i = 0; i < inventoryItemList.Count; i++)
                    {
                        if (inventoryItemList[i].HasChanged)
                        {
                            byte[] newItemID = Utilities.UintToBytes(inventoryItemList[i].ItemID);
                            Array.Copy(newItemID, 0, decryptBytes, inventoryItemList[i].IDLocation, 4);
                            byte[] newItemNum = UShortToBytes(inventoryItemList[i].ItemNum);
                            Array.Copy(newItemNum, 0, decryptBytes, inventoryItemList[i].NumLocation, 2);
                            /*byte[] newDate = UintToBytes(1617267600);
                            Array.Copy(newDate, 0, decryptBytes, inventoryItemList[i].DateLocation, 4);*/
                        }
                    }
                }

                //Debug.Print("Patching hash...");
                //string newHash = Sha256(decryptBytes.Skip(68).ToArray(), (uint)(decryptBytes.Length - 68));
                string newHash = CalculateHash(decryptBytes.Skip(68).ToArray());
                Array.Copy(Encoding.ASCII.GetBytes(newHash), 0, decryptBytes, 4, 64);

                //Console.WriteLine("Re-encrypting...");
                if(!skipEncrypt)
                {
                    for (int i = 0; i < decryptBytes.Length; ++i)
                    {
                        encryptBytes[i] = (byte)(decryptBytes[i] ^ xorKey[i % 999]);
                    }
                }
                else
                {
                    Array.Copy(decryptBytes, encryptBytes, decryptBytes.Length);
                }
            });

            return true;
        }
    }
}
