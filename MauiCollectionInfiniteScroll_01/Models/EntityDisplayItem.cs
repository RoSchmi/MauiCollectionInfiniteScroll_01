
using CommunityToolkit.Mvvm.ComponentModel;
using Azure;
using Azure.Data.Tables;
using RoSchmi.Azure.Converters;
using static MauiCollectionInfiniteScroll_01.Models.EntityDisplayFormat;
using Common.Models.EdmTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiCollectionInfiniteScroll_01.Models
{
    public partial class EntityDisplayItem : ObservableObject // , ISelectableItem
    {
        /* The following constants are now located in EntityDisplayFormat.cs
       public const int GapSize = 2;
       public const int IdxSize = 6;
       public const int PartKeySizeMin = 13;
       public const int PartKeySizeMax = 26;
       public const int RowKeySizeMin = 7;
       public const int RowKeySizeMax = 29;
       public const int TimeStampSize = 28;
       public const int GuidSize = 26;
       public const int PropertySizeMin = 6;
       public const int PropertySizeMax = 26;
       */

        private readonly bool _showIdx;

        private readonly EntityDisplaySchema _schema;
        private readonly Dictionary<string, object> _properties = new();

        public int Index { get; }

        [ObservableProperty]
        public partial string PartitionKey { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string RowKey { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTimeOffset? TimeStamp { get; set; }

        [ObservableProperty]
        public partial ETag ETag { get; set; }

        [ObservableProperty]
        public partial bool IsSelected { get; set; }

        [ObservableProperty]
        public partial string EntityRow { get; set; } = string.Empty;


        #region Constructor
        public EntityDisplayItem(int index, TableEntity pTableEntity, EntityDisplaySchema schema, bool showIdx = true)
        {
            _schema = schema;
            _showIdx = showIdx;

            // Index is limited to be between 0 and 999.999
            Index = (index >= 0 && index < 1_000_000) ? index : -1;

            PartitionKey = pTableEntity.PartitionKey;
            RowKey = pTableEntity.RowKey;
            TimeStamp = pTableEntity.Timestamp;
            ETag = pTableEntity.ETag;

            foreach (var property in pTableEntity)
            {
                if (property.Key is "PartitionKey" or "RowKey" or "Timestamp" or "odata.etag")
                    continue;

                _properties[property.Key] = property.Value;
            }

            EntityRow = BuildFormattedString();
            
        }
        #endregion

        public int GetColumnLength(int i)
        {
            //should not happen, but is secure if it unexpectedly does
            if (i < 0 || i >= _schema.ColumnNames.Count)
                return 0;

            var key = _schema.ColumnNames[i];
            if (!_properties.TryGetValue(key, out var value))
                return PropertySizeMin;
            //  return "null".Length; // "null" muss hineinpassen

            if (value is ETag)
                return 0;

            return EntityPropertyAsString.ConvertProperty(value).Length;
        }

        public string BuildFormattedString()
        {
            // Assert that Collumn Names and Widths have the same number
            System.Diagnostics.Debug.Assert(_schema.ColumnNames.Count == _schema.ColumnWidths.Count);

            string gapSizeString = new string(' ', GapSize);
            StringBuilder builder = new StringBuilder();

            if (_showIdx)
                builder.Append(Index.ToString().PadLeft(IdxSize)).Append(gapSizeString);


            builder.Append(TruncateWithDots(PartitionKey, _schema.PartitionKeyWidth)
                .PadRight(_schema.PartitionKeyWidth))
                .Append(gapSizeString);


            builder.Append(TruncateWithDots(RowKey, _schema.RowKeyWidth)
                .PadRight(_schema.RowKeyWidth))
                .Append(gapSizeString);


            builder.Append(TimeStamp?.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ")
                                .PadRight(TimeStampSize)
                      ?? new string(' ', TimeStampSize))
              .Append(gapSizeString);

            for (int i = 0; i < _schema.ColumnNames.Count; i++)
            {
                string key = _schema.ColumnNames[i];
                string value;

                if (_properties.TryGetValue(key, out var v))
                {
                    if (v is ETag)
                    {
                        value = string.Empty;
                    }
                    else
                    {
                        value = EntityPropertyAsString.ConvertProperty(v);
                        // ggf. hart abschneiden mit Punkten bei Längenüberschreitung
                        if (value.Length > _schema.ColumnWidths[i])
                            value = TruncateWithDots(value, _schema.ColumnWidths[i]);
                    }
                }
                else
                {
                    value = "null";
                }

                builder.Append(value.PadLeft(_schema.ColumnWidths[i], ' '));
                builder.Append(gapSizeString);

            }
            return builder.ToString();
        }

        private static string TruncateWithDots(string value, int width)
        {
            if (value.Length <= width)
                return value;

            if (width <= 2)
                return new string('.', width);

            return value.Substring(0, width - 2) + "..";
        }
    }
}
