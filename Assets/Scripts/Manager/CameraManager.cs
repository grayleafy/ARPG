using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonAutoMono<CameraManager>
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 2f, 0);
    [SerializeField] float distance = 10f;
    [SerializeField] float yawAngle = 0;
    [SerializeField] float pitchAngle = 60;
    [SerializeField] bool lockPerspective = true;
    [SerializeField] bool smooth = true;

    [Header("速度设置")]
    [SerializeField] float followHalfLife = 0.2f;
    [SerializeField] float rotationSpeed = 0.1f;

    [Header("角度限制")]
    [SerializeField] float maxPitchAngle = 80f;
    [SerializeField] float minPitchAngle = 30f;


    new Camera camera;



    public void InitWhenLoadScene()
    {
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdateLockPerspective();
        if (lockPerspective == false) UpdateAngle();
        if (target != null)
        {
            UpdateRotation();
            UpdateCameraPos();
        }

    }


    public void SetTarget(GameObject target)
    {
        this.target = target.transform;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void UpdateLockPerspective()
    {
        lockPerspective = !InputMgr.GetInstance().MouseMiddleButton;
    }

    void UpdateAngle()
    {
        pitchAngle -= InputMgr.GetInstance().MouseDelta.y * rotationSpeed;
        pitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);
        yawAngle += InputMgr.GetInstance().MouseDelta.x * rotationSpeed;
        if (yawAngle < 0) yawAngle += 360;
        if (yawAngle > 360) yawAngle -= 360;
    }

    void UpdateRotation()
    {
        Quaternion targetCameraRot = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        Quaternion verticalRot = Quaternion.Euler(pitchAngle, 0, 0);
        Quaternion horizontalRot = Quaternion.Euler(0, yawAngle, 0);
        targetCameraRot = horizontalRot * verticalRot * targetCameraRot;
        camera.transform.rotation = targetCameraRot;
    }

    void UpdateCameraPos()
    {
        Vector3 targetCameraPos = target.position + offset - distance * camera.transform.forward;
        if (smooth)
        {
            camera.transform.position = SpringSystem.Spring.Damper(camera.transform.position, targetCameraPos, followHalfLife, Time.deltaTime);
        }
        else
        {
            camera.transform.position = targetCameraPos;
        }
    }


}
