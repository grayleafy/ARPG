using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// 知识点
/// 1.场景异步加载
/// 2.协程
/// 3.委托
/// </summary>
public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// 切换场景 同步
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name, UnityAction fun)
    {
        //场景同步加载
        SceneManager.LoadScene(name);
        //加载完成过后 才会去执行fun
        fun();
    }

    /// <summary>
    /// 提供给外部的 异步加载的接口方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name, UnityAction fun)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// 协程异步加载场景,完成时间在脚本周期中不可控，设置脚本顺序优先级？
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction fun)
    {
        //场景切换面板
        LoadScenePanel loadScenePanel = null;
        bool openFinished = false;
        UIManager.GetInstance().ShowPanel<LoadScenePanel>("LoadScenePanel", E_UI_Layer.System, (panel) =>
        {
            loadScenePanel = panel;
            openFinished = true;
        });
        while (!openFinished)
        {
            yield return null;
        }
        //渐变
        float alpha = 0;
        float speed = 1;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime * speed;
            loadScenePanel.SetAlpha(alpha);
            yield return null;
        }


        //加载新场景
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //可以得到场景加载的一个进度
        while (!ao.isDone)
        {
            //事件中心 向外分发 进度情况  外面想用就用
            EventCenter.GetInstance().EventTrigger("进度条更新", ao.progress);
            loadScenePanel.SetProgress(ao.progress);
            //这里面去更新进度条
            yield return ao.progress;
        }
        EventCenter.GetInstance().EventTrigger("进度条更新", 1);
        loadScenePanel.SetProgress(1);
        //加载完成过后 才会去执行fun
        if (fun != null) fun();


        //渐变
        alpha = 1;
        speed = 1;
        yield return new WaitForSeconds(0.5f);
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * speed;
            loadScenePanel.SetAlpha(alpha);
            yield return null;
        }
        UIManager.GetInstance().HidePanel("LoadScenePanel");
    }


}
