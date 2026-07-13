using System.Reflection.Metadata.Ecma335;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer previewImage;
    [SerializeField]
    private SpriteRenderer tilePreview;
    [SerializeField]
    private Vector2Int size;

    public Vector2Int GetSize() => size;

    public void InjectData(FacilityData data)
    {
        previewImage.sprite = data.sprite;
        tilePreview.size = data.size;
    }
}
