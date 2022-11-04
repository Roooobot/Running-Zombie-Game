using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    CharacterStats m_characterStatas;
    private void Awake()
    {
        this.gameObject.SetActive(false);
        m_characterStatas = GetComponentInParent<CharacterStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attackable"))
        {
            var targetStats = other.GetComponent<CharacterStats>();
            targetStats.TakeDamage(m_characterStatas, targetStats);
        }
    }
}
