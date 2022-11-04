using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photographer : MonoBehaviour
{
    //绕X轴旋转的输入量
    public float RotateX { get; private set; }
    //绕Y轴旋转的输入量
    public float RotateY { get; private set; }
    //鼠标灵敏度
    public float MouseSensitivity =5.0f;
    //旋转速度
    public float RotateSpeed = 80.0f;
    //摄像机位置
    private Transform CarmeraTarget;
    private Transform _camera;
    //摄像机在Y轴上移动的速度
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

    //初始化摄像机的位置
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
