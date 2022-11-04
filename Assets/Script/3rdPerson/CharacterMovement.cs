using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    //����
    Rigidbody m_Rigidbody;
    //�ƶ�����
    Vector3 CurrentInput;
    //�����������
    Animator m_Animator;
    //��ʾ3D��Ϸ�е���ת����ʼ��Ϊ����ת
    Quaternion m_Rotation = Quaternion.identity;
    //��ת�ٶ�
    public float turnSpeed = 20f;

    private bool hasHorizontal;
    private bool hasVertical;
    private bool isWallking = false;
    private bool isWandering = false;

    private void Awake()
    {
        //��ȡ�������
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        hasHorizontal=!Mathf.Approximately(CurrentInput.x,0.0f);
        hasVertical=!Mathf.Approximately(CurrentInput.y,0.0f);
        isWallking = hasHorizontal || hasVertical;
        m_Animator.SetBool("IsWalking", isWallking);
        m_Animator.SetBool("IsWandering", isWandering);
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, CurrentInput, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }
    
    public void SetMoveInput(Vector3 input,bool iswandering)
    {
        //�������޶���0~1֮��
        CurrentInput = Vector3.ClampMagnitude(input, 1);
        isWandering=iswandering;
    }

    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position +  m_Animator.deltaPosition.magnitude * CurrentInput);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}
