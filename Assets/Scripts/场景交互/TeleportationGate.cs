using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 传送门
/// </summary>
[RequireComponent(typeof(Collider))]
public class TeleportationGate : MonoBehaviour
{
    public string destinationScene;
    public Vector3 destinationPosition;
    public Quaternion destinationRotation;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            //场景内跳转
            if (SceneManager.GetActiveScene().name == destinationScene)
            {
                GameObject player = other.gameObject;
                player.GetComponent<Rigidbody>().MovePosition(destinationPosition);
                player.GetComponent<Rigidbody>().MoveRotation(destinationRotation);
            }
            //场景外跳转
            else
            {
                var playerInfo = DataManager.GetInstance().GetData().NPCInfos.Find((x) => x.tag == CharacterTag.Player);
                playerInfo.sceneName = destinationScene;
                playerInfo.position = destinationPosition;
                playerInfo.rotation = destinationRotation;

                NPCController npc = DataManager.GetInstance().GetNPC(playerInfo.id).GetComponent<NPCController>();
                npc.updateLocomotion = false;

                GameManager.GetInstance().LoadScene(destinationScene, () =>
                {

                });
            }
        }
    }
}
