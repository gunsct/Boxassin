using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class EffectManager : DeleteSingleton<EffectManager> {
    public Dictionary<string, List<GameObject>> arr_effect_pool = new Dictionary<string, List<GameObject>>();
    public GameObject[] arr_EffectPrefabs;
    public string[] arr_EffectsName;
    public Dictionary<string, GameObject> arr_Effects = new Dictionary<string, GameObject>();

    //이펙트 프리팹 딕셔너리에 전부 저장
    public override void Init() {
        base.Init();
        if (arr_EffectPrefabs.Length != 0) {
            for (int i = 0; i < arr_EffectPrefabs.Length; i++) {
                if (arr_EffectPrefabs[i] != null) {
                    arr_Effects.Add(arr_EffectPrefabs[i].name, arr_EffectPrefabs[i]);
                }
            }
        }
    }

    //소환할 이펙트 켜기
    public GameObject EffectTurnOn(string name, Vector3 pos, Vector3 scale, float during = 2f, string startname = "OnPlay", string endname = "OnStop") {//0초면 지속형
        GameObject effect = null;
        if (arr_effect_pool.ContainsKey(name)) {//풀에 키가 있음
            for (int i = 0; i < arr_effect_pool[name].Count; i++) {
                if (arr_effect_pool[name][i].activeSelf == false) {//꺼져있는 이펙트 발견시 키고 반환함
                    effect = arr_effect_pool[name][i];
                    effect.SetActive(true);
                    break;
                }
            }
        }
        if (effect == null) {//풀에서 사용가능한 이펙트가 없을경우 새로 생성해서 풀에 넣어줌
            effect = Instantiate(arr_Effects[name], pos, arr_Effects[name].transform.rotation, transform) as GameObject;
            if (during != 0f) {
                effect.AddComponent<SelfActiveOff>().m_during = during;
                effect.GetComponent<SelfActiveOff>().m_StartName = startname;
                effect.GetComponent<SelfActiveOff>().m_EndName = endname;
            }
            effect.name = name;

            //키가 있는 경우 없는경우
            if (arr_effect_pool.ContainsKey(name)) {
                arr_effect_pool[name].Add(effect);
            }
            else {
                List<GameObject> list = new List<GameObject>();
                list.Add(effect);
                arr_effect_pool.Add(name, list);
            }
        }

        effect.transform.position = pos;
        effect.transform.localScale = scale;//스케일은 오브젝트에 맞춰 미리 세팅해둔다면 필요없어질것

        return effect;
    }

    //이펙트 끄기
    public void EffectTurnOff(string key) {
        if (arr_effect_pool.ContainsKey(key) && arr_effect_pool[key] != null) {
            GameObject effect = arr_effect_pool[key][0];
            effect.SetActive(false);
        }
    }

    //이펙트 정지
    public void EffectPause(string key) {
        if (arr_effect_pool.ContainsKey(key) && arr_effect_pool[key] != null) {
            GameObject effect = arr_effect_pool[key][0];
            effect.GetComponent<ParticleSystem>().Pause(true);
        }
    }
}
