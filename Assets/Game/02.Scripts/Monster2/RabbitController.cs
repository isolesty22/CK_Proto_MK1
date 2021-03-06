using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RabbitController : MonsterController
{
    #region
    [Serializable]
    public class RabbitStatus
    {
        public float jumpPower;

        [Header("Sub Status")]
        public float moveDelay;
    }

    [Serializable]
    public class RabbitComponents
    {
        public CapsuleCollider capsuleCollider;
    }

    [SerializeField] private RabbitStatus rabbitStatus = new RabbitStatus();
    [SerializeField] private RabbitComponents rabbitComponents = new RabbitComponents();

    public RabbitStatus Stat2 => rabbitStatus;
    public RabbitComponents Com2 => rabbitComponents;

    private float moveTime;
    private Vector3 firstLookDir;
    private Vector3 layDir = new Vector3();
    #endregion
    public override void Initialize()
    {
        base.Initialize();
        Com.animator.SetBool("isDeath", false);
        Com.rigidbody.velocity = Vector3.zero;
        transform.localEulerAngles = firstLookDir;
        moveTime = 0.0f;
    }

    public override void Awake()
    {
        firstLookDir = transform.localEulerAngles;
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
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
        moveTime += Time.deltaTime;

        if (moveTime > Stat2.moveDelay)
        {

            if (transform.localEulerAngles == Vector3.zero)
            {
                layDir = Vector3.left;
            }
            else
            {
                layDir = Vector3.right;
            }

            if (Physics.Raycast(transform.position, layDir * 2, Com2.capsuleCollider.radius + 0.1f, LayerMask.GetMask("Ground")))
            {
                if (transform.localEulerAngles == Vector3.zero)
                {
                    Com.rigidbody.velocity = Vector3.zero;
                    transform.localEulerAngles = new Vector3(0, 180, 0);
                    Com.audio.PlayOneShot(Com.audio.clip);
                    Com.rigidbody.AddForce(new Vector3(Stat.moveSpeed, Stat2.jumpPower, 0), ForceMode.Impulse);
                    layDir = Vector3.right;
                }
                else
                {
                    Com.rigidbody.velocity = Vector3.zero;
                    transform.localEulerAngles = Vector3.zero;
                    Com.audio.PlayOneShot(Com.audio.clip);
                    Com.rigidbody.AddForce(new Vector3(-Stat.moveSpeed, Stat2.jumpPower, 0), ForceMode.Impulse);
                    layDir = Vector3.left;
                }
            }
            else
            {
                if (transform.localEulerAngles == Vector3.zero)
                {
                    Com.audio.PlayOneShot(Com.audio.clip);
                    Com.rigidbody.AddForce(new Vector3(-Stat.moveSpeed, Stat2.jumpPower, 0), ForceMode.Impulse);
                    layDir = Vector3.left;
                }
                else
                {
                    Com.audio.PlayOneShot(Com.audio.clip);
                    Com.rigidbody.AddForce(new Vector3(Stat.moveSpeed, Stat2.jumpPower, 0), ForceMode.Impulse);
                    layDir = Vector3.right;
                }
            }
            Com.animator.SetTrigger("isMove");
            moveTime = 0;
        }
    }
    protected override void Detect()
    {
        base.Detect();
    }
    protected override void Attack()
    {
        base.Attack();
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    protected override void Death()
    {
        Com.animator.SetBool("isDeath", true);
        base.Death();
    }

    protected override void HandleAnimation()
    {
        base.HandleAnimation();
    }
}