using UnityEngine;

[CreateAssetMenu(menuName = "Data/Facility Database")]
public class FacilityDatabase : ItemDatabase<FacillityData, FacilityType>
{
    protected override FacilityType GetCategoryOf(FacillityData item) => item.facilityType;
}