
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScenePanel : BasePanel
{
    float alpha = 0;
    float progress = 0;
    bool isEnd = false;


    public void SetAlpha(float alpha)
    {
        this.alpha = alpha;
    }

    public void SetProgress(float progress)
    {
        GetControl<Slider>("LoadProgressBar").gameObject.SetActive(true);
        this.progress = progress;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        GetControl<Slider>("LoadProgressBar").gameObject.SetActive(false);
    }

    private void Update()
    {
        var color = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, alpha);

        if (progress >= 1 && isEnd == false)
        {
            isEnd = true;
            Invoke("HideBar", 0.2f);
        }
        else
        {
            GetControl<Slider>("LoadProgressBar").value = progress;
        }

    }

    void HideBar()
    {
        GetControl<Slider>("LoadProgressBar").gameObject.SetActive(false);
    }
}
