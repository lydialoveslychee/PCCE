using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace PCCE
{
    public partial class MultiSelectDialogPage : ContentPage
    {
        public List<string> SelectedExclusive { get; private set; } = new List<string>();

        public List<string> SelectedStickers { get; private set; } = new List<string>();

        public List<Model.SelectionTreeNode> TreeNodes {get; set; } = new List<Model.SelectionTreeNode>();
        
        public List<Model.SelectionTreeNode> AvaliableStickers { get; set; } = new List<Model.SelectionTreeNode>();
        
        public MultiSelectDialogPage()
        {
            BindingContext = this;

            TreeNodes =
            [
                new Model.SelectionTreeNode 
                {
                    Name = "ExclusiveTypes"
                },
                new Model.SelectionTreeNode
                {
                    Name = "StickerTypes"
                }
            ];
            TreeNodes[0].GenerateChildNode();
            TreeNodes[1].GenerateChildNode();

            InitializeComponent();
        }


        private async void OnOkButtonClicked(object sender, EventArgs e)
        {
            GetExclusiveItems(TreeNodes[0]);
            GetSelectedStickers(TreeNodes[1]);
            await Navigation.PopModalAsync();
        }

        private void GetSelectedStickers(Model.SelectionTreeNode treeNode)
        {
            if (treeNode.Children.Count == 0)
            {
                if (treeNode.IsChecked)
                {
                    SelectedStickers.Add(treeNode.Name);
                }
            }
            else
            {
                foreach (var node in treeNode.Children)
                {
                    GetSelectedStickers(node);
                }
            }
        }

        private void GetExclusiveItems(Model.SelectionTreeNode treeNode)
        {
            if (treeNode.Children.Count == 0)
            {
                if (treeNode.IsChecked)
                {
                    SelectedExclusive.Add(treeNode.Name);
                }
            }
            else
            {
                foreach (var node in treeNode.Children)
                {
                    GetExclusiveItems(node);
                }
            }
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}