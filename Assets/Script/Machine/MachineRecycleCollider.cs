using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RecycleColliderType
{
    Get,
    Clear
}
public class MachineRecycleCollider : MonoBehaviour
{
    public RecycleColliderType type;
    public Transform clearEffect;
    public AudioSource audioSource;
    private void Start()
    {
        if (clearEffect != null)
        {
            clearEffect.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeSelf || !other.CompareTag("MachineItem"))
        {
            return;
        }

        MachineItemInfo machineItemInfo = other.gameObject.GetComponent<MachineItemInfo>();
        if (type == RecycleColliderType.Get)
        {
            machineItemInfo.GetMachineItemReward();
        }
        else
        {
            if (clearEffect != null)
            {
                if (!clearEffect.gameObject.activeSelf)
                {
                    clearEffect.transform.DOKill();
                    clearEffect.gameObject.SetActive(true);
                    AudioManager.Instance.SetAudioSource(audioSource, "DropItemEffect");
                    DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
                    {
                        clearEffect.gameObject.SetActive(false);
                    }
                    ).SetTarget(clearEffect.transform);
                }
            }
        }
        //机器回收
        if (GameManager.Instance.curMachine.machineItems.Contains(machineItemInfo))
        {
            GameManager.Instance.curMachine.machineItems.Remove(machineItemInfo);
        }
        //对象池回收
        ObjectPoolManager.Instance.RecycleObject(machineItemInfo.gameObject, machineItemInfo.machineItemType);

    }

}
