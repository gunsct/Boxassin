using UnityEngine;
using System.Collections;
/*
  Name : 유병훈
  Date : 16.10.24
  Contants : 사운드 플레이에 쓰이는 오브젝트, 사운드 모드, 오디오 클립 데이터
  Operations : new로 정의해서 사용
 */

public class SoundPlayData {
    private GameObject m_soundObj;
    public GameObject soundObj{
        get{return m_soundObj;}
        set { m_soundObj = value; }
    }

    private AudioClip m_audioClip;
    public AudioClip audioClip {
        get { return m_audioClip; }
        set { m_audioClip = value; }
    }

    private float m_volume;
    public float volume {
        get { return m_volume; }
        set { m_volume = value; }
    }

    private bool b_loop;
    public bool loop {
        get { return b_loop; }
        set { b_loop = value; }
    }
}
