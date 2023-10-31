using System;
using UnityEngine;

namespace SerializedArray
{
    [Serializable]
    public sealed class SerializedPlane<T> : ISerializationCallbackReceiver
    {
        /// <summary>
        /// 配列の値
        /// </summary>
        public T[,] Value;

        /// <summary>
        /// 配列の横(X軸)
        /// </summary>
        public int Width => m_width;

        /// <summary>
        /// 配列の高さ(Y軸)
        /// </summary>
        public int Height => m_height;

        //値保存用
        [SerializeField]
        [HideInInspector]
        private T[] Serialize;

        //配列のサイズ
        [HideInInspector]
        [SerializeField]
        private int m_width, m_height;


        public void OnAfterDeserialize()
        {
            //Valueがnullかサイズが違う場合、リサイズする
            if (Value != null)
            {
                var w = Value.GetLength(0);
                var h = Value.GetLength(1);

                if (m_width != w || m_height != h)
                {
                    Value = new T[m_width, m_height];
                }
            }
            else
            {
                Value = new T[m_width, m_height];
            }

            //要素をコピー
            for (int x = 0; x < m_width; x++)
                for (int y = 0; y < m_height; y++)
                {
                    Value[x, y] = Serialize[x + m_width * y];
                }
            Serialize = null;

        }

        public void OnBeforeSerialize()
        {
            if (Value == null)
                return;


            var w = Value.GetLength(0);
            var h = Value.GetLength(1);

            //要素数が違えばリサイズする
            if (Serialize == null || w * h != m_width * m_height)
            {
                Serialize = new T[w * h];
            }

            m_width = w;
            m_height = h;

            //要素をコピー
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    Serialize[x + w * y] = Value[x, y];
                }

        }
    }

}

/*Docs
 *https://docs.unity3d.com/ja/2021.2/Manual/script-Serialization-Custom.html
 */