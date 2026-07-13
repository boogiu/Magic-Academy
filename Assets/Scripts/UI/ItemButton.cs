using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private BaseItemData baseItemData;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image iconSprite;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        if(button != null )
            button.onClick.AddListener(HandleClick);
    }
    private void OnDestroy()
    {
        if(button != null )
            button.onClick.RemoveListener(HandleClick);
    }

    public void InjectData(BaseItemData _baseItemData)
    {
        baseItemData = null;
        if (_baseItemData == null) return;
        baseItemData = _baseItemData;
        nameText.text = baseItemData.itemName;
        iconSprite.sprite = baseItemData.sprite;
        costText.text = baseItemData.itemCost.ToString() +" °ñµå";
    }

    private void HandleClick()
    {
        if (baseItemData == null) return;
        baseItemData.OnSelected();
    }
}
