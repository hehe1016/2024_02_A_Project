using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private SurvivalStats survivalStats;

    //���� ������ ������ �����ϴ� ����
    public int crystalCount = 0;          //ũ����Ż ����
    public int plantCount = 0;           //�Ĺ� ����
    public int bushCount = 0;             //��Ǯ ����
    public int treeCount = 0;             //���� ����

    public void Start()
    {
        survivalStats = GetComponent<SurvivalStats>();
    }
    public void UseItem(ItemType itemType)
    {
        if (GetItemCount(itemType) <= 0)
        {
            return;
        }

        switch (itemType)
        {
            case ItemType.VeagetableStew:
                RemoveItem(ItemType.VeagetableStew, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[0].hungerResotreAmount);   //������ ��ġ ����
                break;
            case ItemType.FruitSalad:
                RemoveItem(ItemType.FruitSalad, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[1].hungerResotreAmount);   //������ ��ġ ����
                break;
            case ItemType.RepairKit:
                RemoveItem(ItemType.RepairKit, 1);
                survivalStats.EatFood(RecipeList.KitchenRecipes[0].hungerResotreAmount);   //������ ��ġ ����
                break;
        }
    }

    public void AddItem(ItemType itemType, int amount)
    {
        //amount ��ŭ ������ AddItem ȣ��
        for (int i = 0; i < amount; i++)
        {
            AddItem(itemType);
        }
    }

    public bool RemoveItem(ItemType itemType, int amount = 1)
    {
        //������ ������ ���� �ٸ� ���� ����
        switch (itemType)
        {
            case ItemType.Crystal:
                if (crystalCount >= amount)
                {
                    crystalCount -= amount;
                    Debug.Log($"ũ����Ż {amount} ���! ���� ���� : {crystalCount}");
                    return true;
                }
                break;
            case ItemType.Plant:
                if (plantCount >= amount)
                {
                    plantCount -= amount;
                    Debug.Log($"�Ĺ� {amount} ���! ���� ���� : {plantCount}");
                    return true;
                }
                break;
            case ItemType.Bush:
                if (bushCount >= amount)
                {
                    bushCount -= amount;
                    Debug.Log($"��Ǯ {amount} ���! ���� ���� : {bushCount}");
                    return true;
                }
                break;
            case ItemType.Tree:
                if (treeCount >= amount)
                {
                    treeCount -= amount;
                    Debug.Log($"���� {amount} ���! ���� ���� : {treeCount}");
                    return true;
                }
                break;
        }
        return true;
    }

    //�������� �߰��ϴ� �Լ�, ������ ������ ���� �ش� �������� ������ ���� ��Ŵ
    public void AddItem(ItemType itemType)
    {
        //������ ������ ���� �ٸ� ���� ����
        switch (itemType)
        {
            case ItemType.Crystal:          
                crystalCount++;                               //ũ����Ż ���� ����
                Debug.Log($"ũ����Ż ȹ��! ���� ���� : {crystalCount}");   // ���� ũ����Ż ���� ���
                break;
            case ItemType.Plant:
                plantCount++;                               //�Ĺ� ���� ����
                Debug.Log($"�Ĺ� ȹ��! ���� ���� : {plantCount}");   // ���� �Ĺ� ���� ���
                break;
            case ItemType.Bush:
                bushCount++;                               //��Ǯ ���� ����
                Debug.Log($"��Ǯ ȹ��! ���� ���� : {bushCount}");   // ���� ��Ǯ ���� ���
                break;
            case ItemType.Tree:
                treeCount++;                               //���� ���� ����
                Debug.Log($"���� ȹ��! ���� ���� : {treeCount}");   // ���� ���� ���� ���
                break;
        }
    }

    public int GetItemCount(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Crystal:
                return crystalCount;
            case ItemType.Plant:
                return plantCount;
            case ItemType.Bush:
                return bushCount;
            case ItemType.Tree:
                return treeCount;
            default:
                return 0;
        }
    }

    void Update()
    {
        //Ű�� �������� �κ��丮 �α� ������ ������
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory();
        }
    }

    private void ShowInventory()
    {
        Debug.Log("========�κ��丮========");
        Debug.Log($"ũ����Ż:{crystalCount}��");  //ũ����Ż ���� ���
        Debug.Log($"�Ĺ�:{plantCount}��");  //�Ĺ� ���� ���
        Debug.Log($"��Ǯ:{bushCount}��");  //��Ǯ ���� ���
        Debug.Log($"����:{treeCount}��");  //���� ���� ���
        Debug.Log($"=======================");
    }
}
