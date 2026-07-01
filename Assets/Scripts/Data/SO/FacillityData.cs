using UnityEngine;

[CreateAssetMenu(fileName = "FacillityData", menuName = "Scriptable Objects/FacillityData")]
public class FacillityData : BaseItemData
{
    [Header("건물 정보")]
    public FacilityType facilityType;
    public Vector2Int size;
    public int buildCost;
}
