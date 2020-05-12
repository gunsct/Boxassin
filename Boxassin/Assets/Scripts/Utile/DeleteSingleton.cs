using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeleteSingleton<T> : MonoBehaviour where T : DeleteSingleton<T> {
    private static T m_Instance = null;
    private static bool b_ReMake = true;

    public static T Instance {
        get {
            if (m_Instance == null && b_ReMake) {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;

                if (m_Instance == null) {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    m_Instance = obj.AddComponent(typeof(T)) as T;
                }

                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private void Awake() {
        if (m_Instance == null) {
            m_Instance = this as T;
            m_Instance.Init();
        }
    }

    public virtual void Init() { } // 초기화를 상속을 통해 구현    

    private void OnApplicationQuit() {
        if (m_Instance != null) {
            m_Instance = null;
            b_ReMake = false;
        }
    }

    public virtual bool IsValid() {
        if (m_Instance != null)
            return true;
        else
            return false;
    }

    public virtual void SceneStop(bool StopCheck) {

    }
    
}

