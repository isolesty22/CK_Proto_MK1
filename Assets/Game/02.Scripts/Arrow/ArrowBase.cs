using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBase : MonoBehaviour
{
    public int damage = 1;

    public bool isActive;

    private MonsterController currentMonster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            if (currentMonster = other.GetComponent<MonsterController>())
            {
                if (!currentMonster.Stat.isAlive)
                    return;
                else
                {
                    currentMonster.Hit(damage);
                    PlayHitAndRelease();
                }
            }
            else
            {
                PlayHitAndRelease();
            }

            return;
        }
        if (other.CompareTag("Boss"))
        {
            PlayHitAndRelease();
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(AudioManager.Instance.clips.arrowHit);
            var hit = CustomPoolManager.Instance.arrowHitPool.SpawnThis(transform.position, transform.eulerAngles, null);
            hit.Play();

            isActive = false;
            CustomPoolManager.Instance.ReleaseThis(this);
            return;
        }

        if (other.CompareTag("Summons"))
        {
            PlayHitAndRelease();
        }

    }

    private void PlayHitAndRelease()
    {
        AudioManager.Instance.Audios.audioSource_SFX.PlayOneShot(AudioManager.Instance.clips.arrowHit);
        var hit = CustomPoolManager.Instance.arrowHitPool.SpawnThis(transform.position, transform.eulerAngles, null);
        hit.Play();

        var player = GameManager.instance.playerController;
        player.Stat.pixyEnerge = Mathf.Clamp(player.Stat.pixyEnerge += player.Stat.attackEnerge, 0, 30);


        isActive = false;
        CustomPoolManager.Instance.ReleaseThis(this);
    }
}
