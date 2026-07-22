#if UNITY_EDITOR

using Entities;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace Managers
{
    public partial class BoxesBlockConfig
    {
        private BoxColor _fillColor = BoxColor.Red;
        private int _fillStackHeight = 1;

        private int _fromColumn;
        private int _toColumn;

        private int _fromRow;
        private int _toRow;

        private static readonly string[] ColumnNames = CreateNames(BoxesGridConfig.Width);
        private static readonly string[] RowNames = CreateNames(BoxesGridConfig.Height);

        private bool _showFillTools = true;

        private GUIStyle _titleStyle;

        [PropertyOrder(-100)]
        [OnInspectorGUI]
        private void DrawFillTools()
        {
            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(EditorStyles.foldout)
                {
                    fontSize = Mathf.RoundToInt(EditorStyles.foldout.fontSize * 1.25f),
                    fontStyle = FontStyle.Bold
                };
            }

            GUILayout.BeginVertical(EditorStyles.helpBox);

            _showFillTools = EditorGUILayout.Foldout(_showFillTools, "Fill Tools", true, _titleStyle);

            GUILayout.Space(8);

            if (_showFillTools)
            {
                _fillColor = (BoxColor)EditorGUILayout.EnumPopup("Color", _fillColor);

                _fillStackHeight = Mathf.Max(1,
                    EditorGUILayout.IntField("Height", _fillStackHeight));

                GUILayout.Space(8);

                GUILayout.Label("Columns", EditorStyles.boldLabel);

                _fromColumn = EditorGUILayout.Popup("From", _fromColumn, ColumnNames);
                _toColumn = EditorGUILayout.Popup("To", _toColumn, ColumnNames);

                if (GUILayout.Button("Fill Columns"))
                {
                    FillColumns();
                }

                GUILayout.Space(8);

                GUILayout.Label("Rows", EditorStyles.boldLabel);

                _fromRow = EditorGUILayout.Popup("From", _fromRow, RowNames);
                _toRow = EditorGUILayout.Popup("To", _toRow, RowNames);

                if (GUILayout.Button("Fill Rows"))
                {
                    FillRows();
                }

                GUILayout.Space(10);

                if (GUILayout.Button("Reset All", GUILayout.Height(28)))
                {
                    if (EditorUtility.DisplayDialog(
                            "Reset config",
                            "Clear all grid?",
                            "Yes",
                            "No"))
                    {
                        ResetAll();
                    }
                }

                GUI.backgroundColor = Color.white;

                GUILayout.Space(10);
            }

            GUILayout.EndVertical();
        }

        private void FillColumns()
        {
            int from = Mathf.Min(_fromColumn, _toColumn);
            int to = Mathf.Max(_fromColumn, _toColumn);

            for (int x = from; x <= to; x++)
            {
                for (int y = 0; y < BoxesGridConfig.Height; y++)
                {
                    BoxesGridConfig.Grid[x, y] = new BoxData
                    {
                        Color = _fillColor,
                        StackHeight = _fillStackHeight
                    };
                }
            }

            GUI.changed = true;
        }

        private void FillRows()
        {
            int from = Mathf.Min(_fromRow, _toRow);
            int to = Mathf.Max(_fromRow, _toRow);

            for (int y = from; y <= to; y++)
            {
                for (int x = 0; x < BoxesGridConfig.Width; x++)
                {
                    BoxesGridConfig.Grid[x, y] = new BoxData
                    {
                        Color = _fillColor,
                        StackHeight = _fillStackHeight
                    };
                }
            }

            GUI.changed = true;
        }

        private static string[] CreateNames(int count)
        {
            string[] names = new string[count];

            for (int i = 0; i < count; i++)
            {
                names[i] = i.ToString();
            }

            return names;
        }

        private void ResetAll()
        {
            for (int x = 0; x < BoxesGridConfig.Width; x++)
            {
                for (int y = 0; y < BoxesGridConfig.Height; y++)
                {
                    BoxesGridConfig.Grid[x, y] = new BoxData
                    {
                        Color = BoxColor.None,
                        StackHeight = 0
                    };
                }
            }

            GUI.changed = true;
        }
    }
}

#endif