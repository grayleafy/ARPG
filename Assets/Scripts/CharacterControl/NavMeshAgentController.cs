using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    new Rigidbody rigidbody;
    Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (navMeshAgent.isStopped == false)
        {

            //位置
            Vector3 velocity = navMeshAgent.desiredVelocity;
            rigidbody.velocity = new Vector3(velocity.x, rigidbody.velocity.y, velocity.z);
            //如果有走动，则取消动作,并关闭根运动
            if (velocity != Vector3.zero)
            {
                if (animator.GetBool("CanAction") == false)
                {
                    animator.CrossFade("NoAction", 0.25f);
                }
                //animator.ForceCrossFade("NoAction", 0.25f);
                animator.applyRootMotion = false;
            }

            //旋转
            if (velocity.magnitude > 0.01f && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                Quaternion nextRot = Quaternion.LookRotation(velocity, Vector3.up);
                Quaternion deltaRot = nextRot * Quaternion.Inverse(rigidbody.rotation);
                deltaRot.ToAngleAxis(out float deltaAngle, out Vector3 axis);
                if (axis.y < 0f)
                {
                    axis = -axis;
                    deltaAngle = -deltaAngle;
                }
                if (Mathf.Abs(deltaAngle) > 180)
                {
                    if (deltaAngle < 0f)
                    {
                        deltaAngle += 360;
                    }
                    else
                    {
                        deltaAngle -= 360;
                    }
                }
                float angularVelocityY = deltaAngle * Mathf.PI / 180 / Time.fixedDeltaTime;
                rigidbody.angularVelocity = new Vector3(0, Mathf.Lerp(rigidbody.angularVelocity.y, angularVelocityY, 0.2f), 0);
            }
            else if (navMeshAgent.remainingDistance > 0.01f && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Quaternion nextRot = Quaternion.LookRotation(navMeshAgent.destination - transform.position, Vector3.up);
                Quaternion deltaRot = nextRot * Quaternion.Inverse(rigidbody.rotation);
                deltaRot.ToAngleAxis(out float deltaAngle, out Vector3 axis);
                if (axis.y < 0f)
                {
                    axis = -axis;
                    deltaAngle = -deltaAngle;
                }
                if (Mathf.Abs(deltaAngle) > 180)
                {
                    if (deltaAngle < 0f)
                    {
                        deltaAngle += 180;
                    }
                    else
                    {
                        deltaAngle -= 180;
                    }
                }
                float angularVelocityY = deltaAngle * Mathf.PI / 180 / Time.fixedDeltaTime;
                rigidbody.angularVelocity = new Vector3(0, Mathf.Lerp(rigidbody.angularVelocity.y, angularVelocityY, 0.2f), 0);
            }

            navMeshAgent.nextPosition = rigidbody.position;
        }
    }

    public void StartNavagation()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
    }

    public void StopNavagation()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        navMeshAgent?.SetDestination(destination);
    }
}
