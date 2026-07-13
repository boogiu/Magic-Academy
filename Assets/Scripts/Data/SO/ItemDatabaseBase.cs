// Assets/Scripts/Data/Item/ItemDatabaseBase.cs
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDatabaseBase : ScriptableObject
{
    public abstract int CategoryCount { get; }
    public abstract string GetCategoryName(int categoryIndex);
    public abstract IReadOnlyList<BaseItemData> GetItemsByCategoryIndex(int categoryIndex);
}