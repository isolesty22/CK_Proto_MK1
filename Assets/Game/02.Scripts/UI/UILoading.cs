using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{

    public Image backgroundImage;
    public Image loadingBarImage;



    #region Joke
    public RectTransform ipiaTransform;

    public float startPosY;

    public void CalcStartPosY()
    {
        startPosY = 0f - ipiaTransform.rect.height;
    }
    #endregion

    private void Start()
    {
        Init();
        //RegisterUIManager();
    }

    public override void Init()
    {
        base.Init();
    }
    protected override void CheckOpen()
    {
        isOpen = Com.canvas.enabled;
    }

    public override bool Open()
    {
        isOpen = false;
        fadeDuration = 0.3f;
        StartCoroutine(ProcessOpen());
        return true;
    }

    public override bool Close()
    {
        Debug.Log("Close!");
        fadeDuration = 0.5f;
        StartCoroutine(ProcessClose());
        return true;
    }

    public IEnumerator OpenThis()
    {
        fadeDuration = 0.3f;
        yield return StartCoroutine(ProcessOpen());
        isOpen = true;
    }

    protected override IEnumerator ProcessClose()
    {
        yield return StartCoroutine(base.ProcessClose());
        SceneChanger.Instance.isLoading = false;
    }

}
