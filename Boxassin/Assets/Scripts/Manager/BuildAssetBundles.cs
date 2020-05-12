using UnityEngine;
using UnityEditor;

public class BuildAssetBundles : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Bundles/Build AssetBundles")]
    static void BuildAllAssetBundles() {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
#endif
}
