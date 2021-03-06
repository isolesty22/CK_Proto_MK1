using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DarkJinnDustController : MonsterController
{
    #region
    [Serializable]
    public class DarkJinnDustStatus
    {
        public float blinkTime;
        [Header("Sub Status")]
        public bool isNonChaseP = false;

        public string shootDir = "NULL";
    }

    [Serializable]
    public class DarkJinnDustComponents
    {
        public ParticleSystem VFX_Bomb;

    }

    [SerializeField] private DarkJinnDustStatus darkJinnDustStatus = new DarkJinnDustStatus();
    [SerializeField] private DarkJinnDustComponents darkJinnDustComponents = new DarkJinnDustComponents();

    public DarkJinnDustStatus Stat2 => darkJinnDustStatus;
    public DarkJinnDustComponents Com2 => darkJinnDustComponents;

    private Vector3 shootDir;
    private bool isRunCo;
    #endregion
    public override void Initialize()
    {
        base.Initialize();
        isRunCo = false;
        Com.rigidbody.velocity = Vector3.zero;
        Com2.VFX_Bomb.gameObject.SetActive(false);
        Com2.VFX_Bomb.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(gameObject.transform.position, GameManager.instance.playerController.transform.position) > 30 && state == MonsterState.ATTACK)
        {
            ChangeState(MonsterState.DEATH);
        }
    }

    public override void State(MonsterState state)
    {
        base.State(state);
    }

    public override void ChangeState(MonsterState state)
    {
        base.ChangeState(state);
    }

    protected override void Idle()
    {
        base.Idle();
        ChangeState(MonsterState.MOVE);
    }

    protected override void Move()
    {
        base.Move();
    }
    protected override void Detect()
    {
        base.Detect();
    }
    protected override void Attack()
    {
        base.Attack();
        if (!isRunCo)
        {
            var shoot = Shoot();
            StartCoroutine(shoot);
        }

    }

    IEnumerator Shoot()
    {
        isRunCo = true;
        //blink Particle Play
        yield return new WaitForSeconds(Stat2.blinkTime);
        //Shoot Particle Play
        if (!Stat2.isNonChaseP)
        {
            shootDir = GameManager.instance.playerController.transform.position - gameObject.transform.position;
        }
        else
        {
            switch (Stat2.shootDir)
            {
                case "N":
                    shootDir = Vector3.up;
                    break;

                case "S":
                    shootDir = Vector3.down;
                    break;

                case "E":
                    shootDir = Vector3.right;
                    break;

                case "W":
                    shootDir = Vector3.left;
                    break;

                case "NW":
                    shootDir = new Vector3(-1, 1, 0);
                    break;

                case "NE":
                    shootDir = new Vector3(1, 1, 0);
                    break;

                case "SW":
                    shootDir = new Vector3(-1, -1, 0);
                    break;

                case "SE":
                    shootDir = new Vector3(1, -1, 0);
                    break;

                default:
                    shootDir = GameManager.instance.playerController.transform.position - gameObject.transform.position;
                    break;
            }

        }

        Debug.Log(shootDir);

        Com.audio.PlayOneShot(Com.audio.clip);

        while (state != MonsterState.DEATH)
        {
            Com.rigidbody.velocity = shootDir.normalized * Stat.moveSpeed * Time.fixedDeltaTime * 100;
            yield return new WaitForFixedUpdate();
        }

        isRunCo = false;
    }

    public override void Hit(int damage)
    {
        if (state == MonsterState.WAIT || state == MonsterState.DEATH)
            return;

        base.Hit(damage);
    }

    protected override void Death()
    {
        if (!Com2.VFX_Bomb.isPlaying)
        {
            Com.monsterModel.SetActive(false);
            Com2.VFX_Bomb.gameObject.SetActive(true);
            Com2.VFX_Bomb.Play();
        }
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ChangeState(MonsterState.DEATH);
        }
    }

    public void SetStatus(float waitTime = 1f, string Dir = "NULL", float speed = 1f)
    {
        Stat2.isNonChaseP = true;
        Stat2.shootDir = Dir;
        Stat.moveSpeed = speed;
        Stat2.blinkTime = waitTime;
    }
}