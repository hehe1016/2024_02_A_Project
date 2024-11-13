using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRaius = 3.0f;    //������ ���� ����
    public Vector3 lastPostion;        //�÷��̾��� ������ ��ġ (�÷��̾� �̵��� ������ ��� �ֺ��� ã�� ���� ����)
    public float moveThreshold;        //�̵� ���� �Ӱ谪
    public ConstructibleBuilding currentNearbyBuilding;   //���� ������ �ִ� ���� ������ ������

    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRaius);   //���� ���� ���� ��� �ݶ��̴��� ã�ƿ�

        float closestDistance = float.MaxValue;   //���� ����� �Ÿ��� �ʱⰪ
        ConstructibleBuilding closestBuilding = null;        //���� ����� �������� �ʱⰪ

        foreach (Collider collider in hitColliders)  //�� �ݶ��̴��� �˻��Ͽ� ���� ������ �������� ã��
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();         //�������� ����
            if (building != null && building.canBuild && !building.isConstructed)     //�������� �ְ� ������ �������� Ȯ��
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);    //�Ÿ� ���
                if (distance < closestDistance)     //�� ����� �������� �߽߰� ������Ʈ
                {
                    closestDistance = distance;
                    closestBuilding = building;
                }
            }

        }
        if (closestBuilding != currentNearbyBuilding)   //���� ����� �������� ���� �Ǿ��� �� �޼��� ǥ��
        {
            currentNearbyBuilding = closestBuilding;   // ���� ����� �ǹ� ������Ʈ
            if (currentNearbyBuilding != null)
            {
                if (FloatingTextManager.Instance != null)
                {
                    Vector3 textPositon = transform.position + Vector3.up * 0.5f;
                    FloatingTextManager.Instance.Show($" [F]Ű�� {currentNearbyBuilding.buildingName} �Ǽ� (���� {currentNearbyBuilding.requiredTree} �� �ʿ�)", currentNearbyBuilding.transform.position + Vector3.up);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPostion = transform.position;
        CheckForBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPostion, transform.position) > moveThreshold)
        {
            CheckForBuilding();
            lastPostion = transform.position;
        }
        //����� �������� �ְ� EŰ�� ������ �� ������ ����
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            currentNearbyBuilding.StartConstruction(GetComponent <PlayerInventory>());
        }
    }
}
