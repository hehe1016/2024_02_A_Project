using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//GridCell 클래스는 각 그리드 셀의 상태와 데이터를 관리 합니다.
public class GridCell
{
    public Vector3Int Position;
    public bool IsOccupied;
    public GameObject Building;

    public GridCell(Vector3Int position)          //클래스 이름과 동일한 함수 (생성자) 클래스가 생성될때 호출
    {
        Position = position;
        IsOccupied = false;
        Building = null;
    }
}
public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private int width = 10;   //그리드의 가로 크기
    [SerializeField] private int height = 10;   //그리드의 세로 크기
    [SerializeField] float cellSize = 1.0f;   //각 셀의 크기
    [SerializeField] private GameObject cellPrefabs; // 셀 프리팹
    [SerializeField] private GameObject builingPrefabs; // 빌딩 프리팹

    [SerializeField] private PlayerController playerController;    //플레이어 컨트롤러 참조

    [SerializeField] private Grid grid;
    private GridCell[,] cells;                       //GridCell 클래스를 2차원 배열로 선언
    private Camera firstPersonCamera;

    // Start is called before the first frame update
    void Start()
    {
        firstPersonCamera = playerController.firstPersonCamera;    //플레이어의 카메라 객체를 가져온다
        CreateGrid();
    }

    //그리드를 생성하고 셀을 초기화하는 메서드
    private void CreateGrid()
    {
        grid.cellSize = new Vector3(cellSize, cellSize, cellSize);

        cells = new GridCell[width, height];
        Vector3 gridCenter = playerController.transform.position;    //플레이어의 위치를 받아와서
        gridCenter.y = 0;
        transform.position = gridCenter - new Vector3(width * cellSize / 2.0f, 0, height * cellSize / 2.0f); //플레이어 정중앙 기준

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                Vector3Int cellPosition = new Vector3Int(x, 0, z);  //셀의 위치
                Vector3 worldPosition = grid.GetCellCenterWorld(cellPosition); //그리
                GameObject cellObject = Instantiate(cellPrefabs, worldPosition, cellPrefabs.transform.rotation);
                cellObject.transform.SetParent(transform);

                cells[x, z] = new GridCell(cellPosition);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookPosition = GetLookPosition();
        if(lookPosition != Vector3.zero)                                //보고 있는 좌표가 있는지 검사
        {
            Vector3Int gridPosition = grid.WorldToCell(lookPosition);   //그리드 월드 포지션 전환
            if (isValidGridPosition(gridPosition))                      //위치가 유효 한지 확인
            {
                HighlightCell(gridPosition);
                if(Input.GetMouseButton(0))
                {
                    PlaceBuilding(gridPosition);
                }
                if(Input.GetMouseButton(1))
                {
                    RemoveBuilding(gridPosition);
                }
            }
        }
    }

    //그리드 셀에 건물을 배치하는 메서드
    private void PlaceBuilding(Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];         //위치 기반으로 cell을 받아온다.
        if(!cell.IsOccupied)                                           //해당 위치에 건물이 있는지 확인한다.
        {
            Vector3 worldPosition = grid.GetCellCenterLocal(gridPosition);
            GameObject building = Instantiate(builingPrefabs, worldPosition,Quaternion.identity);  //건물을 생성
            cell.IsOccupied = true;                                                                //건물 확인 값
            cell. Building = building;                                                             //Cell에 놓인 빌딩
        }
    }

    //그리드 셀에서 건물을 제거하는 메서드
    private void RemoveBuilding (Vector3Int gridPosition)
    {
        GridCell cell = cells[gridPosition.x, gridPosition.z];         //위치 기반으로 cell을 받아온다.
        if (!cell.IsOccupied)                                           //해당 위치에 건물이 있는지 확인한다.
        {
            Destroy(cell.Building);             //cell 건물을 제거한다.
            cell.IsOccupied = false;            //건물 확인 값
            cell. Building = null;              //cell에 놓인 빌딩 null 값으로 변경
        }
    }
    //선택된 셀을 하이라이트하는 메서드
    private void HighlightCell(Vector3Int gridPosition)
    {
        for (int x = 0; x < width;x++)     //Cell을 돌면서
        {
            for(int z = 0; z < height;z++)
            {
                //건물이 없으면 하얀색으로
                GameObject cellObject = cells[x, z].Building != null ? cells[x, z].Building : transform.GetChild(x * height + z).gameObject;
                cellObject.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        //특정 셀에 건물이 있으면 빨간색 아니면 초록색
        GridCell cell = cells[gridPosition.x, gridPosition.z];
        GameObject highlightObject = cell.Building != null ? cell.Building : transform.GetChild(gridPosition.x * height + gridPosition.z).gameObject;
        highlightObject.GetComponent<Renderer>().material.color = cell.IsOccupied ? Color.red : Color.green;
    }

    //그리드 포지셔이 유효한지 확인하는 메서드
    private bool isValidGridPosition(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width &&
            gridPosition.z >= 0&& gridPosition.z < height;
    }


    // 플레이어가 보고 있는 위치를 계산하는 메서드
    private Vector3 GetLookPosition()
    {
        if (playerController.isFirstPerson)    //1인칭 모드일 경우
        {
            Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);  //카메라 앞 방향으로 Ray를 쏜다.
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red);  //Ray 정보를 보여준다.
                return hitInfo.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white); //Ray 정보를 보여준다.
            }
        }
        //3인칭 모드
        else
        {
            Vector3 characterPosition = playerController.transform.position;            //플레이어의 위치
            Vector3 characterFoward = playerController.transform.forward;               //플레이어의 앞 방향 
            Vector3 rayOrigin = characterPosition + Vector3.up * 1.5f + characterFoward * 0.5f;   //캐릭터 위쪽
            Vector3 rayDirection = (characterFoward - Vector3.up).normalized;                     //캐릭터 보는 방향 앞 대각선

            Ray ray = new Ray (rayOrigin, rayDirection);

            if(Physics.Raycast(ray,out RaycastHit hitInfo,5.0f))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.blue); //Ray 정보를 보여준다
                return hitInfo.point;
            }    
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.white); //Ray 정보를 보여준다ㅣ
            }    
        }    
        return Vector3.zero;
    }   

    //그리드 셀을 Glzmo로 표기하는 메서드
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 cellCenter = grid.GetCellCenterWorld(new Vector3Int(x, 0, z));
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}
