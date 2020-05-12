using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : DeleteSingleton<ResourceLoader> {
    //PathData에 해당 이름 넣으면 경로 자동 지정
    public Texture2D LoadTexture(string path) {
        Texture2D texture = Resources.Load<Texture2D>(path);
        return texture;
    }
    public Sprite LoadSprite(string path) {
        Sprite sprite = Resources.Load<Sprite>(path);
        return sprite;
    }

    public GameObject LoadPrefabs(string name) {
        GameObject obj = Resources.Load("Prefabs/" + name) as GameObject;
        return obj;
    }

    public GameObject LoadEffects(string name) {
        GameObject obj = Resources.Load("FX/" + name) as GameObject;
        return obj;
    }

    public AudioClip LoadAudioClip(string name) {
        AudioClip clip = Resources.Load("Sound/" + name) as AudioClip;
        return clip;
    }
}
