using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    private BaseItemData baseItemData;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image iconSprite;
    [SerializeField] private TextMeshProUGUI costText;


    public void InjectDate(BaseItemData _baseItemData)
    {
        baseItemData = null;
        if (_baseItemData == null) return;
        baseItemData = _baseItemData;
        nameText.text = baseItemData.itemName;
        iconSprite.sprite = baseItemData.sprite;
        costText.text = baseItemData.itemCost.ToString() +" °ñµå";
    }
}
