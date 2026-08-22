using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using MauiCollectionInfiniteScroll_01.Models;

namespace MauiCollectionInfiniteScroll_01.ViewModels
{
    public partial class DisplayViewModel : ObservableObject, IQueryAttributable
    {
        public bool IsBusy { get; set;} = false;

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
            DisplayItemCollection = new ObservableCollection<EntityDisplayItem>(DisplayItemList);
        }
        #endregion

    }
}
