using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTip : MonoBehaviour
{
    public GameObject canvas;

    private void Awake()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            Invoke(nameof(CloseTip), 2.5f);

        }
    }

    void CloseTip()
    {
        canvas.SetActive(false);
    }


}
