using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 종류 정의
public enum ItemType
{
    Crystal,                   //크리스탈
    Plant,                     //식물
    Bush,                      //수풀
    Tree,                      //나무
}
public class ItemDetector : MonoBehaviour
{
    public float checkRaius = 3.0f;    //아이템 감지 범위
    public Vector3 lastPostion;        //플레이어의 마지막 위치 (플레이어 이동이 감지될 경우 주변을 찾기 위한 변수)
    public float moveThreshold;        //이동 감지 임계값
    public CollectibleItem currentNearbyItem;   //가장 가까이 있는 수집 가능한 아이템

    // Start is called before the first frame update
    void Start()
    {
        lastPostion = transform.position;         //시작 시 현재 위치를 마지막 위치로 설정
        CheckForItems();
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 일정 거리 이상 이동했는지 체크
        if (Vector3.Distance(lastPostion, transform.position) > moveThreshold)
        {
            CheckForItems();                    //이동시 아이템 체크
            lastPostion = transform.position;   //현재 위치를 마지막 위치로 업데이트
        }

        if (currentNearbyItem != null && Input.GetKeyDown(KeyCode.E))            //가까운 아이템이 있고 E키를 눌렀을 때 아이템 수집
        {
            currentNearbyItem.CollectorItem(GetComponent<PlayerInventory>());    //PlayerInventory 참조하여 아이템 수집
        }
    }

    //주변에 수집 가능한 아이템을 감지하는 함수
    private void CheckForItems()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRaius);   //감지 범위 내의 모든 콜라이더를 찾아옴

        float closestDistance = float.MaxValue;   //가장 가까운 거리의 초기값
        CollectibleItem closestItem = null;        //가장 가까운 아이템의 초기값

        foreach (Collider collider in hitColliders)  //각 콜라이더를 검사하여 수집 가능한 아이템을 찾음
        {
            CollectibleItem item = collider.GetComponent<CollectibleItem>();         //아이템을 감지
            if (item != null && item.canCollect)     //아이템이 있고 수집이 가능한지 확인
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);    //거리 계산
                if (distance < closestDistance)     //더 가까운 아이템을 발견시 업데이트
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }

        }
        if (closestItem != currentNearbyItem)   //가장 가까운 아이템이 변경 되었을 때 메세지 표시
        {
            currentNearbyItem = closestItem;   // 가장 가까운 아이템 업데이트
            if (currentNearbyItem != null)
            {
                Debug.Log($" [E] 키를 눌러 {currentNearbyItem.itemName} 수집");     // 새로운 아이템 수집 메세지 표시
            }
        }
    }

    private void OnDrawGizmos()    //유니티 Scene 창에 보이는 Debug 그림
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkRaius);
    }
}
