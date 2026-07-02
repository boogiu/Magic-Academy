using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "FacilityData", menuName = "Scriptable Objects/FacillityData")]
public class FacilityData : BaseItemData
{
    [Header("건물 정보")]
    public FacilityType facilityType;
    public Vector2Int size;
    public Tile tile;

    public override void OnSelected()
    {
        Debug.Log("선택됨!");
       EventBus.Raise(new ItemSelectedEvent(this));
    }
}
