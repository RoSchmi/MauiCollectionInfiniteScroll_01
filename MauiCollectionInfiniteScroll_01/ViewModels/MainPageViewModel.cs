using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiCollectionInfiniteScroll_01.Models;
using MauiCollectionInfiniteScroll_01.Pages;
using Azure.Data.Tables;

namespace MauiCollectionInfiniteScroll_01.ViewModels
{
    public partial class MainPageViewModel : ObservableObject  
    {
        EntityDisplaySchema _entityDisplaySchema = new EntityDisplaySchema();
        
        public List<EntityDisplayItem> DisplayItemList { get; } = new List<EntityDisplayItem>();
        IDictionary<string, object>? _properties = new Dictionary<string, object> (0);
       

        #region Region Constructor
        public MainPageViewModel()
        {    
            DisplayItemList.Clear();
            for (int i = 0; i < 100; i++)
            {
                TableEntity tableEntity = new TableEntity();             
                tableEntity.PartitionKey = "First Partition";
                tableEntity.RowKey = $"RowKey{i}";
                tableEntity.Timestamp = DateTime.Now;

                _properties.Clear();
                _properties.Add("T_0", $"Temperature_{i}");
                _properties.Add("T_1", $"Humidity_{i}");
                _properties.Add("T_2", $"Voltage_{i}");
                _properties.Add("T_3", $"Power_{i}");

                foreach (var p in _properties)
                {
                    tableEntity[p.Key] = p.Value;
                }

                DisplayItemList.Add(new EntityDisplayItem(i, tableEntity, _entityDisplaySchema, showIdx: true));
               
            }
        }
        #endregion

        #region Region [RelayCommand] Task Go()
        [RelayCommand]
        private async Task Go()
        {
            //var vm = new DisplayViewModel(_displayItemList);
            var vm = new DisplayViewModel(DisplayItemList);
            var page = new DisplayPage(vm);

            string Argument = "None";

            Dictionary<string, object> navigationParameter = new()
                {
                    {"sender", nameof(MainPage)},
                    {"argument", Argument}
                };
            await Shell.Current.GoToAsync(nameof(DisplayPage), navigationParameter);           
        }
        #endregion
    }
}
