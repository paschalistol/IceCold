using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalPoolObjectBase : MonoBehaviour
{
    protected PoolObject type;
    protected SurvivalPool survivalPool;
    private bool initialized = false;
    
    public void InitPoolObject(PoolObject poolObject, SurvivalPool pool)
    {
        if (!initialized)
        {
            survivalPool = pool;
            type = poolObject;
            initialized = true;
        }
    }
}
