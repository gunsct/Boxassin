using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BundleInfo {
    public string m_Name;
    public string m_URL;
    public int m_Version;
    public AssetBundle m_AssetBundle;
}
public class AssetBundleManager : MonoBehaviour
{
    bool b_AllLoad;
    public BundleInfo[] arr_BundleInfo;
    public Dictionary<string, BundleInfo> arr_AssetBundles = new Dictionary<string, BundleInfo>();
    public List<GameObject> arr_Prefabs = new List<GameObject>();

    private void Start() {
        StartCoroutine(DownloadNCache());
        StartCoroutine(LoadBundle());
    }

    IEnumerator DownloadNCache() {
        while (!Caching.ready) {
            yield return null;
        }
        for (int i = 0; i < arr_BundleInfo.Length; i++) {
            using (WWW www = WWW.LoadFromCacheOrDownload(arr_BundleInfo[i].m_URL, arr_BundleInfo[i].m_Version)) {
                yield return www;
                if (www.error != null) {
                    throw new Exception("WWW 다운 에러 : " + www.error);
                }
                else if (www != null) {
                    arr_BundleInfo[i].m_AssetBundle = www.assetBundle;
                    arr_AssetBundles.Add(arr_BundleInfo[i].m_Name, arr_BundleInfo[i]);
                    //arr_BundleInfo[i].m_AssetBundle.Unload(false);
                    www.Dispose();
                }
            }
        }
        b_AllLoad = true;
    }
    IEnumerator LoadBundle() {
        yield return new WaitUntil(() => b_AllLoad == true);
        StartCoroutine(LoadPrefabs());
        StartCoroutine(LoadSound());
        StartCoroutine(LoadEffect());
    }
    IEnumerator LoadPrefabs() {
        for (int i = 0; i < 3; i++) {
            AssetBundleRequest request = arr_AssetBundles["prefabs"].m_AssetBundle.LoadAssetAsync("BOX" + (i + 1).ToString(), typeof(GameObject));
            yield return request;

            arr_Prefabs.Add(request.asset as GameObject);

            GameObject obj = Instantiate(arr_Prefabs[i]);
            obj.transform.position = new Vector3(-10f + i * 10f, 0f, 0f);
        }
    }
    IEnumerator LoadSound() {
        for(int i = 0; i < SoundPlayManager.Instance.m_ClipsName.Length; i++) {
            AssetBundleRequest request = arr_AssetBundles["sound"].m_AssetBundle.LoadAssetAsync(SoundPlayManager.Instance.m_ClipsName[i], typeof(AudioClip));
            yield return request;

            SoundPlayManager.Instance.arr_AudioClip.Add(SoundPlayManager.Instance.m_ClipsName[i], request.asset as AudioClip);
        }

        SoundPlayManager.Instance.EffectSound("01. The Weeknd - Blinding Lights");
    }
    IEnumerator LoadEffect() {
        for (int i = 0; i < EffectManager.Instance.arr_EffectsName.Length; i++) {
            AssetBundleRequest request = arr_AssetBundles["effect"].m_AssetBundle.LoadAssetAsync(EffectManager.Instance.arr_EffectsName[i], typeof(GameObject));
            yield return request;

            EffectManager.Instance.arr_Effects.Add(EffectManager.Instance.arr_EffectsName[i], request.asset as GameObject);
        }
        EffectManager.Instance.EffectTurnOn("CFX_Hit_C White", new Vector3(0,5,0), Vector3.one);
    }
}
