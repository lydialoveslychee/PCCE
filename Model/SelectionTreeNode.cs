using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Google.Crypto.Tink.Signature;

namespace PCCE.Model
{
    public class SelectionTreeNode : INotifyPropertyChanged
    {
        private bool _isChecked = false;

        public int Count {get; set; } = -1;

        public string DisplayName { get; set; } = string.Empty;

        private string _name = string.Empty;

        public string Name 
        { 
            get => _name; 
            set
            {
                _name = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Console.WriteLine("Tree Node Name: " + value);
                    LoadNameAsync(value);
                }
            } 
        }

        private async void LoadNameAsync(string value)
        {
            string ListFilePath = "List/ExclusiveItems/" + value + ".txt";
            Console.WriteLine("ListFilePath: " + ListFilePath);
            try {
                var exclusiveItemIDsStream = await FileSystem.OpenAppPackageFileAsync(ListFilePath);
                using var exclusiveItemIDsReader = new StreamReader(exclusiveItemIDsStream);
                while (!exclusiveItemIDsReader.EndOfStream)
                {
                    var line = exclusiveItemIDsReader.ReadLine();
                    if (line != null)
                    {
                        if (Count == -1)
                        {
                            DisplayName = line;
                            Count = 0;
                        }
                        else
                        {
                            SelectionTreeNode child = new() { Name = line, Parent = this };
                            Children.Add(child);    
                            Count += child.Count;
                        }
                    }
                }
                DisplayName = DisplayName + " (" + Count + ")";
                exclusiveItemIDsReader.Close();
                exclusiveItemIDsStream.Close();
            } catch (Exception e) {
                Console.WriteLine("Exception: " + e.Message);
                if(Utilities.DisplayNameDict.TryGetValue(Utilities.ConvertToUint(value), out string? displayName))
                {   
                    DisplayName = displayName;
                }
                else
                {
                    Utilities.iNameDict.TryGetValue(Utilities.ConvertToUint(value), out string? iName);
                    DisplayName = iName ?? value;
                }
                Count = 1;
            }
        }

        public List<SelectionTreeNode> Children { get; set; } = [];

        public SelectionTreeNode Parent { get; set; } = null!;

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    UpdateChildrenCheckState(value);
                    UpdateParentCheckState();
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        private void UpdateParentCheckState()
        {
            if (Parent != null)
            {
                bool allChecked = Parent.Children.All(x => x.IsChecked);
                bool allUnchecked = Parent.Children.All(x => !x.IsChecked);

                if (allChecked || allUnchecked)
                {
                    Parent.IsChecked = allChecked;
                }
            }
        }

        private void UpdateChildrenCheckState(bool isChecked)
        {
            if (Children != null && Children.Count > 0)
            {
                foreach (var child in Children)
                {
                    child.IsChecked = isChecked;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}