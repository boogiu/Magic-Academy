using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class FacilitySystem : MonoBehaviour
{
    [SerializeField] private Tilemap targetTileMap;
    [SerializeField] private Camera mainCamera;

    private FacilityData currentFacilityData;
    private GameObject previewObject;
    private SpriteRenderer previewRenderer;
    private bool isPreviewing;

    private void OnEnable()
    {
        EventBus.Subscribe<ItemSelectedEvent>(HandleItemSelected);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<ItemSelectedEvent>(HandleItemSelected);
    }

    private void Update()
    {
        if (!isPreviewing) return;
        UpdatePreview();

        if (Mouse.current.leftButton.wasPressedThisFrame)
            ConfirmPlacement();
    }
    private void HandleItemSelected(ItemSelectedEvent evt)
    {
        if (evt.itemData is not FacilityData facilityData) return;
        currentFacilityData = facilityData;
        EventBus.Raise(new PlacementModeChangedEvent(true));
        StartPreview();
    }

    private void StartPreview()
    {
        previewObject = new GameObject("FacilityPreview");
        previewRenderer = previewObject.AddComponent<SpriteRenderer>();
        previewRenderer.sprite = currentFacilityData.sprite;
        previewRenderer.color = new Color(1f, 1f, 1f, 0.5f); // π›≈ı∏Ì

        isPreviewing = true;
    }

    private void UpdatePreview()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3Int cell = targetTileMap.WorldToCell(mouseWorldPos);
        Vector3 visualPos = targetTileMap.GetCellCenterWorld(cell);
        previewObject.transform.position = visualPos;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private void ConfirmPlacement()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        Vector3Int cell = targetTileMap.WorldToCell(mouseWorldPos);
        if (targetTileMap.HasTile(cell)) return;
        targetTileMap.SetTile(cell, currentFacilityData.tile);
        EndPreview();
    }

    private void EndPreview()
    {
        Destroy(previewObject);
        previewObject = null;
        previewRenderer = null;
        currentFacilityData = null;
        isPreviewing = false;

        EventBus.Raise(new PlacementModeChangedEvent(false));
    }
}
