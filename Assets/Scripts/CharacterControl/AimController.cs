using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] Transform horizontalRotationObject;
    [SerializeField] Transform verticalRotationObject;
    [SerializeField] Transform verticalRotationCenter;
    [SerializeField] Transform muzzle;
    [SerializeField] float disObjectToCenter = 0.2f;
    [SerializeField] Transform target;
    Vector3 AimPos;

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        VerticalUpdate(target.position);
        HorizontalUpdate(target.position);
    }

    void AimUpdate(Vector3 targetPos)
    {

    }

    void HorizontalUpdate(Vector3 aimPos)
    {
        Vector3 dir1 = horizontalRotationObject.transform.InverseTransformPoint(verticalRotationCenter.position);
        dir1.y = 0;
        Vector3 localPos = horizontalRotationObject.transform.InverseTransformPoint(aimPos);
        localPos.y = 0;
        if (localPos.magnitude < dir1.magnitude)
        {
            localPos = localPos.normalized * (dir1.magnitude + 0.1f);
        }
        Vector3 horizontalDir = new Vector3(dir1.x, 0, 0);
        float a = horizontalDir.magnitude;
        float c = localPos.magnitude;
        float h = Mathf.Sqrt(c * c - a * a);
        //float dis = h - dir1.z;

        Vector3 from = horizontalDir + h * Vector3.forward;
        Quaternion deltaRot = Quaternion.FromToRotation(from, localPos);

        if (horizontalRotationObject.GetComponent<Rigidbody>() == null)
        {
            horizontalRotationObject.rotation = deltaRot * horizontalRotationObject.rotation;
        }
        else
        {
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            if (axis.y < 0)
            {
                axis = -axis;
                angle = -angle;
            }
            //horizontalRotationObject.GetComponent<Rigidbody>().MoveRotation(deltaRot * horizontalRotationObject.GetComponent<Rigidbody>().rotation);
            //horizontalRotationObject.GetComponent<Rigidbody>().angularVelocity = deltaRot.eulerAngles * Mathf.PI / 180;
            horizontalRotationObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, angle * Mathf.PI / 180 / Time.fixedDeltaTime, 0);
        }
    }

    void VerticalUpdate(Vector3 aimPos)
    {
        //转换坐标，只保留垂直部分
        Vector3 localPos = verticalRotationCenter.transform.InverseTransformPoint(aimPos);
        Quaternion q = Quaternion.FromToRotation(new Vector3(localPos.x, 0, localPos.z), Vector3.forward);
        aimPos = verticalRotationCenter.transform.TransformPoint(q * localPos);

        Vector3 dir1 = (verticalRotationObject.position - verticalRotationCenter.position).normalized * disObjectToCenter;
        Vector3 dir2 = muzzle.position - verticalRotationObject.position;
        Quaternion q1 = Quaternion.LookRotation(aimPos - verticalRotationCenter.position, Vector3.up);
        Quaternion deltaRot = q1 * Quaternion.Inverse(muzzle.rotation);
        Vector3 pos1 = verticalRotationCenter.position + (aimPos - verticalRotationCenter.position).normalized * (dir1 + dir2).magnitude;
        Vector3 deltaPos = pos1 - muzzle.position;

        verticalRotationObject.position = verticalRotationObject.position + deltaPos;
        verticalRotationObject.rotation = deltaRot * verticalRotationObject.rotation;
    }
}
