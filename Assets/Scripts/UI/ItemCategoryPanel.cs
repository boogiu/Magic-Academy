using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCategoryPanel : MonoBehaviour
{
    [SerializeField]
    private ItemDatabaseBase itemDatabase;

    [Header("Category")]
    [SerializeField]
    private Transform categoryContent;
    [SerializeField]
    private Button categoryButton;
    private List<Button> categoryButtons = new();

    [Header("item")]
    [SerializeField]
    private MovealbePanel itemPanel;
    [SerializeField]
    private Transform itemContent;
    [SerializeField]
    private ItemButton itemButton;
    private List<ItemButton> itemButtons = new();


    private void Awake()
    {
        SpawnCategorys();
    }

    public void SpawnCategorys()
    {
        Clear();
        for (int i = 0; i < itemDatabase.CategoryCount; ++i)
        {
            Button btn = Instantiate(categoryButton, categoryContent);
            btn.GetComponentInChildren<TMP_Text>().text = itemDatabase.GetCategoryName(i);

            int capturedIndex = i;
            btn.onClick.AddListener(() => OnCategorySelected(capturedIndex));
            btn.onClick.AddListener(() => itemPanel.OpenPanel());

            categoryButtons.Add(btn);
        }
    }

    private void Clear()
    {
        foreach (var btn in categoryButtons)
            Destroy(btn.gameObject);
        categoryButtons.Clear();
    }

    public void OnCategorySelected(int index)
    {
        ClearItems();
        var items = itemDatabase.GetItemsByCategoryIndex(index);
        if (items.Count == 0)
        {
            Debug.Log($"{itemDatabase.GetCategoryName(index)}: 아이템 없음");
            return;
        }

        Debug.Log($"{items[0].itemName} 선택됨");
        SpawnItemButtonsIfNeed(items);
    }

    private void SpawnItemButtonsIfNeed(IReadOnlyList<BaseItemData> list)
    {
        if (list.Count == 0) return;
        if (list.Count > itemButtons.Count)
        {
            int needMore = list.Count - itemButtons.Count;
            for (int i = 0; i < needMore; ++i)
            {
                ItemButton btn = Instantiate(itemButton, itemContent);
                btn.gameObject.SetActive(false);
                itemButtons.Add(btn);
            }
        }

        for (int i = 0; i < list.Count; ++i)
        {
            itemButtons[i].gameObject.SetActive(true);
            itemButtons[i].InjectData(list[i]);
        }
    }

    private void ClearItems()
    {
        foreach (var item in itemButtons)
        {
            item.gameObject.SetActive(false);
        }
    }

}