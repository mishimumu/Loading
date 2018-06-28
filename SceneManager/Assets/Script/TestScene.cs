using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : SceneBase
{
    private bool para1 = true;
    private bool para2 = true;
    protected override void GameAwake()
    {
        base.GameAwake();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            para1 = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            para2 = true;
        }
    }

    public override bool FinishConfigRes()
    {
        return para1;
    }

    public override bool FinishGetData()
    {
        return para2;
    }
}

