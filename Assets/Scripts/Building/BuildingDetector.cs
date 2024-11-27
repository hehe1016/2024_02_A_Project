using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BuildingDetector : MonoBehaviour
{
    public float checkRaius = 3.0f;    //아이템 감지 범위
    public Vector3 lastPostion;        //플레이어의 마지막 위치 (플레이어 이동이 감지될 경우 주변을 찾기 위한 변수)
    public float moveThreshold;        //이동 감지 임계값
    public ConstructibleBuilding currentNearbyBuilding;   //가장 가까이 있는 수집 가능한 아이템
    public BuildingCrafter currentBuildingCrafter;  //추가

    private void CheckForBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRaius);   //감지 범위 내의 모든 콜라이더를 찾아옴

        float closestDistance = float.MaxValue;   //가장 가까운 거리의 초기값
        ConstructibleBuilding closestBuilding = null;        //가장 가까운 아이템의 초기값
        BuildingCrafter closesCrafter = null;

        foreach (Collider collider in hitColliders)  //각 콜라이더를 검사하여 수집 가능한 아이템을 찾음
        {
            ConstructibleBuilding building = collider.GetComponent<ConstructibleBuilding>();         //아이템을 감지
            if (building != null)     //모든 건물 감지로 변경
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);    //거리 계산
                if (distance < closestDistance)     //더 가까운 아이템을 발견시 업데이트
                {
                    closestDistance = distance;
                    closestBuilding = building;
                    closesCrafter = building.GetComponent<BuildingCrafter>();    //여기서 크래프터 가져오기
                }
            }

        }
        if (closestBuilding != currentNearbyBuilding)   //가장 가까운 아이템이 변경 되었을 때 메세지 표시
        {
            currentNearbyBuilding = closestBuilding;   // 가장 가까운 건물 업데이트
            currentBuildingCrafter = closesCrafter;  //추가

            if (currentNearbyBuilding != null && !currentNearbyBuilding.isConstructed)
            {
                if (FloatingTextManager.Instance != null)
                {
                    Vector3 textPositon = transform.position + Vector3.up * 0.5f;
                    FloatingTextManager.Instance.Show($" [F]키로 {currentNearbyBuilding.buildingName} 건설 (나무 {currentNearbyBuilding.requiredTree} 개 필요)", currentNearbyBuilding.transform.position + Vector3.up);
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
        //가까운 아이템이 있고 E키를 눌렀을 때 아이템 수집
        if (currentNearbyBuilding != null && Input.GetKeyDown(KeyCode.F))
        {
            if (!currentNearbyBuilding.isConstructed)
            {
                currentNearbyBuilding.StartConstruction(GetComponent<PlayerInventory>());
            }
            else if (currentBuildingCrafter != null)
            {
                Debug.Log($"{currentNearbyBuilding.buildingName} 의 제작 메뉴 열기");
                CraftingUIManager.Instance?.ShowUI(currentBuildingCrafter);
            }
        }
    }
}
