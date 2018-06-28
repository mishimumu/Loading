using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoSingleton<LoadingManager>
{

    public void Load(string name)
    {
        StartCoroutine(SceneMgr.Instance.LoadScene(name));
    }

}
