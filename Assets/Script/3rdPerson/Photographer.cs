using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    //��X����ת��������
    public float RotateX { get; private set; }
    //��Y����ת��������
    public float RotateY { get; private set; }
    //���������
    public float MouseSensitivity =5.0f;
    //��ת�ٶ�
    public float RotateSpeed = 80.0f;
    //�����λ��
    private Transform CarmeraTarget;
    private Transform _camera;
    //�������Y�����ƶ����ٶ�
    private float CameraYSpeed = 5.0f;
    [SerializeField]
    private AnimationCurve _armLengthCurve;

    private void Awake()
    {
        _camera = transform.GetChild(0);
    }
    void Start()
    {
        
    }

    //��ʼ���������λ��
    public void InitCamera(Transform target)
    {
        CarmeraTarget = target;
        transform.position = target.position;
    }
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        UpdatearmLength();
    }

    private void UpdateRotation()
    {
        RotateY += Input.GetAxis("Mouse X")*MouseSensitivity;
        RotateY += Input.GetAxis("Camera X") * RotateSpeed * Time.deltaTime;
        RotateX += Input.GetAxis("Mouse Y")*MouseSensitivity * (-1);
        RotateX += Input.GetAxis("Camera Y") * RotateSpeed * Time.deltaTime;
        RotateX = Mathf.Clamp(RotateX, -90, 90);


        transform.rotation=Quaternion.Euler(RotateX, RotateY, 0);
    }

    private void UpdatePosition()
    {
        Vector3 position = CarmeraTarget.position;
        float newY = Mathf.Lerp(transform.position.y, position.y, CameraYSpeed * Time.deltaTime);
        transform.position = new Vector3(position.x, newY, position.z);
    }


    private void UpdatearmLength()
    {
        _camera.localPosition = new Vector3(0, 0, _armLengthCurve.Evaluate(RotateX)*-1); 
    }
}
