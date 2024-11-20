using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ���� ����
public enum ItemType
{
    Crystal,                   //ũ����Ż
    Plant,                     //�Ĺ�
    Bush,                      //��Ǯ
    Tree,                      //����
    VeagetableStew,            //��ä ��Ʃ (��� ȸ����)
    FruitSalad,                //
    RepairKit
}
public class ItemDetector : MonoBehaviour
{
    public float checkRaius = 3.0f;    //������ ���� ����
    public Vector3 lastPostion;        //�÷��̾��� ������ ��ġ (�÷��̾� �̵��� ������ ��� �ֺ��� ã�� ���� ����)
    public float moveThreshold;        //�̵� ���� �Ӱ谪
    public CollectibleItem currentNearbyItem;   //���� ������ �ִ� ���� ������ ������

    // Start is called before the first frame update
    void Start()
    {
        lastPostion = transform.position;         //���� �� ���� ��ġ�� ������ ��ġ�� ����
        CheckForItems();
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾ ���� �Ÿ� �̻� �̵��ߴ��� üũ
        if (Vector3.Distance(lastPostion, transform.position) > moveThreshold)
        {
            CheckForItems();                    //�̵��� ������ üũ
            lastPostion = transform.position;   //���� ��ġ�� ������ ��ġ�� ������Ʈ
        }

        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))            //����� �������� �ְ� EŰ�� ������ �� ������ ����
        {
            currentNearbyItem.CollectorItem(GetComponent<PlayerInventory>());    //PlayerInventory �����Ͽ� ������ ����
        }
    }

    //�ֺ��� ���� ������ �������� �����ϴ� �Լ�
    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRaius);   //���� ���� ���� ��� �ݶ��̴��� ã�ƿ�

        float closestDistance = float.MaxValue;   //���� ����� �Ÿ��� �ʱⰪ
        CollectibleItem closestItem = null;        //���� ����� �������� �ʱⰪ

        foreach (Collider collider in hitColliders)  //�� �ݶ��̴��� �˻��Ͽ� ���� ������ �������� ã��
        {
            CollectibleItem item = collider.GetComponent<CollectibleItem>();         //�������� ����
            if (item != null && item.canCollect)     //�������� �ְ� ������ �������� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);    //�Ÿ� ���
                if (distance < closestDistance)     //�� ����� �������� �߽߰� ������Ʈ
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }

        }
        if (closestItem != currentNearbyItem)   //���� ����� �������� ���� �Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyItem = closestItem;   // ���� ����� ������ ������Ʈ
            if (currentNearbyItem != null)
            {
                Debug.Log($" [E] Ű�� ���� {currentNearbyItem.itemName} ����");     // ���ο� ������ ���� �޼��� ǥ��
            }
        }
    }

    private void OnDrawGizmos()    //����Ƽ Scene â�� ���̴� Debug �׸�
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRaius);
    }
}
