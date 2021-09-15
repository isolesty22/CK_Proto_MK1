﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 시작 연출등
/// </summary>
public class StageStarter : MonoBehaviour
{
    [Header("Null Check")]
    public bool isDebug;

    [Header("Fade Out이전 대기 시간")]
    [Tooltip("스테이지 시작 후 출력되는 시작 UI가 출력되어있는 시간")]
    public float waitTime;

    [Header("Stage Start UI")]
    public UIStageStart uiStageStart;
    public void Start()
    {
        StartCoroutine(WaitSceneLoading());
    }

    private IEnumerator WaitSceneLoading()
    {
        if (isDebug)
        {
            if (SceneChanger.Instance != null)
            {
                while (SceneChanger.Instance.isLoading)
                {
                    Debug.Log("StageStarter : waiting...");
                    yield return null;
                }
            }
        }
        else
        {
            while (SceneChanger.Instance.isLoading)
            {
                Debug.Log("StageStarter : waiting...");
                yield return null;
            }
        }
        //로딩이 끝날 때 까지 대기

        yield return new WaitForSeconds(0.5f);
        //StageStart 열기.
        //UIManager랑 상관없어야하므로 그냥 Open 호출
        uiStageStart.Open();

        //Fade할 시간 정하기
        uiStageStart.SetFadeDuration(0.5f);

        //대기
        //일시정지에 영향을 받으면 좋을것같아서
        float totalWaitTime = uiStageStart.GetFadeDuration() + waitTime;
        yield return new WaitForSeconds(totalWaitTime);

        uiStageStart.Close();

    }
}
