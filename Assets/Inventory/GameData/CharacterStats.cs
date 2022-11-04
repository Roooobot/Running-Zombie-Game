using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    private CharacterStats player;

    private void Awake()
    {
        if(templateData != null)
            characterData = Instantiate(templateData);
    }

    private void Start()
    {
        
        player =GameManager.Instance.GetGameObject("Zombie(Clone)").GetComponent<CharacterStats>();
    }

    #region Read from CharacterData_SO
    public int MaxHealth { get { return characterData.maxHealth; } set { characterData.maxHealth = value; } }
    public int CurrentHealth { get { return characterData.currentHealth; } set { characterData.currentHealth = value; } }
    public int Gold { get { return characterData.gold; } set { characterData.gold = value; } }
    #endregion
    #region Read from AttackData_SO
    public float AttackRange{ get { return attackData.attackRange; }set { attackData.attackRange = value; } }
    public int Damge { get { return attackData.damge; } set { attackData.damge = value; } }
    public float CoolDowm { get { return attackData.coolDowm; } set { attackData.coolDowm = value; } }
    public int CurrentDefence { get { return attackData.currentDefence; } set { attackData.currentDefence = value; } }
    #endregion

    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage;
        int equipDamage = 0;
        if(attacker == player)
        {
            for(int i=0;i<InventoryManager.Instance.myEquip.itemsList.Count;i++)
            {
                if (InventoryManager.Instance.myEquip.itemsList[i]!=null)
                equipDamage += InventoryManager.Instance.myEquip.itemsList[i].damage;
            }
        }
        damage = Mathf.Max(1, equipDamage+attacker.Damge - defener.CurrentDefence);
        CurrentHealth = Mathf.Max(CurrentHealth - damage,0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
    }
    
    public void Damage(int da)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - da, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
    }

}
