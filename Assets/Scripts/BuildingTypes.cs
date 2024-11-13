using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    CraftingTable,  //���۴�
    Funace,           //�뱤��
    Kitchen,           //�ֹ�
    Storage             //â��
}

[System.Serializable]
public class CafetingRecipe
{
    public string itemName;   //������ ������ �̸�
    public ItemType resultItem;  //�����
    public int resultAmount = 1;//����� ����
    public ItemType[] requiredItems;   //�ʿ��� ����
    public int[] requiredAmounts;   //�ʿ��� ��� ����
}