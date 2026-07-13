using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "FacilityData", menuName = "Scriptable Objects/FacillityData")]
public class FacilityData : BaseItemData
{
    [Header("건물 정보")]
    public FacilityType facilityType;
    public Vector2Int size;

    [Header("건물")]
    public GameObject prefab;

    public override void OnSelected()
    {
       EventBus.Raise(new ItemSelectedEvent(this));
    }
}
