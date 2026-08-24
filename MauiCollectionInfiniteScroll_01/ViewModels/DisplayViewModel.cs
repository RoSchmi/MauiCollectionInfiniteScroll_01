using Azure.Data.Tables;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiCollectionInfiniteScroll_01.Models;
using System.Collections.ObjectModel;


namespace MauiCollectionInfiniteScroll_01.ViewModels
{
    public partial class DisplayViewModel : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private bool isBusy;

        EntityDisplaySchema _entityDisplaySchema = new EntityDisplaySchema();
        public ObservableCollection<EntityDisplayItem> DisplayItemCollection { get; } = new ObservableCollection<EntityDisplayItem>();

        public List<EntityDisplayItem> DisplayItemList { get; }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var theThing = query;
            // get additional Arguments if needed
        }

        #region Region Constructor         
        public DisplayViewModel(List<EntityDisplayItem> items)
        { 
            DisplayItemList = items;
            IsBusy = true;  // Is immediately set to false in Loaded event of page
            
            DisplayItemCollection = new ObservableCollection<EntityDisplayItem>(DisplayItemList);
        }
        #endregion
        [RelayCommand]
        private async Task GetNextData()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Task.Delay(1);   // Only to show ActivityIndicator for at least 100 ms

                _entityDisplaySchema = new EntityDisplaySchema();

                int start = DisplayItemList.Count;
                for (int i = start; i < start + 20; i++)
                {
                    TableEntity tableEntity = new TableEntity();
                    tableEntity.PartitionKey = "FirstPartition";
                    tableEntity.RowKey = $"RowKey{i}";
                    tableEntity.Timestamp = DateTime.Now;

                    IDictionary<string, object>? properties = new Dictionary<string, object>(0);

                    properties.Add("T_0", $"Temp_{i}");
                    properties.Add("T_1", $"Humid_{i}");
                    properties.Add("T_2", $"Volt_{i}");
                    properties.Add("T_3", $"Power_{i}");

                    foreach (var p in properties)
                    {
                        tableEntity[p.Key] = p.Value;
                    }
                    if (i == start)
                    {
                        _entityDisplaySchema.InitializeFromEntity(tableEntity);
                    }

                    DisplayItemList.Add(new EntityDisplayItem(i, tableEntity, _entityDisplaySchema, showIdx: true));
                    DisplayItemCollection.Add(DisplayItemList[DisplayItemList.Count - 1]);
                }            
                IsBusy = false;
            }
        }
    }
}
