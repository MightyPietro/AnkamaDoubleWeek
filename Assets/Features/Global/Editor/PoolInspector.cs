using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WeekAnkama
{
    [CustomEditor(typeof(TileEffectPool))]
    public class TileEffectPoolInspector : PoolInspector<TileEffectDisplay> { }

    [CustomEditor(typeof(Pool<>))]
    public class PoolInspector<T> : Editor where T : MonoBehaviour
    {
        private Pool<T> pool;
        private int sliderValue;
        private int nbMaxRectOnLine;
        private T tempPrefab;
        private bool hasChanged = false;

        private void OnEnable()
        {
            hasChanged = false;
            pool = target as Pool<T>;
            if (!pool.IsInitialized)
            {
                pool.Init(pool.transform);
                EditorUtility.SetDirty(pool);
            }
        }

        public override void OnInspectorGUI()
        {
            GUIStyle TitleStyle = GUI.skin.GetStyle("Label");
            TitleStyle.alignment = TextAnchor.MiddleCenter;
            TitleStyle.fontSize = 14;
            TitleStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Pool", TitleStyle);

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            tempPrefab = EditorGUILayout.ObjectField(new GUIContent("Object prefab", "The object that will fill the pool"), pool.Prefab, typeof(T), false) as T;
            if (EditorGUI.EndChangeCheck())
            {
                hasChanged = true;
                Undo.RecordObject(pool, "Change prefab");
                pool.Prefab = tempPrefab;
                pool.Reset();
            }
            sliderValue = EditorGUILayout.IntSlider(sliderValue, 0, 100);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Generate", "Generate X(slider value) gameobjects from the given prefab")))
            {
                hasChanged = true;
                Undo.RecordObject(pool, "Generate");
                pool.Generate(sliderValue);
            }
            if (GUILayout.Button(new GUIContent("Remove", "Remove X(slider value) gameobjects")))
            {
                hasChanged = true;
                Undo.RecordObject(pool, "Remove");
                pool.Remove(sliderValue);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.LabelField($"{pool.ActualNumberOfObject} Objects");
            GUILayout.BeginHorizontal();
            nbMaxRectOnLine = 10;
            for (int index = 0; index < pool.ActualNumberOfObject; index++)
            {
                if (index < pool.GetPoolCount && pool.GetAt(index) == null)
                {
                    Debug.LogError("Object's pool manually destroyed -- Reset of the pool");
                    pool.Reset();
                    return;
                }
                if (index != 0 && index % nbMaxRectOnLine == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(30));
                //if (!pool.GetAt(index).gameObject.activeInHierarchy)
                if (index < pool.GetPoolCount)
                {
                    EditorGUI.DrawRect(rect, Color.green);
                }
                //else if(pool.GetAt(index).gameObject.activeInHierarchy)
                else
                {
                    EditorGUI.DrawRect(rect, Color.red);
                }
            }
            GUILayout.EndHorizontal();

            if (hasChanged)
            {
                EditorUtility.SetDirty(pool);
            }
            hasChanged = false;
        }

    }
}
