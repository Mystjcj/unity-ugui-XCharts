/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    internal static class SerieLabelPool
    {
        private static readonly Stack<GameObject> m_Stack = new Stack<GameObject>(200);

        public static GameObject Get(string name, Transform parent, SerieLabel label, Font font, Color color, SerieData serieData)
        {
            GameObject element;
            if (m_Stack.Count == 0)
            {
                element = ChartHelper.AddSerieLabel(name, parent, font,
                        color, label.backgroundColor, label.fontSize, label.fontStyle, label.rotate,
                        label.backgroundWidth, label.backgroundHeight);
                ChartHelper.AddIcon("Icon", element.transform, serieData.iconWidth, serieData.iconHeight);
            }
            else
            {
                element = m_Stack.Pop();
                element.name = name;
                element.transform.SetParent(parent);
                ChartHelper.SetActive(element, true);
            }
            return element;
        }

        public static void Release(GameObject element)
        {
            ChartHelper.SetActive(element, false);
            //if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            //    Debug.LogError("Internal error. Trying to destroy object that is already released to pool." + element.name);
            m_Stack.Push(element);
        }

        public static void ReleaseAll(Transform parent)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Release(parent.GetChild(i).gameObject);
            }
        }

        public static void ClearAll()
        {
            m_Stack.Clear();
        }
    }
}
