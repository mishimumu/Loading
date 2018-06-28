using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoSingleton<SceneMgr>
{
    /*条件：保证场景中有个不销毁的场景,激活场景非不销毁的场景
    Loading both Synchronous and Asynchronous happens on a thread. The thread loads all the required assets and 
    then it loads all the objects from the scene, when it is done progress is 0.9 or 90%, the last 10% is the
    awake on the main thread. So when you set allowSceneActivation to false, the last10% are never done before
    you set it back to true. This is why scene.isLoaded never becomes true.  Another thing, in your script you
    have to set nextScene active before you unload the previous scene otherwise Unload will set the next scene
    in the SceneManager as active which might not be the scene you want.
    同时加载和异步加载发生在线程上。 线程加载所有必需的资产和
    那么它会加载场景中的所有对象，当它完成时，进度是0.9或90％，最后10％是
    在主线上醒来。 因此，当您将allowSceneActivation设置为false时，最后的10％永远不会完成
    你将其设置回true。 这就是为什么scene.isLoaded永远不会成为现实。 另一件事，在你的脚本你
    必须在卸载前一个场景之前设置nextScene激活，否则Unload将设置下一个场景
    在SceneManager中处于活动状态，这可能不是您想要的场景。

     */

    private bool m_isLoading;
    AsyncOperation m_unloadAsync;
    AsyncOperation m_loadAsync;
    public System.Action<float> FreshProcessEvent;

    public IEnumerator LoadScene(string nextScene)
    {
        Scene curScene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(nextScene) || m_isLoading || curScene.name.Equals(nextScene)) yield break;

        m_isLoading = true;

        SceneManager.LoadScene(SceneConst.SceneName, LoadSceneMode.Additive);

        yield return StartCoroutine(AsyncLoadScene(nextScene));
        FreshProcessEvent.Invoke(SceneConst.LOADSCENEPROCESS);
        m_unloadAsync = SceneManager.UnloadSceneAsync(curScene);
        /*
        //使用LoadSceneMode.Additive进行场景切换，无法完全释放资源。即使使用Resources.UnloadUnusedAssets()也不行
        //可能出现如下情况：
        //1.资源没有引用了，但是还没有回收
        //2.被UnityEngine.EventSystems.StandaloneInputModule和UnityEngine.EventSystems引用
        //当然如果单纯从一个场景切换另外一个场景，直接使用UnityEngine.SceneManagement.SceneManager.LoadScene("xxx")
        //直接切换场景,这个会进行内存回收
        */
        yield return m_unloadAsync;
        FreshProcessEvent.Invoke(SceneConst.UNSCENEPROCESS);
        //场景中必须有一个对象挂载SceneBase
        SceneBase sceneLoader = UnityTool.GetTopObject("SceneLoader").GetComponent<SceneBase>();
        if (sceneLoader == null)
        {
            Debug.LogError("lack of gameobject of name 'SceneLoader'");
            yield break;
        }

        yield return new WaitUntil(sceneLoader.FinishConfigRes);
        FreshProcessEvent.Invoke(SceneConst.INITNETPROCESS);

        yield return new WaitUntil(sceneLoader.FinishGetData);
        FreshProcessEvent.Invoke(SceneConst.INITCONFIGPROCESS);

        yield return new WaitUntil(sceneLoader.FinishInit);
        FreshProcessEvent.Invoke(SceneConst.INITSCENEPROCESS);

        yield return new WaitForSeconds(1f);
        FreshProcessEvent.Invoke(1f);
        m_loadAsync = SceneManager.UnloadSceneAsync(SceneConst.SceneName);

        yield return m_loadAsync;
        m_isLoading = false;
    }

    IEnumerator AsyncLoadScene(string sceneName)
    {
        m_loadAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!m_loadAsync.isDone)
        {
            //更新进度
            FreshProcessEvent?.Invoke(m_loadAsync.progress * SceneConst.LOADSCENEPROCESS);
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

}

public class SceneConst
{
    public const float LOADSCENEPROCESS = 0.5f;
    public const float UNSCENEPROCESS = 0.6f;
    public const float INITNETPROCESS = 0.7f;
    public const float INITCONFIGPROCESS = 0.8f;
    public const float INITSCENEPROCESS = 0.9f;
    public const string SceneName = "LoadingScene";
}

