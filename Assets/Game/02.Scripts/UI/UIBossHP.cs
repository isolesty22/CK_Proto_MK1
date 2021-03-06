using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIBossHP : UIBase
{

    public BossController bossController;
    public Image hpImage;

    private float maxHP;
    private string sceneName;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        UIManager.Instance.AddDict(this);
    }
    public override void Init()
    {
        CheckOpen();
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == SceneNames.stage_02 || sceneName == SceneNames.stage_04)
        {
            gameObject.SetActive(true);
            bossController = FindObjectOfType<BossController>();
            maxHP = bossController.hp;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void StartUI()
    {
        StartCoroutine(ProcessOpen());
        StartCoroutine(UpdateUI());
    }

    float timer = 0f;
    float progress = 1f;


    public override bool Close()
    {
        if (isOpen)
        {
            StartCoroutine(ProcessClose());
            return true;
        }
        else
        {
            return false;
        }

    }
    protected override IEnumerator ProcessClose()
    {
        yield return StartCoroutine(base.ProcessClose());
        gameObject.SetActive(false);
    }

    //private const string str_Die = "Die";
    private IEnumerator UpdateUI()
    {

        while (bossController.hp > 0)
        {
            timer += Time.deltaTime * 0.01f;
            progress = Mathf.Lerp(progress, bossController.hp / maxHP, timer);
            hpImage.fillAmount = progress;
            yield return null;
        }

        hpImage.fillAmount = 0f;
        yield break;
        //yield return new WaitUntil(() => bossController.stateInfo.state == str_Die);
        //float startFillAmount = hpImage.fillAmount;

        //float oneSecTimer = 0f;

        //while (progress < 1f)
        //{
        //    oneSecTimer += Time.deltaTime;
        //    progress = oneSecTimer / 1f;
        //    hpImage.fillAmount = Mathf.Lerp(startFillAmount, 0f, oneSecTimer);

        //    yield return null;
        //}

    }

    //private void Update()
    //{
    //    timer += Time.deltaTime * 0.01f;
    //    progres = Mathf.Lerp(progres, bossController.hp / maxHP, timer);
    //    hpImage.fillAmount = progres;

    //    // hpImage.fillAmount = bossController.hp / maxHP;
    //}
}
