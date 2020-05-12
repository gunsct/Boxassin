using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SelfActiveOff : MonoBehaviour {
    //이펙트에 달아서 사용할것
    public float m_during, m_timer;
    public ParticleSystem m_particle;
    //public VisualEffect m_vfx;
    public string m_StartName = "VFXPlay";
    public string m_EndName = "VFXStop";

    private void Awake() {
        //if (GetComponent<VisualEffect>())
            //m_vfx = GetComponent<VisualEffect>();
        if (GetComponent<ParticleSystem>())
            m_particle = GetComponent<ParticleSystem>();
    }
    private void Start() {
        On();
    }
    void OnEnable() {
        On();
    }
    private void Update() {
        m_timer += Time.deltaTime;
        if(m_timer >= m_during) {
            m_timer = 0f;
            //if (m_vfx != null) {
            //    m_vfx.SendEvent(m_EndName);
            //    gameObject.SetActive(false);
            //}
            if(m_particle != null)
                gameObject.SetActive(false);
        }
    }
    void On() {
        if (m_particle != null)
            if (!m_particle.isPlaying)
                m_particle.Play();
        //if (m_vfx != null) {
        //    m_vfx.SendEvent(m_StartName);
        //}
    }
}
