using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHealthBar : MonoBehaviour
{
    public Image mask;

    float originalSize;

    private void Awake()
    {
        mask = GameManager.Instance.GetGameObject("Mask").GetComponent<Image>();
        originalSize = mask.rectTransform.rect.width;
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float value =(float)GameManager.Instance.playerStats.CurrentHealth/ (float)GameManager.Instance.playerStats.MaxHealth;
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    } 

}
