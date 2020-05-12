using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MapGenerator : DeleteSingleton<MapGenerator>
{
   
    public Sector[,] arr_Sector = new Sector[5,5];
    int[,] arr_MapTileType = new int[20, 20];

    // Start is called before the first frame update
    private void Awake() {
        StartCoroutine(MapInit());
    }
    
    IEnumerator MapInit() {
        yield return new WaitUntil(() => AssetBundleManager.Instance.b_AllSet == true);

        LoadMapTileType();
        InitSetcor();
    }
    void InitSetcor() {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                arr_Sector[i, j] = Instantiate(AssetBundleManager.Instance.arr_Prefabs["Sector"], new Vector3(2f + i * 4, -0.5f, 2f + j * 4), Quaternion.identity, BoxassinManager.Instance.m_Maps).transform.GetComponent<Sector>();
                arr_Sector[i, j].SetIndex(i, j);
            }
        }
    }
    void LoadMapTileType() {
        string strFile = Application.dataPath + "/MapTile.csv";

        string text = System.IO.File.ReadAllText(strFile);
        byte[] byteArray = Encoding.UTF8.GetBytes(text);
        MemoryStream stream = new MemoryStream(byteArray);
        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8, false)) {
            string strLineValue = null;
            string[] keys = null;
            string[] values = null;
            int line = 0;

            while ((strLineValue = sr.ReadLine()) != null) {
                // Must not be empty.
                if (string.IsNullOrEmpty(strLineValue)) return;

                if (strLineValue.Substring(0, 1).Equals("#")) {
                    keys = strLineValue.Split(',');

                    keys[0] = keys[0].Replace("#", "");
                    continue;
                }

                values = strLineValue.Split(',');

                for (int i = 0; i < values.Length; i++) {
                    arr_MapTileType[line, i] = int.Parse(values[i]);
                }
                line++;
            }
        }
        Debug.Log("CharacterSettingLoad Success");

        InitMap();
    }

    void InitMap() {
        for(int i = 0; i < 20; i++) {
            for(int j = 0; j < 20; j++) {
                string type = ((EnumData.TileType)arr_MapTileType[i, j]).ToString();
                if(type != "Road")
                    Instantiate(AssetBundleManager.Instance.arr_Prefabs[type],new Vector3(0.5f + i, 0f, 0.5f + j), Quaternion.identity, BoxassinManager.Instance.m_Maps);
            }
        }
    }
}
