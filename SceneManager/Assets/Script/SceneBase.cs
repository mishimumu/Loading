using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneBase : MonoBehaviour
{
    //gameobj name需要用SceneLoader

    private void Awake()
    {
        GameAwake();
    }


    protected virtual void GameAwake()
    {

    }

    public virtual bool FinishConfigRes()
    {
        return true;
    }

    public virtual bool FinishGetData()
    {
        return true;
    }

    public virtual bool FinishInit()
    {
        return true;
    }
}

