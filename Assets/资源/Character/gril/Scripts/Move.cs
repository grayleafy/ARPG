using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent), typeof(Animator))]
public class Move : MonoBehaviour
{
    [SerializeField, HideInInspector]
    UnityEngine.AI.NavMeshAgent agent;
    [SerializeField, HideInInspector]
    Animator animator;

    void Reset()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }
}