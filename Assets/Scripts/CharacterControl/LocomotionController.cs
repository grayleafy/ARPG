using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class LocomotionController : MonoBehaviour
{
    [Header("动画参数")]
    [SerializeField] float vertical = 0;
    [SerializeField] float horizontal = 0;
    [SerializeField] float turn = 0;

    [Header("转换后的输入方向")]
    [SerializeField] Vector3 playerDir = Vector3.zero;

    [Header("基础设置")]
    [SerializeField] float timeScale = 1;
    [SerializeField] public bool applyLocomotion = false;
    [SerializeField] ArmState armState = ArmState.Normal;
    [SerializeField] PlayerPosture postureState = PlayerPosture.Stand;
    [SerializeField] LocomotionState locomotionState = LocomotionState.Run;
    [SerializeField] Transform aimPoint; //瞄准目标
    [SerializeField] Transform lockPoint; //攻击时的锁定目标

    [Header("速度设置")]
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float aimRunSpeed = 4f;
    [SerializeField] float turnSpeed = 0.2f;
    [SerializeField] float crouchWalkSpeed = 0.7f;

    [Header("地面检测")]
    [SerializeField] bool isKeepStickGround = true;
    [SerializeField] Transform leftFootBottom;
    [SerializeField] Transform rightFootBottom;
    Vector3[] castPoint = new Vector3[5]{
        new Vector3(-0.18f, 0.5f, 0),   //左脚，会自动计算
        new Vector3(0.18f, 0.5f, 0),    //右键
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
    };
    [SerializeField] Vector3 castOriginOffset = new Vector3(0, 0.7f, 0);
    float[] castDistance = new float[5];
    [SerializeField] float toleranceDistance = 0.5f;
    RaycastHit[] groundHits = new RaycastHit[5];
    bool[] isHits = new bool[5];
    [SerializeField] public bool onGround = true;
    int groundLaymask;

    [Header("消除IK不连续性")]
    [SerializeField] bool isFootIK = true;
    [SerializeField] bool useFade = true;
    [SerializeField] float IKDisThreshold = 0.2f;
    [SerializeField] float IKFollowSpeed = 0.2f;
    [SerializeField] float maxDis = 1f;   //例如人物传送时忽略
    float lastLeftFootWeight = 0;
    float lastLeftFootRotWeight = 0;
    float leftFootWeightOffset = 0;
    float leftFootRotWeightOffset = 0;
    Vector3 lastLeftFootIKPos = Vector3.zero;
    Vector3 leftFootIKPosOffset = Vector3.zero;
    Quaternion lastLeftFootIKRot = Quaternion.identity;
    Quaternion leftFootIKRotOffset = Quaternion.identity;
    float lastRightFootWeight = 0;
    float lastRightFootRotWeight = 0;
    float rightFootWeightOffset = 0;
    float rightFootRotWeightOffset = 0;
    Vector3 lastRightFootIKPos = Vector3.zero;
    Vector3 rightFootIKPosOffset = Vector3.zero;
    Quaternion lastRightFootIKRot = Quaternion.identity;
    Quaternion rightFootIKRotOffset = Quaternion.identity;



    ////刚体跟踪地面的垂直速度，用于弹簧系统
    //[SerializeField] float trackVelocity = 0;
    ////跟踪高度
    //[SerializeField] float trackHeight = 0;
    ////跟踪灵敏度
    //[SerializeField] float trackXGain = 0.1f;


    [Header("权重曲线")]
    [SerializeField] AnimationCurve footIKPosCurve;
    [SerializeField] AnimationCurve footIKRotCurve;

    Animator animator;
    NPCController npcController;
    new Rigidbody rigidbody;

    int horizontalID;
    int verticalID;
    int turnID;
    int crouchID;
    int aimID;

    public enum PlayerPosture
    {
        Crouch = 0,
        Stand = 1,
        MidAir = 2
    }
    public enum ArmState
    {
        Normal, Aim
    }

    public enum LocomotionState
    {
        Walk,
        Run
    }

    public bool death = false;



    private void Awake()
    {
        npcController = GetComponent<NPCController>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        animator.SetFloat("ScaleFactor", 1 / animator.humanScale);
        animator.applyRootMotion = false;

        horizontalID = Animator.StringToHash("Horizontal");
        verticalID = Animator.StringToHash("Vertical");
        turnID = Animator.StringToHash("Turn");
        crouchID = Animator.StringToHash("Crouch");
        aimID = Animator.StringToHash("Aim");

        //地面检测图层
        groundLaymask = LayerMask.GetMask(new string[] { "Character" });
        groundLaymask = ~groundLaymask;

        Time.timeScale = timeScale;
    }

    private void Update()
    {
        if (death == false) UpdateAnimator();
    }

    private void FixedUpdate()
    {
        //PlayerControlLocomotionProcess();
        if (death == false) FixedUpdateRigidbody();
        if (isKeepStickGround)
        {
            GroundCast(2, 4, true, out Vector3 hitPoint);
            StickGround(hitPoint);
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (onGround && isFootIK)
        {
            GroundCast(0, 2, false, out Vector3 hitPoint);

            //左脚
            if (isHits[0])
            {
                Quaternion rot = Quaternion.FromToRotation(leftFootBottom.up, groundHits[0].normal);
                Quaternion targetRot = rot * leftFootBottom.rotation;

                Plane plane = new Plane(groundHits[0].normal, groundHits[0].point);
                Vector3 foot = leftFootBottom.position;
                Vector3 footTarget = plane.ClosestPointOnPlane(foot);
                //Debug.DrawLine(footTarget, footTarget + Vector3.up);
                Vector3 dir = rot * leftFootBottom.parent.TransformDirection(leftFootBottom.localPosition);
                Vector3 IKTarget = footTarget - dir;

                float dis = (foot - footTarget).magnitude;
                if (Vector3.Dot(groundHits[0].normal, foot - footTarget) <= 0) dis = 0;
                float weight = footIKPosCurve.Evaluate(dis);
                float rotWeight = footIKRotCurve.Evaluate(dis);


                if (Mathf.Abs((IKTarget - lastLeftFootIKPos).y) > IKDisThreshold && (IKTarget - lastLeftFootIKPos).magnitude <= maxDis && useFade)
                {
                    leftFootWeightOffset = lastLeftFootWeight + leftFootWeightOffset - weight;
                    leftFootRotWeightOffset = lastLeftFootRotWeight + leftFootRotWeightOffset - rotWeight;
                    leftFootIKPosOffset = lastLeftFootIKPos + leftFootIKPosOffset - IKTarget;
                    leftFootIKRotOffset = leftFootIKRotOffset * lastLeftFootIKRot * Quaternion.Inverse(targetRot);
                }

                //leftFootWeightOffset = Mathf.Lerp(leftFootWeightOffset, 0, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);
                //leftFootIKPosOffset = Vector3.Lerp(leftFootIKPosOffset, Vector3.zero, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);
                //leftFootIKRotOffset = Quaternion.Lerp(leftFootIKRotOffset, Quaternion.identity, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);
                leftFootWeightOffset = SpringSystem.Spring.Damper(leftFootWeightOffset, 0, IKFollowSpeed, Time.deltaTime);
                leftFootRotWeightOffset = SpringSystem.Spring.Damper(leftFootRotWeightOffset, 0, IKFollowSpeed, Time.deltaTime);
                leftFootIKPosOffset = SpringSystem.Spring.Damper(leftFootIKPosOffset, Vector3.zero, IKFollowSpeed, Time.deltaTime);
                leftFootIKRotOffset = SpringSystem.Spring.Damper(leftFootIKRotOffset, Quaternion.identity, IKFollowSpeed, Time.deltaTime);

                lastLeftFootWeight = weight;
                lastLeftFootRotWeight = rotWeight;
                lastLeftFootIKPos = IKTarget;
                lastLeftFootIKRot = targetRot;


                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight + leftFootWeightOffset);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, IKTarget + leftFootIKPosOffset);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, rotWeight + leftFootRotWeightOffset);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootIKRotOffset * targetRot);
            }

            //右脚
            if (isHits[1])
            {
                Quaternion rot = Quaternion.FromToRotation(rightFootBottom.up, groundHits[0].normal);
                Quaternion targetRot = rot * rightFootBottom.rotation;

                Plane plane = new Plane(groundHits[1].normal, groundHits[1].point);
                Vector3 foot = rightFootBottom.position;
                Vector3 footTarget = plane.ClosestPointOnPlane(foot);
                //Debug.DrawLine(footTarget, footTarget + Vector3.up);
                Vector3 dir = rot * rightFootBottom.parent.TransformDirection(rightFootBottom.localPosition);
                Vector3 IKTarget = footTarget - dir;

                float dis = (foot - footTarget).magnitude;
                if (Vector3.Dot(groundHits[1].normal, foot - footTarget) <= 0) dis = 0;
                float weight = footIKPosCurve.Evaluate(dis);
                float rotWeight = footIKRotCurve.Evaluate(dis);

                if (Mathf.Abs((IKTarget - lastRightFootIKPos).y) > IKDisThreshold && (IKTarget - lastRightFootIKPos).magnitude <= maxDis && useFade)
                {
                    rightFootWeightOffset = lastRightFootWeight + rightFootWeightOffset - weight;
                    rightFootRotWeightOffset = lastRightFootRotWeight + rightFootRotWeightOffset - rotWeight;
                    rightFootIKPosOffset = lastRightFootIKPos + rightFootIKPosOffset - IKTarget;
                    rightFootIKRotOffset = rightFootIKRotOffset * lastRightFootIKRot * Quaternion.Inverse(targetRot);
                }

                //rightFootWeightOffset = Mathf.Lerp(rightFootWeightOffset, 0, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);
                //rightFootIKPosOffset = Vector3.Lerp(rightFootIKPosOffset, Vector3.zero, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);
                //rightFootIKRotOffset = Quaternion.Lerp(rightFootIKRotOffset, Quaternion.identity, IKFollowSpeed * Time.deltaTime / Time.fixedDeltaTime);

                rightFootWeightOffset = SpringSystem.Spring.Damper(rightFootWeightOffset, 0, IKFollowSpeed, Time.deltaTime);
                rightFootRotWeightOffset = SpringSystem.Spring.Damper(rightFootRotWeightOffset, 0, IKFollowSpeed, Time.deltaTime);
                rightFootIKPosOffset = SpringSystem.Spring.Damper(rightFootIKPosOffset, Vector3.zero, IKFollowSpeed, Time.deltaTime);
                rightFootIKRotOffset = SpringSystem.Spring.Damper(rightFootIKRotOffset, Quaternion.identity, IKFollowSpeed, Time.deltaTime);

                lastRightFootWeight = weight;
                lastLeftFootRotWeight = rotWeight;
                lastRightFootIKPos = IKTarget;
                lastRightFootIKRot = targetRot;

                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight + rightFootWeightOffset);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, IKTarget + rightFootIKPosOffset);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rotWeight + rightFootRotWeightOffset);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootIKRotOffset * targetRot);
            }
        }
    }

    #region 外部设置状态
    public void StartListenInput()
    {
        applyLocomotion = true;
        InputMgr.GetInstance().AddListener<Vector2>(InputEvent.WSADUpdate, DoMove);
        InputMgr.GetInstance().AddListener<bool>(InputEvent.CrouchChange, DoCrouch);
    }
    public void StopListemInput()
    {
        applyLocomotion = false;
        InputMgr.GetInstance().RemoveListener<Vector2>(InputEvent.WSADUpdate, DoMove);
        InputMgr.GetInstance().RemoveListener<bool>(InputEvent.CrouchChange, DoCrouch);
        UpdatePlayerDir(Vector2.zero);
    }

    /// <summary>
    /// 根据输入更新上下左右
    /// </summary>
    /// <param name="input"></param>
    public void InputWSADUpdate(Vector2 input)
    {
        DoMove(input);
    }
    public void SetIsStickGround(bool isStickGround)
    {
        this.isKeepStickGround = isStickGround;
    }

    public void SetIsFootIK(bool isFootIK)
    {
        this.isFootIK = isFootIK;
    }

    public void SetAimPoint(Transform target)
    {
        aimPoint = target;
    }

    /// <summary>
    /// 攻击前重新对准目标
    /// </summary>
    public void SetLockTarget(Transform target)
    {
        lockPoint = target;
    }

    /// <summary>
    /// 取消锁定目标
    /// </summary>
    public void CancelLock()
    {
        lockPoint = null;
    }

    #endregion

    #region Actions
    public void DoCrouch(bool crouch)
    {
        if (crouch)
        {
            postureState = PlayerPosture.Crouch;
        }
        else
        {
            postureState = PlayerPosture.Stand;
        }
    }

    public void DoMove(Vector2 input)
    {
        if (npcController.isInStagger)
        {
            UpdatePlayerDir(Vector2.zero);
        }
        else
        {
            UpdatePlayerDir(input);
        }
    }

    public void DoRun(bool run)
    {
        if (run == false)
        {
            locomotionState = LocomotionState.Walk;
        }
        else if (run == true)
        {
            locomotionState = LocomotionState.Run;
        }
    }

    public void DoAim(bool aim)
    {
        if (aim == false)
        {
            armState = ArmState.Normal;
        }
        else if (aim == true)
        {
            armState = ArmState.Aim;
        }
    }

    public void DoIdle()
    {
        DoMove(Vector2.zero);
    }

    //瞬间面向
    public void DoFaceInstantly(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        Quaternion Rot = Quaternion.LookRotation(dir, Vector3.up);
        rigidbody.MoveRotation(Rot);
    }

    #endregion

    void UpdatePlayerDir(Vector2 input)
    {

        playerDir = new Vector3(input.x, 0, input.y);
        playerDir = Camera.main.transform.TransformDirection(playerDir);
        playerDir = Vector3.ProjectOnPlane(playerDir, Vector3.up);
        playerDir.Normalize();
        playerDir = transform.InverseTransformDirection(playerDir);

        if (input == Vector2.zero)
        {
            playerDir = Vector3.zero;
        }


        //处理速度
        switch (locomotionState)
        {
            case LocomotionState.Walk:
                playerDir *= walkSpeed;
                break;
            case LocomotionState.Run:
                if (armState == ArmState.Normal)
                {
                    playerDir *= runSpeed;
                }
                else if (armState == ArmState.Aim)
                {
                    playerDir *= aimRunSpeed;
                }
                break;
        }
        switch (postureState)
        {
            case PlayerPosture.Stand:
                break;
            case PlayerPosture.Crouch:
                playerDir = playerDir.normalized * crouchWalkSpeed;
                break;
        }
    }



    //设置动画
    void UpdateAnimator()
    {
        //根据刚体处理水平垂直方向和转向
        vertical = Vector3.Dot(rigidbody.velocity, transform.forward);
        horizontal = Vector3.Dot(rigidbody.velocity, transform.right);
        float rad = rigidbody.angularVelocity.y;
        //if (playerDir == Vector3.zero) rad = 0;
        turn = rad * turnSpeed;

        //var currentInfo = animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Action"));
        //var nextInfo = animator.GetAnimatorTransitionInfo(animator.GetLayerIndex("Action"));
        //Debug.Log("current: " + currentInfo.fullPathHash);
        ////Debug.Log("next: " + nextInfo.fullPathHash);

        //如果有走动，则取消动作,并关闭根运动
        if (playerDir != Vector3.zero)
        {
            if (animator.GetBool("CanAction") == false)
            {
                animator.CrossFade("NoAction", 0.25f);
            }
            //animator.ForceCrossFade("NoAction", 0.25f);
            animator.applyRootMotion = false;
        }

        animator.SetFloat(horizontalID, horizontal, 0.2f, Time.deltaTime);
        animator.SetFloat(verticalID, vertical, 0.2f, Time.deltaTime);
        animator.SetFloat(turnID, turn, 0.2f, Time.deltaTime);
        switch (postureState)
        {
            case PlayerPosture.Stand:
                animator.SetBool(crouchID, false);
                break;
            case PlayerPosture.Crouch:
                animator.SetBool(crouchID, true);
                break;
        }
        switch (armState)
        {
            case ArmState.Normal:
                animator.SetBool(aimID, false);
                break;
            case ArmState.Aim:
                animator.SetBool(aimID, true);
                break;
        }
    }



    //更新物理运动
    void FixedUpdateRigidbody()
    {
        Vector3 dir = transform.TransformDirection(playerDir);

        //如果在自由模式下则根据输入转向
        if (armState == ArmState.Normal)
        {
            if (lockPoint == null)
            {
                if (playerDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                    targetRot = (Quaternion.Slerp(rigidbody.rotation, targetRot, 0.2f));
                    Quaternion deltaRot = targetRot * Quaternion.Inverse(rigidbody.rotation);
                    deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
                    if (axis.y < 0)
                    {
                        axis = -axis;
                        angle = -angle;
                    }
                    //rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, Mathf.Lerp(rigidbody.angularVelocity.y, angle * Mathf.PI / 180 / Time.fixedDeltaTime, 0.2f), rigidbody.angularVelocity.z);
                    rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, angle * Mathf.PI / 180 / Time.fixedDeltaTime, rigidbody.angularVelocity.z);  //不会转弯过头，但是不平滑
                }
                else
                {
                    //rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, Mathf.Lerp(rigidbody.angularVelocity.y, 0, 0.2f), rigidbody.angularVelocity.z);
                    rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, 0, rigidbody.angularVelocity.z); // 不会转弯过头，但是不平滑
                }
            }
            else  //有正在锁定的目标
            {
                Vector3 tempDir = lockPoint.position - transform.position;
                tempDir.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(tempDir, Vector3.up);
                targetRot = (Quaternion.Slerp(rigidbody.rotation, targetRot, 0.2f));
                Quaternion deltaRot = targetRot * Quaternion.Inverse(rigidbody.rotation);
                deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
                if (axis.y < 0)
                {
                    axis = -axis;
                    angle = -angle;
                }
                //rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, Mathf.Lerp(rigidbody.angularVelocity.y, angle * Mathf.PI / 180 / Time.fixedDeltaTime, 0.2f), rigidbody.angularVelocity.z);
                rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, angle * Mathf.PI / 180 / Time.fixedDeltaTime, rigidbody.angularVelocity.z);  //不会转弯过头，但是不平滑
            }
        }
        else if (armState == ArmState.Aim)
        {
            Vector3 tempDir = aimPoint.position - transform.position;
            tempDir.y = 0;
            Quaternion targetRot = Quaternion.LookRotation(tempDir, Vector3.up);
            targetRot = (Quaternion.Slerp(rigidbody.rotation, targetRot, 0.2f));
            Quaternion deltaRot = targetRot * Quaternion.Inverse(rigidbody.rotation);
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
            if (axis.y < 0)
            {
                axis = -axis;
                angle = -angle;
            }
            //rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, Mathf.Lerp(rigidbody.angularVelocity.y, angle * Mathf.PI / 180 / Time.fixedDeltaTime, 0.2f), rigidbody.angularVelocity.z);
            rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, angle * Mathf.PI / 180 / Time.fixedDeltaTime, rigidbody.angularVelocity.z);  //不会转弯过头，但是不平滑
            //if (Mathf.Abs(angle) < 1)
            //{
            //    rigidbody.angularVelocity = new Vector3(rigidbody.angularVelocity.x, 0, rigidbody.angularVelocity.z);
            //}
        }

        Vector3 targetVelocity = Vector3.Lerp(rigidbody.velocity, dir, 0.2f);
        rigidbody.velocity = new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.z);
    }

    //计算地面检测的投射起点
    void CalculateCastPoint()
    {
        castPoint[0] = leftFootBottom.position;
        castPoint[1] = rightFootBottom.position;
        //castPoint[2] = transform.TransformPoint(new Vector3(-0.18f, 0, 0));
        //castPoint[3] = transform.TransformPoint(new Vector3(0.18f, 0, 0)); //旋转时不稳定
        castPoint[2] = transform.TransformPoint(new Vector3(-0, 0, 0));
        castPoint[3] = transform.TransformPoint(new Vector3(0, 0, 0));
        for (int i = 0; i < castPoint.Length; i++)
        {
            castPoint[i] += castOriginOffset;
            castDistance[i] = castPoint[i].y - transform.position.y + toleranceDistance;
        }
    }

    void CalculateCastPoint(float downDistance)
    {
        castPoint[0] = leftFootBottom.position;
        castPoint[1] = rightFootBottom.position;
        //castPoint[2] = transform.TransformPoint(new Vector3(-0.18f, 0, 0));
        //castPoint[3] = transform.TransformPoint(new Vector3(0.18f, 0, 0)); //旋转时不稳定
        castPoint[2] = transform.TransformPoint(new Vector3(-0, 0, 0));
        castPoint[3] = transform.TransformPoint(new Vector3(0, 0, 0));
        for (int i = 0; i < castPoint.Length; i++)
        {
            castPoint[i] += castOriginOffset;
            castDistance[i] = castPoint[i].y - transform.position.y + downDistance;
        }
    }

    /// <summary>
    /// 判断是否在地面并保留击中信息, castpoint0-1是双脚, 2是原点坐标系下的左键， 3是原点坐标系下的右脚
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    public bool GroundCast(int castPointFrom, int castPointTo, bool updateOnGround, out Vector3 hitPoint, float downDistance = Mathf.Infinity - 9)
    {
        if (downDistance == Mathf.Infinity - 9)
        {
            CalculateCastPoint();
        }
        else
        {
            CalculateCastPoint(downDistance);
        }

        bool tempOnGround = true;
        hitPoint = new Vector3(0, Mathf.Infinity, 0);
        for (int i = castPointFrom; i < castPointTo; i++)
        {
            if (Physics.Raycast(castPoint[i], Vector3.down, out groundHits[i], castDistance[i], groundLaymask))
            {
                isHits[i] = true;
                if (groundHits[i].point.y < hitPoint.y)
                {
                    hitPoint = groundHits[i].point;
                }
            }
            else
            {
                isHits[i] = false;
                tempOnGround = false;
            }
        }

        if (updateOnGround) onGround = tempOnGround;
        return tempOnGround;
    }

    /// <summary>
    /// 修正高度, 以最低点为准，并且和footIK一致
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    public void StickGround(Vector3 hitPoint)
    {
        //SpringSystem.Spring.TrackingSpringUpdateNoVelocityAcceleration(ref trackHeight, ref trackVelocity, hitPoint.y, trackXGain, Time.fixedDeltaTime);
        float targetY = Mathf.Lerp(rigidbody.position.y, hitPoint.y, 0.2f);
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, (targetY - rigidbody.position.y) / Time.fixedDeltaTime, rigidbody.velocity.z);
    }









    #region abandoned
    ////private void OnAnimatorMove()
    ////{
    ////    MoveByRootMotion();
    ////}

    ////从动画取得根运动结果并更新刚体
    //void MoveByRootMotion()
    //{

    //    rigidbody.velocity = animator.velocity;
    //    rigidbody.angularVelocity = animator.angularVelocity;
    //}


    ///// <summary>
    ///// 根据输入更新移动，包括动画和刚体
    ///// </summary>
    //public void PlayerControlLocomotionProcess()
    //{
    //    DoCrouch();
    //    UpdatePlayerDir(input);
    //    Move();
    //}


    //void UpdateAnimator()
    //{
    //    //UpdateHorizontalAndVertical();
    //    SetAnimator();
    //}

    ////根据刚体速度更新参数信息
    //void UpdateHorizontalAndVertical()
    //{
    //    //根据刚体处理水平垂直方向和转向
    //    vertical = Vector3.Dot(rigidbody.velocity, transform.forward);
    //    horizontal = Vector3.Dot(rigidbody.velocity, transform.right);
    //    float rad = rigidbody.angularVelocity.y;
    //    //if (playerDir == Vector3.zero) rad = 0;
    //    turn = rad * turnSpeed;
    //}
    #endregion
}
