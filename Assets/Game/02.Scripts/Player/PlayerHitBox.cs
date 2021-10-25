using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    public PlayerController playerController;
    public CapsuleCollider hitBox;
    public CapsuleCollider crouchHitBox;

    public ParticleSystem spawn;

    public IEnumerator parry;

    private void Start()
    {
        parry = playerController.Parrying();
        spawn.gameObject.SetActive(false);
    }

    IEnumerator Fall(Vector3 spawnPos)
    {
        playerController.Hit();

        spawn.gameObject.SetActive(true);
        spawn.transform.position = spawnPos + Vector3.down * .95f;
        spawn.Play();

        yield return new WaitForSeconds(playerController.Stat.spawnTime);
        playerController.transform.position = spawnPos;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall"))
        {
            var fall = Fall(other.GetComponent<FallController>().spawnPos.position);
            StartCoroutine(fall);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (other.GetComponent<MonsterController>())
            {
                if (!other.GetComponent<MonsterController>().Stat.isAlive)
                    return;
            }
        }

        if (playerController.State.canParry)
        {
            if(other.CompareTag("Monster"))
            {
                StopCoroutine(parry);

                parry = playerController.Parrying();
                StartCoroutine(parry);

                if (other.GetComponent<ArmadiloController>())
                {
                    other.GetComponent<ArmadiloController>().ChangeToOverturn();
                }

                return;
            }

            else if (other.CompareTag("ParryingObject"))
            {
                StopCoroutine(parry);

                parry = playerController.Parrying();
                StartCoroutine(parry);
                return;
            }

            else if (other.CompareTag("EnemyBullet"))
            {
                StopCoroutine(parry);

                parry = playerController.Parrying();
                StartCoroutine(parry);
                return;
            }
        }

        //�ǰ� ����
        if (!playerController.State.isInvincible )
        {
            if (other.CompareTag("Monster"))
            {
                playerController.Hit();
            }

            else if (other.CompareTag("ParryingObject"))
            {
                if (other.GetComponent<ParryingObject>().Stat.isDamaged)
                {
                    playerController.Hit();
                }
            }

            else if (other.CompareTag("EnemyBullet"))
            {
                playerController.Hit();
            }

        }

    }
}
