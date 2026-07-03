using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class FacilitySystem : MonoBehaviour
{
    [SerializeField] private Tilemap editingTile;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private EditModeUI editModeUI;
    [SerializeField] private PreviewObject previewPrefab;

    [SerializeField] private TileBase occupiedMark;
    [SerializeField] private TileBase emptyMark;
    [SerializeField] private TileBase forbidenMark;

    private readonly Dictionary<Vector3Int, GameObject> occupiedCells = new();

    private FacilityData currentFacilityData;
    private PreviewObject previewObject;
    private bool isPreviewing;

    private void OnEnable()
    {
        EventBus.Subscribe<ItemSelectedEvent>(HandleItemSelected);
        if (previewPrefab != null)
        {
            GameObject obj = Instantiate(previewPrefab.gameObject);
            previewObject = obj.GetComponent<PreviewObject>();
            previewObject.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<ItemSelectedEvent>(HandleItemSelected);
    }

    private void Update()
    {
        if (!isPreviewing) return;
        UpdatePreview();

        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
            ConfirmPlacement();
    }

    private void HandleItemSelected(ItemSelectedEvent evt)
    {
        if (evt.itemData is not FacilityData facilityData) return;
        currentFacilityData = facilityData;
        previewObject.InjectData(currentFacilityData);
        EventBus.Raise(new PlacementModeChangedEvent(true));
        StartPreview();
    }

    private void StartPreview()
    {
        if (previewObject != null)
            previewObject.gameObject.SetActive(true);

        editingTile.gameObject.SetActive(true);
        editModeUI.OpenEditUI();

        isPreviewing = true;
    }

    private void UpdatePreview()
    {
        Vector3Int cell = GetOriginCell();
        previewObject.transform.position = GetFootprintOriginWorldPos(cell, currentFacilityData.size);
    }

    private void ConfirmPlacement()
    {
        Vector3Int originCell = GetOriginCell();
        List<Vector3Int> footprintCells = GetFootprintCells(originCell, currentFacilityData.size);

        foreach (var cell in footprintCells)
        {
            if (occupiedCells.ContainsKey(cell)) return;
        }

        Vector3 spawnPos = GetFootprintOriginWorldPos(originCell, currentFacilityData.size);
        if (currentFacilityData.prefab == null) return;

        GameObject placed = Instantiate(currentFacilityData.prefab, spawnPos, Quaternion.identity);
        //GameObject placed = Instantiate(previewPrefab.gameObject, spawnPos, Quaternion.identity);

        foreach (var cell in footprintCells)
        {
            occupiedCells.Add(cell, placed);
            editingTile.SetTile(cell, occupiedMark);
        }
    }

    public void EndPreview()
    {
        if (previewObject != null)
            previewObject.gameObject.SetActive(false);

        currentFacilityData = null;
        isPreviewing = false;

        EventBus.Raise(new PlacementModeChangedEvent(false));
        editingTile.gameObject.SetActive(false);
        editModeUI.CloseEditUI();
    }

    private Vector3Int GetOriginCell()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        return editingTile.WorldToCell(mouseWorldPos);
    }

    private List<Vector3Int> GetFootprintCells(Vector3Int originCell, Vector2Int size)
    {
        var cells = new List<Vector3Int>();

        int startX = -(size.x - 1) / 2; 

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                cells.Add(originCell + new Vector3Int(startX + x, y, 0));
            }
        }

        return cells;
    }

    private Vector3 GetFootprintOriginWorldPos(Vector3Int originCell, Vector2Int size)
    {
        Vector3 cellBottomLeft = editingTile.CellToWorld(originCell);
        Vector3 offset = new Vector3(editingTile.cellSize.x * 0.5f, 0f, 0f); 
        return cellBottomLeft + offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();
        screenPos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(screenPos);
    }
}