#if UNITY_EDITOR

using System;
using Entities;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Managers
{
    public partial class TanksGridConfig
    {
        private bool _statisticsFoldout = true;

        private static GUIStyle _statisticsStyle;

        private static GUIStyle StatisticsStyle
        {
            get
            {
                if (_statisticsStyle == null)
                {
                    _statisticsStyle = new GUIStyle(EditorStyles.boldLabel)
                    {
                        fontSize = Mathf.RoundToInt(EditorStyles.boldLabel.fontSize * 1.25f)
                    };
                }

                return _statisticsStyle;
            }
        }

        protected static TankData DrawGrid(Rect rect, TankData value)
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

            if (value.ShootsCount > 0)
            {
                GUIStyle style = new(EditorStyles.boldLabel)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = EditorStyles.boldLabel.fontSize * 2
                };

                style.normal.textColor = Color.black;

                EditorGUI.LabelField(rect, value.ShootsCount.ToString(), style);
            }

            Event e = Event.current;

            if (rect.Contains(e.mousePosition) && e.type == EventType.MouseDown)
            {
                if (e.button == 0)
                {
                    if (value.Color == BoxColor.None)
                    {
                        value.Color = BoxColor.Red;
                        value.ShootsCount = 1;
                    }
                    else
                    {
                        int colorCount = Enum.GetValues(typeof(BoxColor)).Length - 1;

                        int colorIndex = (int)value.Color;
                        colorIndex++;

                        if (colorIndex > colorCount)
                            colorIndex = 1;

                        value.Color = (BoxColor)colorIndex;
                    }

                    GUI.changed = true;
                    e.Use();
                }
                
                if (e.button == 1)
                {
                    if (value.Color != BoxColor.None)
                    {
                        if (e.shift)
                        {
                            value.ShootsCount = Mathf.Max(1, value.ShootsCount - 1);
                        }
                        else
                        {
                            value.ShootsCount++;
                        }

                        GUI.changed = true;
                    }

                    e.Use();
                }
            }

            return value;
        }

        [PropertyOrder(1000)]
        [OnInspectorGUI]
        private void DrawStatistics()
        {
            _statisticsFoldout = EditorGUILayout.Foldout(
                _statisticsFoldout,
                "Shoots Count",
                true);

            if (!_statisticsFoldout)
                return;

            int[] counts = new int[Enum.GetValues(typeof(BoxColor)).Length];

            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    TankData data = Grid[x, y];

                    if (data.Color == BoxColor.None)
                        continue;

                    counts[(int)data.Color] += Mathf.Max(1, data.ShootsCount);
                }
            }

            GUILayout.BeginVertical(EditorStyles.helpBox);

            DrawColorCount(BoxColor.Red, counts);
            DrawColorCount(BoxColor.Green, counts);
            DrawColorCount(BoxColor.Blue, counts);
            DrawColorCount(BoxColor.Yellow, counts);
            DrawColorCount(BoxColor.Purple, counts);

            GUILayout.EndVertical();
        }

        private static void DrawColorCount(BoxColor color, int[] counts)
        {
            Color old = GUI.color;

            GUI.color = color switch
            {
                BoxColor.Red => Color.red,
                BoxColor.Green => Color.green,
                BoxColor.Blue => Color.blue,
                BoxColor.Yellow => Color.yellow,
                BoxColor.Purple => new Color(0.7f, 0.2f, 1f),
                _ => Color.white
            };

            GUILayout.BeginHorizontal();

            GUILayout.Label(color.ToString(), StatisticsStyle, GUILayout.Width(180));
            GUILayout.Label(counts[(int)color].ToString(), StatisticsStyle);

            GUILayout.EndHorizontal();

            GUI.color = old;
        }
    }
}

#endif