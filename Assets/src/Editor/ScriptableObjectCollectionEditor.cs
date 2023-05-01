using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

abstract public class ScriptableObjectCollectionEditor<T> : Editor where T : ScriptableObject
{

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            var list = (ScriptableObjectCollection<T>)target;
            list.List = new List<T>(GetAll());
            EditorUtility.SetDirty(list);
        }

        DrawDefaultInspector();
    }

    T[] GetAll()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return a;
    }
}
