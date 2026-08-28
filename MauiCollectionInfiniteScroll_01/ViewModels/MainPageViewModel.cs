using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MauiCollectionInfiniteScroll_01.Models;
using MauiCollectionInfiniteScroll_01.Pages;
using Azure.Data.Tables;

namespace MauiCollectionInfiniteScroll_01.ViewModels
{
    public partial class MainPageViewModel : ObservableObject  
    {
        //EntityDisplaySchema _entityDisplaySchema = new EntityDisplaySchema();
        public EntityDisplaySchema DisplaySchema = new EntityDisplaySchema();

        public List<EntityDisplayItem> DisplayItemList { get; } = new List<EntityDisplayItem>();
        
        #region Region Constructor
        public MainPageViewModel()
        {
            InitializeSampleData();      
        }
        #endregion

        #region Region [RelayCommand] Task Go()
        [RelayCommand]
        private async Task Go()
        {
            //EntityDisplaySchema schemaBackUp = new EntityDisplaySchema();
            /*
            _properties.Clear();
            foreach (var columnname in DisplaySchema.ColumnNames)
            {
                
            }
            */
            /*
            _properties = DisplaySchema.

            _properties.Clear();
            _properties.Add("T_0", $"Temp_{i}");
            _properties.Add("T_1", $"Humid_{i}");
            _properties.Add("T_2", $"Volt_{i}");
            _properties.Add("T_3", $"Power_{i}");

            foreach (var p in _properties)
            {
                tableEntity[p.Key] = p.Value;
            }
            */


            //var vm = new DisplayViewModel(DisplayItemList, _entityDisplaySchema);

            var vm = new DisplayViewModel(DisplayItemList, DisplaySchema);

            var page = new DisplayPage(vm);

            /*

            string Argument = "None";

            Dictionary<string, object> navigationParameter = new()
                {
                    {"sender", nameof(MainPage)},
                    {"argument", Argument}
                };
            */

            //Microsoft.Maui.Controls.INavigation.
            //await Microsoft.Maui.Controls.Navigation.PushAsync(page);
            //await Shell.Current.PushAsync(page, navigationParameter);
            //await Shell.Current.GoToAsync(page, navigationParameter);

            // await Shell.Current.GoToAsync(nameof(DisplayPage), navigationParameter);

            await Shell.Current.Navigation.PushAsync(page);
        }
        #endregion

        #region Region Method InitializeSampleData()
        void InitializeSampleData()
        {
            DisplayItemList.Clear();
            for (int i = 0; i < 100; i++)
            {
                TableEntity tableEntity = new TableEntity();
                tableEntity.PartitionKey = "FirstPartition";
                tableEntity.RowKey = $"RowKey{i}";
                tableEntity.Timestamp = DateTime.Now;

                var properties = new Dictionary<string, object>
                {
                    ["T_0"] = $"Temp_{i}",
                    ["T_1"] = $"Humid_{i}",
                    ["T_2"] = $"Volt_{i}",
                    ["T_3"] = $"Power_{i}"
                };

                foreach (var p in properties)
                    tableEntity[p.Key] = p.Value;

                /*
                IDictionary<string, object>? properties = new Dictionary<string, object>(0);
                properties.Clear();
                properties.Add("T_0", $"Temp_{i}");
                properties.Add("T_1", $"Humid_{i}");
                properties.Add("T_2", $"Volt_{i}");
                properties.Add("T_3", $"Power_{i}");

                foreach (var p in properties)
                {
                    tableEntity[p.Key] = p.Value;
                }
                */

                if (i == 0)
                {
                    DisplaySchema.InitializeFromEntity(tableEntity);               
                    var vm = new DisplayViewModel(DisplayItemList, DisplaySchema);
                }
                
                DisplayItemList.Add(new EntityDisplayItem(i, tableEntity, DisplaySchema, showIdx: true));
            }
        #endregion
        }
    }
}
