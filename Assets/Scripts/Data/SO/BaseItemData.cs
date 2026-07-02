// Assets/Scripts/Facility/FacilityData.cs
using UnityEngine;

public abstract  class BaseItemData : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public int itemCost;

    [Header("비주얼")]
    public Sprite sprite;

    public abstract void OnSelected();
}
