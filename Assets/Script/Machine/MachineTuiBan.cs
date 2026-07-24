using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTuiBan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOMoveZ(1.9f, 2f))
            .Append(transform.DOMoveZ(2.38f, 2f)).SetLoops(-1);
    }
}
