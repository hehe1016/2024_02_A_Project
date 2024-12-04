using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI countText;
    public Button useButton;

    private ItemType itemType;
    private int itemCount;

    public void Setup (ItemType type, int Count)
    {
        itemType = type;
        itemCount = Count;

        itemNameText.text = GetItemDisplayName (type);
        countText.text = Count.ToString ();

        useButton.onClick.AddListener(UseItem);
    }

    private string GetItemDisplayName(ItemType type)
    {
        switch (type)
        {
            case ItemType.VeagetableStew: return "야채 스튜";
            case ItemType.FruitSalad: return "과일 샐러드";
            case ItemType.RepairKit: return "수리 키트";
            default: return type.ToString();
        }
    }

    private void UseItem()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        SurvivalStats stats = FindObjectOfType<SurvivalStats>();

        switch(itemType)
        {
            case ItemType.VeagetableStew:
                if(inventory.RemoveItem(itemType, 1))
                {
                    stats.EatFood(40f);
                    InventoryUIManager.Instance.RefreshInventory();
                }
                break;

            case ItemType.FruitSalad:
                if (inventory.RemoveItem(itemType, 1))
                {
                    stats.EatFood(50f);
                    InventoryUIManager.Instance.RefreshInventory();
                }
                break;

            case ItemType.RepairKit:
                if (inventory.RemoveItem(itemType, 1))
                {
                    stats.EatFood(25f);
                    InventoryUIManager.Instance.RefreshInventory();
                }
                break;
        }
    }
}
