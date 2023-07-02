using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimonIsHere : MonoBehaviour
{
    public Transform generatePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.activeInHierarchy)
        {
            if (other.gameObject.tag == "Player")
            {
                Task task = TaskManager.GetInstance().Tasks.Find((x) => x.id == 2);
                if (task != null && task.state == TaskState.Completed)
                {
                    NPCInfo simon = DataManager.GetInstance().GetData().NPCInfos.Find((x) => x.id == 2);
                    simon.position = generatePoint.position;
                    simon.rotation = generatePoint.rotation;
                    simon.sceneName = SceneManager.GetActiveScene().name;
                    simon.tag = CharacterTag.Ally;

                    ResMgr.GetInstance().LoadAsync<GameObject>(simon.prefabName, null);

                    GetComponent<SceneObjectController>().SetActive(false);
                }
            }
        }
    }
}
