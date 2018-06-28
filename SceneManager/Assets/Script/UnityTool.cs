using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityTool
{

    public static GameObject GetTopObject(string objName)
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] objs = scene.GetRootGameObjects();
        foreach (var o in objs)
        {
            if (o.name == objName)
            {
                return o;
            }
        }
        return null;
    }
}
