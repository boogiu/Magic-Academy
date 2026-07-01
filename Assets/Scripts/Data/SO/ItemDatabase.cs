// Assets/Scripts/Data/Item/ItemDatabase.cs
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDatabase<TData, TCategory> : ItemDatabaseBase
    where TData : BaseItemData
    where TCategory : System.Enum
{
    [SerializeField]
    protected List<TData> allItems;

    private Dictionary<TCategory, List<TData>> cache;

    protected abstract TCategory GetCategoryOf(TData item);

    private void BuildCacheIfNeeded()
    {
        if (cache != null) return;

        cache = new Dictionary<TCategory, List<TData>>();
        foreach (var item in allItems)
        {
            var category = GetCategoryOf(item);
            if (!cache.TryGetValue(category, out var list))
            {
                list = new List<TData>();
                cache[category] = list;
            }
            list.Add(item);
        }
    }

    public List<TData> GetByCategory(TCategory category)
    {
        BuildCacheIfNeeded();
        return cache.TryGetValue(category, out var list) ? list : new List<TData>();
    }

    public override int CategoryCount => System.Enum.GetValues(typeof(TCategory)).Length;

    public override string GetCategoryName(int categoryIndex)
    {
        var values = (TCategory[])System.Enum.GetValues(typeof(TCategory));
        return values[categoryIndex].ToString();
    }

    public override IReadOnlyList<BaseItemData> GetItemsByCategoryIndex(int categoryIndex)
    {
        var values = (TCategory[])System.Enum.GetValues(typeof(TCategory));
        return GetByCategory(values[categoryIndex]);
    }
}