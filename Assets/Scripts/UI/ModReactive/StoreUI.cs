using UnityEngine;

public class StoreUI : ModeReactiveUI<PlacementModeChangedEvent>
{
    [SerializeField] private GameObject[] shopGroup;
    protected override void HandleModeChanged(PlacementModeChangedEvent e)
    {
        foreach(var obj in shopGroup)
        {
            obj.SetActive(!e.IsActive);
        }
    }
}
