using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PCCE
{
    public partial class MultiSelectDialogPage : ContentPage
    {
        public List<string> SelectedExclusive { get; private set; } = new List<string>();

        public List<Model.SelectionTreeNode> TreeNodes {get; set; } = new List<Model.SelectionTreeNode>
        {
            new() {
                Name = "ExclusiveTypes"
            }
        };
        public MultiSelectDialogPage()
        {
            InitializeComponent();
        }

        private async void OnOkButtonClicked(object sender, EventArgs e)
        {
            GetExclusiveItems(TreeNodes);
            await Navigation.PopModalAsync();
        }

        private void GetExclusiveItems(List<Model.SelectionTreeNode> treeNodes)
        {
            foreach (var node in treeNodes)
            {
                if (node.Children.Count == 0)
                {
                    if (node.IsChecked)
                    {
                        SelectedExclusive.Add(node.Name);
                    }
                }
                else
                {
                    GetExclusiveItems(node.Children);
                }
            }
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}