#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public partial class LevelConfig
    {
        protected static BoxData DrawCell(Rect rect, BoxData value)
        {
            Color background = value.Color switch
            {
                BoxColor.Red => Color.red,
                BoxColor.Green => Color.green,
                BoxColor.Blue => Color.blue,
                BoxColor.Yellow => Color.yellow,
                BoxColor.Purple => new Color(0.7f, 0.2f, 1f),
                _ => new Color(0.25f, 0.25f, 0.25f)
            };

            EditorGUI.DrawRect(rect, background);

            Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.black);

            if (value.StackHeight > 0)
            {
                GUIStyle style = new(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter
                };

                style.normal.textColor = Color.white;

                EditorGUI.LabelField(rect, value.StackHeight.ToString(), style);
            }

            Event e = Event.current;

            if (rect.Contains(e.mousePosition) && e.type == EventType.MouseDown)
            {
                // ЛКМ - следующий цвет
                if (e.button == 0)
                {
                    if (value.Color == BoxColor.None)
                    {
                        value.Color = BoxColor.Red;
                        value.StackHeight = 1;
                    }
                    else
                    {
                        int colorCount = Enum.GetValues(typeof(BoxColor)).Length - 1; // без None

                        int colorIndex = (int)value.Color;
                        colorIndex++;

                        if (colorIndex > colorCount)
                            colorIndex = 1;

                        value.Color = (BoxColor)colorIndex;
                    }

                    GUI.changed = true;
                    e.Use();
                }

                // ПКМ - изменить высоту
                if (e.button == 1)
                {
                    if (value.Color != BoxColor.None)
                    {
                        if (e.shift)
                        {
                            value.StackHeight = Mathf.Max(1, value.StackHeight - 1);
                        }
                        else
                        {
                            value.StackHeight++;
                        }

                        GUI.changed = true;
                    }

                    e.Use();
                }
            }

            return value;
        }
    }
}

#endif