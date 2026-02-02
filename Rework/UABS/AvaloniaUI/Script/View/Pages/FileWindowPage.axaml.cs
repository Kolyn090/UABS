using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using UABS.Data;
using UABS.Util;
using UABS.ViewModel;

namespace UABS.AvaloniaUI
{
    public partial class FileWindowPage : UserControl
    {
        public FileWindowPage()
        {
            InitializeComponent();

            this.AttachedToVisualTree += (_, __) =>
            {
                if (this.Parent?.DataContext is MainViewModel mainVm)
                {
                    this.DataContext = mainVm.FileWindowVM;
                }
                else
                {
                    Log.Error("Folder Window must be placed inside a MainWindow with MainViewModel as DataContext.");
                }

                dataGrid.SelectionMode = DataGridSelectionMode.Extended;
                dataGrid.SelectionChanged += (s, e) =>
                {
                    if (this.DataContext is FileWindowViewModel vm)
                    {
                        // Add newly selected items
                        foreach (var item in e.AddedItems.OfType<AssetEntry>())
                            vm.SelectAsset(item);

                        // Remove newly deselected items
                        foreach (var item in e.RemovedItems.OfType<AssetEntry>())
                            vm.DeselectAsset(item);
                    }
                };

                if (this.DataContext is FileWindowViewModel vm)
                {
                    Log.Info("Displaying unsupported preview panel.", file: "FileWindowPage.axaml.cs");
                    vm.AssetPreview = new UnknownPreviewViewModel("Preview not available for this asset type.");
                }
            };
        }

        private void RowClicked(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Border b && b.DataContext is AssetEntry row)
            {
                if (this.DataContext is FileWindowViewModel vm)
                {
                    vm.LastSelectedAsset = row;
                    // Log.Info($"Last clicked row: {row.Name}");
                }
            }
        }
    }
}