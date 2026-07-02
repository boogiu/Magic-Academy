using UnityEngine;

[CreateAssetMenu(menuName = "Data/Facility Database")]
public class FacilityDatabase : ItemDatabase<FacilityData, FacilityType>
{
    protected override FacilityType GetCategoryOf(FacilityData item) => item.facilityType;
}