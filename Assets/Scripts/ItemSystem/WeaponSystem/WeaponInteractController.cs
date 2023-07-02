using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponInteractController : MonoBehaviour
{
    [SerializeField] public UnityEvent<GameObject> Init;
    [SerializeField] public UnityEvent attack;
    [SerializeField] public UnityEvent aim;
    [SerializeField] public UnityEvent shoot;

    [SerializeField] public string holdAnimationName;
    [SerializeField] public string rigAnimationName;
}
