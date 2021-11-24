using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    private UIMovieScreen movieScreen;
    private UIEndingCredit endingCredit;
    private IEnumerator Start()
    {
        //로딩창이 사라질때까지 대기
        yield return new WaitWhile(() => SceneChanger.Instance.isLoading);
        
        //무비스크린 데려오기
        movieScreen = UIManager.Instance.GetUI("UIMovieScreen") as UIMovieScreen;

        //무비가 끝나면 할짓 설정
        movieScreen.onMovieEnded += MovieScreen_onMovieEnded;
    }

    private void MovieScreen_onMovieEnded()
    {
        endingCredit = UIManager.Instance.GetUI("UIEndingCredit") as UIEndingCredit;

    }
}
