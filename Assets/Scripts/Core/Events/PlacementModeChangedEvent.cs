using UnityEngine;
public readonly struct PlacementModeChangedEvent
{
    public readonly bool IsActive;
    public PlacementModeChangedEvent(bool isActive) => IsActive = isActive;
}