using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterMovement m_characterMovement;
    private BackpackageController m_backpackageController;
    public bool showMouse=false;
    private Animator m_animator;
    //摄像机脚本
    public Photographer m_cameraMovement;
    //摄像机跟随点
    [SerializeField]
    private Transform m_followingTarget;
    //互动范围
    private readonly float m_interactRadius = 1.0f;

    public GameObject m_RightHand;
    private bool isDead = false;

    private CharacterStats m_characterStatas;
    private float lastAttackTime;
    AudioSource walkStep;

    RaycastHit hitInfo;

    private void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_cameraMovement = GameManager.Instance.GetGameObject("Photographer").GetComponent<Photographer>();
        m_cameraMovement.InitCamera(m_followingTarget);
        m_animator = GetComponent<Animator>();
        m_characterStatas = GetComponent<CharacterStats>();
        m_backpackageController = GetComponent<BackpackageController>();
        walkStep = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        GameManager.Instance.RigisterPlayer(m_characterStatas);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InventoryManager.RefreshItem();
        InventoryManager.RefreshEquip();
        SaveManager.Instance.LoadPlayerData();
    }

    void Update()
    {
        isDead = m_characterStatas.CurrentHealth == 0;
        if (isDead)
        {
            m_animator.SetBool("IsDead", isDead);
            GameManager.Instance.NotifyObservers();
        }
        else
        {
            UpdateMovement();
            UpdateAttack();
            UpdateInteract();
            UpdateSkill();
            SelectAttackTarget();
            UpdateUseItem();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void UpdateMovement()
    {
        Quaternion rot = Quaternion.Euler(0, m_cameraMovement.RotateY, 0);
        
        m_characterMovement.SetMoveInput(rot*Vector3.forward*Input.GetAxis("Vertical") +
            rot*Vector3.right*Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.LeftShift));
    }
    void UpdateAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (lastAttackTime < 0)
            {
                m_animator.SetTrigger("IsAttacking");
                lastAttackTime = m_characterStatas.CoolDowm;
            }
        }
    }
    void UpdateInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var colliders = Physics.OverlapSphere(transform.position, m_interactRadius);
           
            foreach (var collider in colliders)
            {
                if(collider.CompareTag("NPC"))
                {
                    //TODO:打开物品
                    return ;
                }
            }
        }
    }
    void UpdateSkill()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            showMouse = !showMouse;
            if (showMouse || m_backpackageController.m_ShowMouse)
                Cursor.lockState = CursorLockMode.Confined;
        }
        if (showMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
            }
        }
    }
    void UpdateUseItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (InventoryManager.Instance.myBag.itemsList[0] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[0].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[0].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth+20,m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[0].itemHeld <= 0) 
            {
                InventoryManager.Instance.myBag.itemsList[0].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[0] = null;
            }
            InventoryManager.RefreshItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (InventoryManager.Instance.myBag.itemsList[1] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[1].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[1].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth + 20, m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[1].itemHeld <= 0)
            {
                InventoryManager.Instance.myBag.itemsList[1].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[1] = null;
            }
            InventoryManager.RefreshItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (InventoryManager.Instance.myBag.itemsList[2] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[2].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[2].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth + 20, m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[2].itemHeld <= 0)
            {
                InventoryManager.Instance.myBag.itemsList[2].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[2] = null;
            }
            InventoryManager.RefreshItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (InventoryManager.Instance.myBag.itemsList[3] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[3].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[3].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth + 20, m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[3].itemHeld <= 0)
            {
                InventoryManager.Instance.myBag.itemsList[3].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[3] = null;
            }
            InventoryManager.RefreshItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (InventoryManager.Instance.myBag.itemsList[4] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[4].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[4].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth + 20, m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[4].itemHeld <= 0)
            {
                InventoryManager.Instance.myBag.itemsList[4].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[4] = null;
            }
            InventoryManager.RefreshItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (InventoryManager.Instance.myBag.itemsList[5] == null)
                return;
            if (InventoryManager.Instance.myBag.itemsList[5].itemName != "HealthPotion")
                return;
            InventoryManager.Instance.myBag.itemsList[5].itemHeld -= 1;
            m_characterStatas.CurrentHealth = Mathf.Min(m_characterStatas.CurrentHealth + 20, m_characterStatas.MaxHealth);
            if (InventoryManager.Instance.myBag.itemsList[5].itemHeld <= 0)
            {
                InventoryManager.Instance.myBag.itemsList[5].itemHeld += 1;
                InventoryManager.Instance.myBag.itemsList[5] = null;
            }
            InventoryManager.RefreshItem();
        }
    }

    void HitBegin()
    {
        m_RightHand.SetActive(true);
    }
    void HitEnd()
    {
        m_RightHand.SetActive(false);
    }
    void PlayStepSource()
    {
        walkStep.Play();
    }


    void SelectAttackTarget()
    {
        if (Input.GetMouseButtonDown(1)&&hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                GameManager.Instance.GetAttackTarget(hitInfo.collider.gameObject);
        }
    }
}
