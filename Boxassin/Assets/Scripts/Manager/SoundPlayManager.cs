using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
/*
  Name : 유병훈
  Date : 16.10.17
  Contants
   사운드의 타입과 VR,Non VR 모드에 따라 원하는 오브젝트에 2d or 3d 사운드 효과를 낼 수 있고 뷰 전환시 배경음 fade in/out 기능 포함.
   SoundPlayManager 라는 오브젝트가 생성되고 SoundPlayManager, SoundFade 스크립트가 붙어서 씬이 바뀌어도 사라지지않고 계속해서 사용 할 수 있음.
  Operations :
   SoundPlayManager.instance.SoundPlay((AudioClip)ResourcesLoaderManager.instance.GetSoundAudioclip(1,1),(int)SoundDataEnum.SOUND_TYPE.EFFECT_SOUND, this.gameObject);
 */

public class SoundPlayManager : DeleteSingleton<SoundPlayManager> {        
    private int CHILD_COUNT = 30;
    private GameObject[] m_childObj;
    [SerializeField]
    private List<GameObject> m_emptyAudioClip = new List<GameObject>();
    
    private float m_pauseDelay = 0.0f;
    AudioHighPassFilter m_highPassFilter;

    [SerializeField]
    private int m_bgmNum = 8;
    public int BGM_NUM { get { return m_bgmNum; } }
    public AudioClip[] BGM_LIST;

    public AudioClip[] m_AudioClips;
    public string[] m_ClipsName;
    public Dictionary<string, AudioClip> arr_AudioClip = new Dictionary<string, AudioClip>();

    private AudioSource m_audioSource;//
    public AudioSource audioSource {
        get { return m_audioSource; }
        set { m_audioSource = value; }
    }

    [SerializeField]//기타 외부에서 사운드 추가시 사용
    private AudioClip[] m_etcAudioList;
    public AudioClip[] EtcAudioList {
        get { return m_etcAudioList; }
        set { m_etcAudioList = value; }
    }

    private SoundPlayData m_soundPlayData;//셋팅에 필요한 데이터
    public SoundPlayData soundPlayData {
        get { return m_soundPlayData; }
        set { m_soundPlayData = value; }
    }

    private SoundPlayData m_soundPlayDataEft;//셋팅에 필요한 데이터
    public SoundPlayData soundPlayDataEft {
        get { return m_soundPlayDataEft; }
        set { m_soundPlayDataEft = value; }
    }

    private float m_PlayTime;
    public float PlayTime {
        get { return m_PlayTime; }
        set { m_PlayTime = value; }
    }

    public override void Init() {
        m_audioSource = this.GetComponent<AudioSource>();
        m_highPassFilter = GetComponent<AudioHighPassFilter>();

        m_childObj = new GameObject[CHILD_COUNT];
        for (int i = 0; i < CHILD_COUNT; i++) {
            GameObject empty = new GameObject("EmptyClip");
            empty.AddComponent<AudioSource>();
            empty.transform.parent = transform;
            m_childObj[i] = empty;
        }
        if (m_AudioClips.Length != 0) {
            for (int i = 0; i < m_AudioClips.Length; i++) {
                if (m_AudioClips[i] != null) {
                    arr_AudioClip.Add(m_AudioClips[i].name, m_AudioClips[i]);
                }
            }
        }
        m_soundPlayData = new SoundPlayData();
        m_soundPlayDataEft = new SoundPlayData();
    }
    
    
    void Update() {
        for (int i = 0; i < CHILD_COUNT; i++) { 
            if (m_childObj[i].GetComponent<AudioSource>().isPlaying.Equals(false)) {
                m_childObj[i].GetComponent<AudioSource>().clip = null;
                if(!m_emptyAudioClip.Contains(m_childObj[i]))
                    m_emptyAudioClip.Add(m_childObj[i]);
            }
        }
        //HighPassFilterSmaller();
    }

    GameObject EmptyAudioSource() {
        GameObject obj = null;

        obj = m_emptyAudioClip[0];
        m_emptyAudioClip.Remove(obj);

        return obj;
    }

