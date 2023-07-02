using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonPanel : MonoBehaviour
{
    public void SetInteractButton(InteractButton button)
    {
        transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = button.description;
        GetComponent<Button>().onClick.AddListener(() => button.onClick.Invoke());
    }
}
