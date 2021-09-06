﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 필드맵 UI
/// </summary>
public class UIFieldMap : MonoBehaviour
{
    public Transform ipiaTransform;

    public List<Transform> stageTransformList = new List<Transform>();
    [SerializeField]
    private int currentStageNumber;

    [SerializeField, Tooltip("이동키를 입력할 수 있는 상태인가?")]
    private bool canInputKey;


    private void Start()
    {
        currentStageNumber = DataManager.Instance.currentData_player.currentStageNumber;
        StartCoroutine(ProcessInputMoveKey());
    }


    private IEnumerator ProcessInputMoveKey()
    {
        while (true)
        {
            if (!canInputKey)
            {
                yield return null;
                continue;
            }

            //엔터키
            if (Input.GetKeyDown(KeyCode.Return))
            {
                canInputKey = false;
                SceneChanger.Instance.LoadThisScene(GetSceneNameUseStageNumber(currentStageNumber));
                yield break;
            }

            //왼쪽
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                yield return TryMoveCharacter("Right");

            }

            //오른쪽
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                yield return TryMoveCharacter("Left");
            }

            yield return null;
        }
    }

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private float moveSpeed = 1f;
    private IEnumerator TryMoveCharacter(string _moveDir)
    {

        int moveDir = 0;

        //이동 방향 정하기
        switch (_moveDir)
        {
            case "Right":
                moveDir = 1;
                break;

            case "Left":
                moveDir = -1;
                break;

            default:
                break;
        }

        DataManager dataManager = DataManager.Instance;
        int moveNumber = moveDir + dataManager.currentData_player.currentStageNumber;

        //0보다 작거나, 클리어하지 못한 스테이지거나, 전체 스테이지 수보다 크면
        if (moveNumber < 0
            || moveNumber > dataManager.currentData_player.finalStageNumber + 1
            || moveNumber > stageTransformList.Count - 1)
        {
            Debug.LogWarning("이동할 수 없어!");
            yield break;
        }

        
        //지나갈 수 있다면...지나가야지!!
        
        //이동키 입력 방지
        canInputKey = false;

        //현재 위치 설정
        originalPosition = ipiaTransform.position;

        //목표 위치 설정 
        targetPosition = stageTransformList[moveNumber].position;
        float distance = 25252;

        //일정 거리 안에 들어올 때 까지
        while (distance < 0.1f)
        {
            distance = Vector2.Distance(ipiaTransform.position, targetPosition);

            ipiaTransform.position = Vector2.MoveTowards(originalPosition, targetPosition, moveSpeed);
            yield return YieldInstructionCache.WaitForFixedUpdate;
        }

        //혹시 모르니까 정확하게 딱! 해주기
        ipiaTransform.position = targetPosition;

        //현재 스테이지 넘버를 움직인 스테이지 넘버로 변경
        currentStageNumber = moveNumber;

        //이동키 입력 가능
        canInputKey = true;
        yield break;
    }

    /// <summary>
    /// 번호를 주면 씬이름을 슝~
    /// </summary>
    /// <param name="_number"></param>
    /// <returns></returns>
    private string GetSceneNameUseStageNumber(int _number)
    {
        switch (_number)
        {
            case 0:
            default:
                return "Main";
        }
    }
}
