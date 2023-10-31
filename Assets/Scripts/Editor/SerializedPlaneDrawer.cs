using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SerializedArray
{
    [CustomPropertyDrawer(typeof(SerializedPlane<>))]
    public class SerializedPlaneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);
            // 子のフィールドをインデントしない 
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // 矩形を計算
            var labelRect = new Rect(position.x, position.y, position.width * 0.4f, position.height);
            var sizeRect = new Rect(position.x + position.width * 0.4f, position.y, position.width * 0.4f, position.height);
            var buttonRect = new Rect(position.x + position.width * 0.8f + 10, position.y, position.width * 0.2f - 10, position.height);

            var sizeVec = new Vector2Int(property.FindPropertyRelative("m_width").intValue,
                                         property.FindPropertyRelative("m_height").intValue);

            //ラベル表示
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), label);

            //サイズ表示
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.Vector2IntField(sizeRect, GUIContent.none, sizeVec);
            EditorGUI.EndDisabledGroup();

            //編集ウィンドウを表示する
            if (GUI.Button(buttonRect, "Edit"))
            {
                var obj = property.serializedObject.targetObject;
                var fields = obj.GetType().GetFields();

                //BoolPlaneを見つける
                foreach (var field in fields)
                {
                    var val = field.GetValue(obj);

                    if (val != null &&
                        field.Name.ToLower() == label.text.Replace(" ", "").ToLower())
                    {

                        var type = val.GetType();
                        /***型によってウィンドウを選択する***/
                        if (type == typeof(SerializedPlane<int>))
                            IntPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);
                        else if (type == typeof(SerializedPlane<float>))
                            FloatPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);
                        else if (type == typeof(SerializedPlane<string>))
                            StringPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);
                        else if (type == typeof(SerializedPlane<Color>))
                            ColorPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);
                        else if (type == typeof(SerializedPlane<GameObject>))
                            GameObjectPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);
                        else if (type == typeof(SerializedPlane<Object>))
                            ObjectPlaneWindow.ShowWindow(field, obj, property.serializedObject, label.text);

                        /***型によってウィンドウを選択する***/
                    }
                }
            }
            // インデントを元通りに戻す
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    internal abstract class PlaneWindowBase<T> : EditorWindow
    {
        //値の編集・更新
        SerializedObject serialized;
        UnityEngine.Object obj;
        SerializedPlane<T> box;


        //編集対象
        T[,] value;


        //EditorGUI用変数
        Vector2 scroll;
        Vector2Int planeSize;
        Color line = new Color(0.8f, 0.8f, 0.8f);



        /// <summary>
        /// 継承先でウィンドウを取得した後に必ず呼び出す
        /// </summary>
        /// <param name="window"></param>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <param name="serialized"></param>
        protected void Init(PlaneWindowBase<T> window, FieldInfo field, Object obj, SerializedObject serialized)
        {
            //値の設定
            window.serialized = serialized;
            window.obj = obj;
            var box = field.GetValue(obj) as SerializedPlane<T>;
            window.box = box;
            window.value = box.Value;
            window.planeSize = new Vector2Int(box.Width, box.Height);

            //画面の中心に表示する
            var res = Screen.currentResolution;
            var rect = new Rect(res.width / 2 - 150, res.height / 2 - 150, 300, 300);
            window.position = rect;
            window.ShowAsDropDown(rect, new Vector2(300, 300));
        }

        /// <summary>
        /// 値をエディターに表示する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract T DrawValue(T value);



        private void OnGUI()
        {
            if (serialized == null)
            {
                Close();
                return;
            }

            //配列のサイズを設定する
            EditorGUILayout.LabelField("Set array size:");
            EditorGUILayout.BeginHorizontal();
            planeSize = EditorGUILayout.Vector2IntField(GUIContent.none, planeSize);
            if (GUILayout.Button("Set"))
            {
                //配列のサイズ変更
                ResizeArray();

                //変更した配列をBoolPlaneに適用
                serialized.Update();
                box.Value = value;
                serialized.ApplyModifiedPropertiesWithoutUndo();

                //保存処理
                EditorUtility.SetDirty(obj);
                AssetDatabase.SaveAssets();
            }
            EditorGUILayout.EndHorizontal();


            //値を表示するエリア
            if (value == null)
                return;

            scroll = EditorGUILayout.BeginScrollView(scroll);

            for (int y = 0; y < value.GetLength(1); y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < value.GetLength(0); x++)
                {
                    //5マスごとに色を変更する
                    if (x % 5 == 4 || y % 5 == 4)
                    {
                        GUI.color = line;
                        value[x, y] = DrawValue(value[x, y]);
                        GUI.color = Color.white;
                    }
                    else
                    {
                        value[x, y] = DrawValue(value[x, y]);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }

        private void OnDestroy()
        {
            //ウィンドウを閉じるときに値を保存する
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 配列をリサイズする
        /// </summary>
        private void ResizeArray()
        {
            var newValue = new T[planeSize.x, planeSize.y];

            if (value == null)
            {
                value = newValue;
                return;
            }
            for (int x = 0; x < planeSize.x && x < value.GetLength(0); x++)
                for (int y = 0; y < planeSize.y && y < value.GetLength(1); y++)
                {
                    newValue[x, y] = value[x, y];
                }
            value = newValue;
        }
    }

}

/*Docs
 *https://kazupon.org/unity-no-edit-param-view-inspector/
 *https://docs.unity3d.com/ja/2021.3/Manual/editor-PropertyDrawers.html
 *https://docs.unity3d.com/ja/2021.1/ScriptReference/EditorGUILayout.html
 *https://docs.unity3d.com/ja/current/ScriptReference/GUI.html
 */