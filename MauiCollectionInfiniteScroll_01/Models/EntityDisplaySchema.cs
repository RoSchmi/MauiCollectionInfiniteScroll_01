using static MauiCollectionInfiniteScroll_01.Models.EntityDisplayFormat;
using System;
using System.Collections.Generic;
using System.Text;
using Azure.Data.Tables;
using RoSchmi.Azure.Converters;

namespace MauiCollectionInfiniteScroll_01.Models
{
    public class EntityDisplaySchema
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

        public int PartitionKeyWidth { get; set; }
        public int RowKeyWidth { get; set; }
        public int TimeStampWidth { get; set; } = TimeStampSize;

        public List<string> ColumnNames { get; } = new();
        public List<int> ColumnWidths { get; } = new();

        public void InitializeFromEntity(TableEntity entity)
        {
            PartitionKeyWidth = Math.Clamp(entity.PartitionKey.Length, PartKeySizeMin, PartKeySizeMax);
            RowKeyWidth = Math.Clamp(entity.RowKey.Length, RowKeySizeMin, RowKeySizeMax);

            ColumnNames.Clear();
            ColumnWidths.Clear();

            foreach (var property in entity)
            {
                if (property.Key is "PartitionKey" or "RowKey" or "Timestamp" or "odata.etag")
                    continue;

                ColumnNames.Add(property.Key);
                ColumnWidths.Add(CalcInitialWidth(property.Key, property.Value));
            }
        }

        public void UpdateWidths(EntityDisplayItem item)
        {
            for (int i = 0; i < ColumnWidths.Count; i++)
            {
                int len = item.GetColumnLength(i);
                if (len > ColumnWidths[i])
                    ColumnWidths[i] = len;
            }
        }

        private int CalcInitialWidth(string name, object value)
        {
            int columnWidth = PropertySizeMin;

            switch (value)
            {
                case DateTimeOffset:
                    columnWidth = TimeStampSize;
                    break;

                case Guid:
                    columnWidth = GuidSize;
                    break;

                default:
                    {
                        string stringValue = EntityPropertyAsString.ConvertProperty(value);
                        string longerString = stringValue.Length >= name.Length ? stringValue : name;

                        columnWidth =
                            longerString.Length > PropertySizeMin
                                ? (longerString.Length <= PropertySizeMax ? longerString.Length : PropertySizeMax)
                                : PropertySizeMin;

                        break;
                    }
            }

            return columnWidth;
        }

        public string BuildHeader(bool showIndex)
        {
            string gapSizeString = new string(' ', GapSize);
            StringBuilder builder = new StringBuilder();

            if (showIndex)
                builder.Append("Index".PadRight(IdxSize)).Append(gapSizeString);

            builder.Append("PartitionKey".PadRight(PartitionKeyWidth)).Append(gapSizeString);
            builder.Append("RowKey".PadRight(RowKeyWidth)).Append(gapSizeString);
            builder.Append("Timestamp".PadRight(TimeStampSize)).Append(gapSizeString);

            for (int i = 0; i < ColumnNames.Count; i++)
                builder.Append(ColumnNames[i].PadRight(ColumnWidths[i])).Append(gapSizeString);

            return builder.ToString();
        }
    }
}

