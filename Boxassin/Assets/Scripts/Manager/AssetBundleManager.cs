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
public class AssetBundleManager : DeleteSingleton<AssetBundleManager>
{
    public bool b_AllLoad, b_SetPrefabs, b_SetEffect, b_SetSound, b_AllSet;
    public BundleInfo[] arr_BundleInfo;
    public Dictionary<string, BundleInfo> arr_AssetBundles = new Dictionary<string, BundleInfo>();
    public Dictionary<string, GameObject> arr_Prefabs = new Dictionary<string, GameObject>();

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

        yield return new WaitUntil(() => b_SetPrefabs && b_SetEffect && b_SetSound == true);
        b_AllSet = true;
    }
    IEnumerator LoadPrefabs() {
        AssetBundleRequest request = arr_AssetBundles["prefabs"].m_AssetBundle.LoadAllAssetsAsync();
        yield return request;
        for (int i = 0; i < request.allAssets.Length; i++) {
            arr_Prefabs.Add(request.allAssets[i].name, request.allAssets[i] as GameObject);
        }
        b_SetPrefabs = true;
    }
    IEnumerator LoadSound() {
        for(int i = 0; i < SoundPlayManager.Instance.m_ClipsName.Length; i++) {
            AssetBundleRequest request = arr_AssetBundles["sound"].m_AssetBundle.LoadAssetAsync(SoundPlayManager.Instance.m_ClipsName[i], typeof(AudioClip));
            yield return request;

            SoundPlayManager.Instance.arr_AudioClip.Add(SoundPlayManager.Instance.m_ClipsName[i], request.asset as AudioClip);
        }
        b_SetSound = true;
    }
    IEnumerator LoadEffect() {
        for (int i = 0; i < EffectManager.Instance.arr_EffectsName.Length; i++) {
            AssetBundleRequest request = arr_AssetBundles["effect"].m_AssetBundle.LoadAssetAsync(EffectManager.Instance.arr_EffectsName[i], typeof(GameObject));
            yield return request;

            EffectManager.Instance.arr_Effects.Add(EffectManager.Instance.arr_EffectsName[i], request.asset as GameObject);
        }
        b_SetEffect = true;
    }
}
