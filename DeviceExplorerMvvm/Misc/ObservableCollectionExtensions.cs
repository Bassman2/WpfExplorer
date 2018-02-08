using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeviceExplorer.Misc
{
    public static class ObservableCollectionExtensions
    {
        public static void Update<T>(this ObservableCollection<T> col, IEnumerable<T> en) where T : IEquatable<T>
        {
            en = en.ToList();
            var del = col.Where(e => !en.Contains(e)).ToList();
            var add = en.Where(e => !col.Contains(e)).ToList();

            del.ForEach(e => col.Remove(e));
            add.ForEach(e => col.Add(e));
        }

        public static void UpdateSorted<T>(this ObservableCollection<T> col, IEnumerable<T> en) where T : IEquatable<T>, IComparable<T>
        {
            var del = col.Where(e => !en.Contains(e)).ToList();
            var add = en.Where(e => !col.Contains(e)).ToList();

            del.ForEach(e => col.Remove(e));
            add.ForEach(e => col.Insert(SortIndex(col, e), e));
        }

        private static int SortIndex<T>(this ObservableCollection<T> col, T e) where T : IComparable<T>
        {
            for (int i = 0; i < col.Count; i++)
            {
                if (e.CompareTo(col[i]) < 0)
                {
                    return i;
                }
            }
            return col.Count; // add to end
        }
    }
}