    #region DEFAULT_FUNCTION
    //오디오 클립 교체
    public void AudioChange(GameObject obj, AudioClip audioClip) {
        obj.GetComponent<AudioSource>().clip = audioClip;
        obj.GetComponent<AudioSource>().Play();
    }
    public void AudioChange(AudioSource audioSource, AudioClip audioClip) {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    //볼륨 조절
    public void AudioVolume(float volumeSize, GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        obj.GetComponent<AudioSource>().volume = volumeSize;
    }

    //볼륨 페이드, 배경음용
    WaitForSeconds m_wfs;
    public Coroutine AudioFade(bool inout, float interval) {
        m_wfs = new WaitForSeconds(interval);
        return StartCoroutine(FadeInOut(inout));
    }

    IEnumerator FadeInOut(bool inout) {
        yield return m_wfs;
        if (inout) {
            this.GetComponent<AudioSource>().volume += 0.1f;
        }
        else {
            this.GetComponent<AudioSource>().volume -= 0.1f;
        }
        if(this.GetComponent<AudioSource>().volume < 1f && this.GetComponent<AudioSource>().volume > 0f)
            StartCoroutine(FadeInOut(inout));
    }

    //뮤트
    public void AudioMuteOn(GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        if (obj.GetComponent<AudioSource>() != null)
            obj.GetComponent<AudioSource>().mute = true;
    }
    public void AudioMuteOn(AudioSource audioSource = null) {
        if (!audioSource)
            audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.mute = true;
    }

    public void AudioMuteOff(GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        if (obj.GetComponent<AudioSource>() != null)
            obj.GetComponent<AudioSource>().mute = false;
    }
    public void AudioMuteOff(AudioSource audioSource = null) {
        if (!audioSource)
            audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.mute = false;
    }

    //정지
    public void AudioStop(GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        if(obj.GetComponent<AudioSource>() != null)
            obj.GetComponent<AudioSource>().Stop();
    }
    public void AudioStop(AudioSource audioSource) {
        audioSource.Stop();
    }
    public void AudioAllStop(bool bgmImport) {//bgm포함 재생중인 전체 사운드인지 가려서 작동
        if (bgmImport)
            m_audioSource.Stop();

        for (int i = 0; i < CHILD_COUNT; i++)
            if (m_childObj[i].GetComponent<AudioSource>().clip)
                m_childObj[i].GetComponent<AudioSource>().Stop();
    }

    //일시정지
    public void AudioPauseOn(GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        if (obj.GetComponent<AudioSource>() != null)
            obj.GetComponent<AudioSource>().Pause();
    }
    public void AudioPauseOn(AudioSource audioSource) {
        audioSource.Pause();
    }
    public void AudioAllPauseOn(bool bgmImport) {//bgm포함 재생중인 전체 사운드인지 가려서 작동
        if (bgmImport)
            m_audioSource.Pause();

        for (int i = 0; i < CHILD_COUNT; i++)
            if(m_childObj[i].GetComponent<AudioSource>().clip)
                 m_childObj[i].GetComponent<AudioSource>().Pause();
    }

    //일시정지 해제
    public void AudioPauseOff(GameObject obj = null) {
        if (!obj)
            obj = this.gameObject;
        if (obj.GetComponent<AudioSource>() != null)
            obj.GetComponent<AudioSource>().UnPause();
    }
    public void AudioPauseOff(AudioSource audioSource) {
        audioSource.UnPause();
    }
    public void AudioAllPauseOff(bool bgmImport) {
        if (bgmImport)
            m_audioSource.UnPause();

        for (int i = 0; i < CHILD_COUNT; i++)
            m_childObj[i].GetComponent<AudioSource>().UnPause();
    }

    //일시정지의 정지시간을 지정
    public void AudioPauseDelay(float delay = 0.0f, GameObject obj = null ) {
        m_pauseDelay = delay;
        if (!obj)
            obj = EmptyAudioSource();

        if (!obj.GetComponent<AudioSource>()) {
            obj.AddComponent<AudioSource>();
        }
        StartCoroutine("PauseDelay", obj.GetComponent<AudioSource>());
    }
    public void AudioPauseDelay(float delay = 0.0f, AudioSource audioSource = null) {
        m_pauseDelay = delay;

        if (!audioSource) {
            if (!EmptyAudioSource().GetComponent<AudioSource>())
                EmptyAudioSource().AddComponent<AudioSource>();
            
            audioSource = EmptyAudioSource().AddComponent<AudioSource>();
        }
        
        StartCoroutine("PauseDelay", audioSource);
    }
    public void AudioAllPauseDelay(bool bgmImport, float delay = 0.0f) {
        m_pauseDelay = delay;

        StartCoroutine("AllPauseDelay", bgmImport);
    }

    IEnumerator PauseDelay(AudioSource audioSource) {
        audioSource.Pause();

        yield return new WaitForSeconds(m_pauseDelay);

        audioSource.UnPause();

        yield return null;
    }

    IEnumerator AllPauseDelay(bool bgmImport) {
        AudioAllPauseOn(bgmImport);

        yield return new WaitForSeconds(m_pauseDelay);

        AudioAllPauseOff(bgmImport);

        yield return null;
    }

    #endregion

    #region PLAY_AND_DELAY
    /*사운드 플레이 사용 방법
     * 인자로 들어가는 타입들, 상수로 넣는것이 짧고 좋음.
     * 사운드 타입 => (int)SoundDataEnum.SOUND_ATTACH_TYPE [OBJECT_SOUND, BGM_SOUND]
     *                (int)SoundDataEnum.SOUND_TYPE [ALPHABET_SOUND, LETTER_SOUND, WORD_SOUND, EFFECT_SOUND, BGM_SOUND]
     *                (int)AlphabetDataEnum.SOUND_HUMAN_TYPE [WOMAN_SOUND, GIRL_TYPE, MAN_TYPE] 
     * 사용법              
     * 효과음 및 배경음 => SoundPlayManager.instance.SoundPlay((int)SoundDataEnum.SOUND_ATTACH_TYPE.OBJECT_SOUND, (int)SoundDataEnum.SOUND_TYPE.EFFECT_SOUND, 1, 1, this.gameObject, delayTime);
     * 알파벳 및 단어음 => SoundPlayManager.instance.SoundPlay((int)SoundDataEnum.SOUND_ATTACH_TYPE.OBJECT_SOUND, (int)SoundDataEnum.SOUND_TYPE.EFFECT_SOUND,
     *                                          (int)AlphabetDataEnum.SOUND_HUMAN_TYPE.WOMAN_SOUND, soundId, listId , this.gameObject, delayTime);
    */

    //사운드 모드, 타입, 휴먼타입, 아이디, 리스트 아이디, 오브젝트, 딜레이 타임을 인자로 오디오 클립을 추출해 사운드 셋팅에 넘김. 필요에 따라 딜레이 기능도 사용 가능.
    //아마 알파벳, 레러, 이펙트, 비지엠으로 크게 네개로 나눠쓸듯
    //리소스 로더 만들어지면 주석 풀고
    public float AnimationSound(string name, float volume = 0.5f, GameObject obj = null, float delayTime = 0.0f, float whileTime = 0.0f) {
        soundPlayData.audioClip = ResourceLoader.Instance.LoadAudioClip(name);
        soundPlayData.soundObj = obj;
        soundPlayData.volume = volume;
        soundPlayData.loop = false;

        DelaySound(delayTime, whileTime);

        return soundPlayData.audioClip.length;
    }

    public float EffectSound(string name, float volume = 1.0f, GameObject obj = null, float delayTime = 0.0f, float whileTime = 0.0f) {
        if (m_emptyAudioClip.Count != 0 && arr_AudioClip.ContainsKey(name)) {
            soundPlayDataEft.audioClip = arr_AudioClip[name]; //ResourceLoader.Instance.LoadAudioClip(name);
            soundPlayDataEft.soundObj = obj;
            soundPlayDataEft.volume = volume;
            soundPlayDataEft.loop = false;

            DelaySound(delayTime, whileTime, 1);

            return soundPlayDataEft.audioClip.length;
        }
        return 0f;
    }
    public AudioSource EffectSoundLoop(string name, float volume = 1.0f, GameObject obj = null, float delayTime = 0.0f, float whileTime = 0.0f) {
        if (m_emptyAudioClip.Count != 0 && arr_AudioClip.ContainsKey(name)) {
            soundPlayDataEft.audioClip = arr_AudioClip[name];
            soundPlayDataEft.soundObj = obj;
            soundPlayDataEft.volume = volume;
            soundPlayDataEft.loop = true;
            
            return DelaySound(delayTime, whileTime, 1).GetComponent<AudioSource>();
        }
        return null;
    }

    public float BGMSound(string name, bool loop = true, float volume = 1f, float delayTime = 0.0f, float whileTime = 0.0f) {
        soundPlayData.audioClip = ResourceLoader.Instance.LoadAudioClip(name);
        soundPlayData.soundObj = this.gameObject;
        soundPlayData.volume = volume;
        soundPlayData.loop = true;

        DelaySound(delayTime, whileTime);

        return soundPlayData.audioClip.length;
    }
    public float BGMSoundList(int index, bool loop = true, float volume = 1f, float delayTime = 0.0f, float whileTime = 0.0f) {
        soundPlayData.audioClip = BGM_LIST[index];
        soundPlayData.soundObj = this.gameObject;
        soundPlayData.volume = volume;
        soundPlayData.loop = loop;

        DelaySound(delayTime, whileTime);

        return soundPlayData.audioClip.length;
    }
    public float EtcSound(string name, float volume = 1.0f, GameObject obj = null, float delayTime = 0.0f, float whileTime = 0.0f) {
        soundPlayData.audioClip = ResourceLoader.Instance.LoadAudioClip(name);
        soundPlayData.soundObj = obj;
        soundPlayData.volume = volume;
        soundPlayData.loop = false;

        DelaySound(delayTime, whileTime);

        return soundPlayData.audioClip.length;
    }

    public float SelfInsertSound(AudioClip audioclip, float volume = 1.0f, GameObject obj = null, float delayTime = 0.0f, float whileTime = 0.0f) {
        soundPlayData.audioClip = audioclip;
        soundPlayData.soundObj = obj;
        soundPlayData.volume = volume;
        soundPlayData.loop = false;

        DelaySound(delayTime, whileTime);

        return soundPlayData.audioClip.length;
    }

    GameObject DelaySound(float delayTime, float whileTime = 0, int type = 0) {//type 0은 나머지 1이면 effect
        GameObject soundobj = null;
        if (type != 1) {
            if (!soundPlayData.soundObj)
                soundPlayData.soundObj = EmptyAudioSource();
            soundobj = soundPlayData.soundObj;
        }
        else {
            if (!soundPlayDataEft.soundObj)
                soundPlayDataEft.soundObj = EmptyAudioSource();
            soundobj = soundPlayDataEft.soundObj;
        }

        StartCoroutine(DelaySoundSetting(type, delayTime, whileTime));

        return soundobj;
    }

    //SoundPlayData의 변수를 가져와 게임 모드와 오브젝트의 사운드 모드를 선택해 재생.
    AudioSource SoundSetting() {
        GameObject obj = soundPlayData.soundObj;
        AudioClip audioClip = soundPlayData.audioClip;

        if (!obj.GetComponent<AudioSource>()) {
            obj.AddComponent<AudioSource>();
        }

        AudioSource objSource = obj.GetComponent<AudioSource>();
        objSource.volume = soundPlayData.volume;
        objSource.clip = audioClip;
        objSource.playOnAwake = true;

        objSource.loop = soundPlayData.loop;
        objSource.Play();

        return objSource;
    }

    //SoundPlayData의 변수를 가져와 게임 모드와 오브젝트의 사운드 모드를 선택해 재생.
    AudioSource SoundSetting2() {
        GameObject obj = soundPlayDataEft.soundObj;
        AudioClip audioClip = soundPlayDataEft.audioClip;

        if (!obj.GetComponent<AudioSource>()) {
            obj.AddComponent<AudioSource>();
        }

        AudioSource objSource = obj.GetComponent<AudioSource>();
        objSource.volume = soundPlayDataEft.volume;
        objSource.clip = audioClip;
        objSource.playOnAwake = true;

        objSource.loop = soundPlayDataEft.loop;
        objSource.Play();

        return objSource;
    }
    IEnumerator DelaySoundSetting(int _type, float _delay, float _while) {
        yield return new WaitForSeconds(_delay);
        AudioSource source = null;
        if(_type != 1) {
            source = SoundSetting();
        }
        else {
            source = SoundSetting2();
        }
        if (_while != 0f) {
            yield return new WaitForSeconds(_while);
            source.Stop();
        }
    }
    #endregion

    float m_highAmount = 0.999f;
    void HighPassFilterSmaller() {
        if (m_highPassFilter.cutoffFrequency > 10f) {
            m_highPassFilter.cutoffFrequency *= m_highAmount;
            m_highAmount -= 0.0002f;
        }
        if (m_highPassFilter.cutoffFrequency < 10f) {
            m_highPassFilter.cutoffFrequency = 10f;
        }
    }
    public void HighPassFilterBigger() {
        m_highAmount = 0.999f;
        m_highPassFilter.cutoffFrequency = 3000f;
    }
}
