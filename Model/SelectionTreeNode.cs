using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PCCE.Model
{
    public class SelectionTreeNode
    {
        private bool _isChecked;

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
                    LoadNameAsync(value);
                }
            } 
        }

        private async void LoadNameAsync(string value)
        {
            var ListFilePath = "List/" + value + ".txt";
            if (File.Exists(ListFilePath))
            {
                var exclusiveItemIDsStream = await FileSystem.OpenAppPackageFileAsync(ListFilePath);
                using var exclusiveItemIDsReader = new StreamReader(exclusiveItemIDsStream);
                int count = -1;
                while (!exclusiveItemIDsReader.EndOfStream)
                {
                    var line = exclusiveItemIDsReader.ReadLine();
                    if (line != null)
                    {
                        if (count == -1)
                        {
                            DisplayName = line;
                        }
                        else
                        {
                            Children.Add(new SelectionTreeNode { Name = line });    
                        }
                        count++;
                    }
                }
                DisplayName = DisplayName + " (" + count + ")";
                exclusiveItemIDsReader.Close();
                exclusiveItemIDsStream.Close();
            }
            else
            {
                DisplayName = value;
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
                    UpdateParentCheckState();
                    UpdateChildrenCheckState(value);
                }
            }
        }

        private void UpdateParentCheckState()
        {
            if (Parent != null)
            {
                Parent.IsChecked = Parent.Children.All(x => x.IsChecked);
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
    }
}