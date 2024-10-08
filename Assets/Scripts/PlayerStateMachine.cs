using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //현재 플레이어의 상태를 나타내는 변수

    public PlayerState currentState;             //현재 플레이어의 상태를 나타내는 변수
    public PlayerController playerController;    //플레이어 참조용

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();   //플레이어 컴포넌트 참조
    }

    // Start is called before the first frame update
    void Start()
    {
        TransitionToState(new IdleState(this));     //초기 상태를 IdleState로 설정
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)    //현재 상태가 존재 한다면
        {
            currentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)    //현재 상태가 존재 한다면
        {
            currentState.FixedUpdate();
        }
    }

    //TransitionToState 새로운 상태로 전화하는 메서드
    public void TransitionToState(PlayerState newstate) 
    {
        currentState?.Exit();                                           //현재 상태가 존재한다면 [?] If 문 처럼 쓰임 (상태 종료)
        currentState = newstate;                                        //새로운 상태로 전환
        currentState.Enter();                                           //상태 시작
        Debug.Log($"Transitioned to State {newstate.GetType().Name}");  //관련 내용 로그로 출력
    }
}
