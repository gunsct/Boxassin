using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGenerator : MonoBehaviour
{
    public EnumData.GeneratorType m_Type;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(AssetBundleManager.Instance.arr_Prefabs[m_Type.ToString()], transform.position, Quaternion.identity, BoxassinManager.Instance.m_Actors);   
    }
}
