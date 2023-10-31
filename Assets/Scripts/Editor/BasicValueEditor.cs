using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SerializedArray
{

    /// <summary>
    /// Int型を表示するウィンドウ
    /// </summary>
    internal sealed class IntPlaneWindow : PlaneWindowBase<int>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<IntPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override int DrawValue(int value)
        {
            return EditorGUILayout.IntField(value, GUILayout.Width(45));
        }
    }

    /// <summary>
    /// Float型を表示するウィンドウ
    /// </summary>
    internal sealed class FloatPlaneWindow : PlaneWindowBase<float>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<FloatPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override float DrawValue(float value)
        {
            return EditorGUILayout.FloatField(value, GUILayout.Width(45));
        }
    }

    /// <summary>
    /// String型を表示するウィンドウ
    /// </summary>
    internal sealed class StringPlaneWindow : PlaneWindowBase<string>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<StringPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override string DrawValue(string value)
        {
            return EditorGUILayout.TextField(value, GUILayout.Width(60));
        }
    }

    /// <summary>
    /// Object型を表示するウィンドウ
    /// </summary>
    internal sealed class ObjectPlaneWindow : PlaneWindowBase<Object>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<ObjectPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override Object DrawValue(Object value)
        {
            return EditorGUILayout.ObjectField(value, typeof(Object), true, GUILayout.Width(100));
        }
    }

    /// <summary>
    /// GameObject型を表示するウィンドウ
    /// </summary>
    internal sealed class GameObjectPlaneWindow : PlaneWindowBase<GameObject>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<GameObjectPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override GameObject DrawValue(GameObject value)
        {
            return EditorGUILayout.ObjectField(value, typeof(GameObject), true, GUILayout.Width(100)) as GameObject;
        }
    }

    /// <summary>
    /// Color型を表示するウィンドウ
    /// </summary>
    internal sealed class ColorPlaneWindow : PlaneWindowBase<Color>
    {
        public static void ShowWindow(FieldInfo field, Object obj, SerializedObject serialized, string label)
        {
            var window = GetWindow<ColorPlaneWindow>(label);
            window.Init(window, field, obj, serialized);
        }

        protected override Color DrawValue(Color value)
        {
            return EditorGUILayout.ColorField(value, GUILayout.Width(45));
        }
    }
}
