using UnityEngine;
using UnityEditor;

public class ShaderConverterTool
{
    private const string TARGET_SHADER = "Particles/Standard Unlit";

    [MenuItem("Tools/Fix Shaders/Convert Selected To Particles Unlit")]
    static void ConvertSelected()
    {
        Shader targetShader = Shader.Find(TARGET_SHADER);
        if (targetShader == null)
        {
            Debug.LogError("Không tìm thấy shader: " + TARGET_SHADER);
            return;
        }

        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(path))
                continue;

            Material[] materials = AssetDatabase.LoadAllAssetsAtPath(path) as Material[];

            foreach (Material mat in materials)
            {
                if (mat != null)
                {
                    ConvertMaterial(mat, targetShader);
                }
            }

            // Nếu là prefab/model
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null)
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer renderer in renderers)
                {
                    foreach (Material mat in renderer.sharedMaterials)
                    {
                        if (mat != null)
                        {
                            ConvertMaterial(mat, targetShader);
                        }
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Đã convert shader cho selected objects.");
    }

    [MenuItem("Tools/Fix Shaders/Convert All Materials In Project")]
    static void ConvertAll()
    {
        Shader targetShader = Shader.Find(TARGET_SHADER);
        if (targetShader == null)
        {
            Debug.LogError("Không tìm thấy shader: " + TARGET_SHADER);
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Material");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null)
            {
                ConvertMaterial(mat, targetShader);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Đã convert toàn bộ materials trong project.");
    }

    static void ConvertMaterial(Material mat, Shader targetShader)
    {
        if (mat.shader == targetShader)
            return;

        // Giữ lại texture chính nếu có
        Texture mainTex = null;
        if (mat.HasProperty("_MainTex"))
        {
            mainTex = mat.GetTexture("_MainTex");
        }

        mat.shader = targetShader;

        if (mainTex != null && mat.HasProperty("_MainTex"))
        {
            mat.SetTexture("_MainTex", mainTex);
        }

        EditorUtility.SetDirty(mat);
    }
}