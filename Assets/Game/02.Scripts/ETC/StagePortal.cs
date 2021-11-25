using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 닿으면 게임매니저의 StageClear를 호출합니다. 
/// </summary>
public class StagePortal : MonoBehaviour
{

    public GameObject parentObject;
    public Collider col;

    public CanvasGroup canvasGroup;
    public Image image;

    public float talkTime = 2f;

    private RectTransform rectTransform;
    [Tooltip("true일 경우, 2초간 걸어간 뒤 스테이지를 이동합니다.")]
    public bool moveOnEnter;

    [Tooltip("false일 경우, Awake때 오브젝트를 비활성화합니다.")]
    public bool activeOnAwake;

    private WaitForSeconds waitSec;
    private void Awake()
    {
        canvasGroup.alpha = 0f;
        if (activeOnAwake)
        {
            parentObject.SetActive(true);
            rectTransform = image.rectTransform;
        }
        else
        {
            parentObject.SetActive(false);
        }

        waitSec = new WaitForSeconds(talkTime);
    }

    public void Active()
    {
        parentObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagName.Player))
        {
            col.enabled = false;

            if (moveOnEnter)
            {
                StartCoroutine(CoStageClear());
            }
            else
            {
                GameManager.instance.StageClear();
            }
        }
    }

    Vector2 leftPos = new Vector2(-2000f, 0f);
    Vector2 endPos = new Vector2(0f, 0f);
    private IEnumerator CoFadeClearImage()
    {
        rectTransform.anchoredPosition = leftPos;

        //canvasGroup.alpha = 0f;

        float progress = 0f;
        float timer = 0f;
        canvasGroup.alpha = 1f;
        while (progress < 1f)
        {
            timer += Time.deltaTime;
            progress = timer / 0.5f;

            rectTransform.anchoredPosition = Vector2.Lerp(leftPos, endPos, progress);
            //canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }
    }
    private IEnumerator CoStageClear()
    {
        ActiveMoveSystem(true);

        //UI 끄기
        UIPlayerHP ui = UIManager.Instance.GetUI("UIPlayerHP") as UIPlayerHP;
        ui.Close();

        //카메라 고정
        GameManager.instance.cameraManager.vcam.Follow = null;


        if (SceneChanger.Instance.GetNowSceneName() == SceneNames.stage_01)
        {
            AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(AudioManager.Instance.clipDict_SFX["Bear_ForwardRoar"]);
        }

        yield return new WaitForSeconds(0.8f);

        //대화 뜨기
        UIManager.Instance.Talk(DataManager.Instance.stageCode, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());

        UIManager.Instance.Talk(DataManager.Instance.stageCode + 1, 2f);
        yield return StartCoroutine(CoWaitTalkEnd());
        yield return new WaitForSeconds(0.6f);


        //오른쪽으로 이동
        StartCoroutine(CoFadeClearImage());
        GameManager.instance.playerController.InputVal.movementInput = 1f;
        yield return new WaitForSeconds(1.3f);
        GameManager.instance.StageClear();
    }

    private WaitForSeconds waitSsec = new WaitForSeconds(0.1f);
    private IEnumerator CoWaitTalkEnd()
    {
        float timer = 0f;

        while (timer < talkTime)
        {
            timer += Time.deltaTime;

            if (IsInputSkipKey())
            {
                UIManager.Instance.TalkEnd();
                yield return waitSsec;
                yield break;
            }
            yield return null;
        }
    }

    private bool IsInputSkipKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActiveMoveSystem(bool _b)
    {
        if (_b)
        {
            GameManager.instance.playerController.InputVal.movementInput = 0f;

            GameManager.instance.playerController.State.isCrouching = false;
            GameManager.instance.playerController.State.isLookUp = false;
            GameManager.instance.playerController.State.isAttack = false;

            GameManager.instance.playerController.State.moveSystem = true;
        }
        else
        {
            GameManager.instance.playerController.State.moveSystem = false;
        }
    }
}
