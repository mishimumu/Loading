using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneTest : MonoBehaviour
{
    public Button btn;
    public string sceneName;
    // Use this for initialization
    void Start()
    {
        btn.onClick.AddListener(() => {
            //不能直接这边调用StartCoroutine(SceneMgr.Instance.LoadScene("xx"))，否则会导致场景无法卸载完成。
            //因为这个协程还在调用，导致场景还未释放
            LoadingManager.Instance.Load(sceneName);
        });
    }

}
