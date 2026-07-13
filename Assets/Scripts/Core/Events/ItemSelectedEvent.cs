using UnityEngine;

public class ItemSelectedEvent
{
    public readonly BaseItemData itemData;
    public ItemSelectedEvent(BaseItemData _itemData) => itemData = _itemData;
}
