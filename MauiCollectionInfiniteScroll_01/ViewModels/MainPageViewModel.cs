using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiCollectionInfiniteScroll_01.Models;
using MauiCollectionInfiniteScroll_01.Pages;
using Azure.Data.Tables;

namespace MauiCollectionInfiniteScroll_01.ViewModels
{
    public partial class MainPageViewModel : ObservableObject  
    {   
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
            var vm = new DisplayViewModel(DisplayItemList, DisplaySchema);

            var page = new DisplayPage(vm);

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
                tableEntity.RowKey = $"RowKey{i.ToString("D4")}";
                tableEntity.Timestamp = DateTime.Now;

                var properties = new Dictionary<string, object>
                {

                    ["SampleTime"] = $"{DateTimeOffset.UtcNow.ToString("MM-dd-yyyy HH:mm:ss")} +{((int)DateTimeOffset.Now.Offset.TotalMinutes).ToString("D3")}",
                    ["T_0"] = $"Temp_{i.ToString("D4")}",
                    ["T_1"] = $"Humid_{i.ToString("D4")}",
                    ["T_2"] = $"Volt_{i.ToString("D4")}",
                    ["T_3"] = $"Power_{i.ToString("D4")}"
                };

                foreach (var p in properties)
                    tableEntity[p.Key] = p.Value;

                if (i == 0)
                {
                    DisplaySchema.InitializeFromEntity(tableEntity);               
                    var vm = new DisplayViewModel(DisplayItemList, DisplaySchema);
                }
                
                DisplayItemList.Add(new EntityDisplayItem(i, tableEntity, DisplaySchema, showIdx: true));
            }
        
        }
        #endregion
    }
}
