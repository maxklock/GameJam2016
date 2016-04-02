namespace Assets.Scripts.Editor
{
    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(Game))]
    public class GameEditor : Editor
    {
        private const int MaxPlayers = 4;
        private bool _autoUpdate = true;

        public override void OnInspectorGUI()
        {
            var hasChanged = false;
            var game = (Game)target;
            serializedObject.Update();

            var tmpCount = EditorGUILayout.IntSlider(new GUIContent("Player Count"), game.PlayerCount, 1, MaxPlayers);
            if (game.PlayerCount != tmpCount)
            {
                game.PlayerCount = tmpCount;
                hasChanged = true;
            }
            if (game.StartPositions.Length != 4)
            {
                game.StartPositions = new Vector3[4];
            }

            var tmpSplit = (Orientation)EditorGUILayout.EnumPopup(new GUIContent("Split Screen"), game.SplitScreen);
            var tmpAllow = EditorGUILayout.Toggle(new GUIContent("Allow Empty ViewPort"), game.AllowEmptyViewPort);
            if (game.SplitScreen != tmpSplit)
            {
                game.SplitScreen = tmpSplit;
                hasChanged = true;
            }
            if (game.AllowEmptyViewPort != tmpAllow)
            {
                game.AllowEmptyViewPort = tmpAllow;
                hasChanged = true;
            }

            for (var i = 0; i < MaxPlayers && i < game.PlayerCount; i++)
            {
                EditorGUILayout.PrefixLabel(new GUIContent("Player " + (i + 1) + ":"));
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                var tmpInput = (InputType)EditorGUILayout.EnumPopup(new GUIContent("Input Type"), game.InputTypes[i]);
                var tmpStart = EditorGUILayout.Vector3Field(new GUIContent("Start Position"), game.StartPositions[i]);

                if (game.InputTypes[i] != tmpInput)
                {
                    game.InputTypes[i] = tmpInput;
                    hasChanged = true;
                }

                if (game.StartPositions[i] != tmpStart)
                {
                    game.StartPositions[i] = tmpStart;
                    hasChanged = true;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            var btnUpdateClicked = GUILayout.Button(new GUIContent("Update"));

            if ((hasChanged && _autoUpdate) || btnUpdateClicked)
            {
                game.InitPlayers();
            }

            _autoUpdate = EditorGUILayout.Toggle(new GUIContent("Auto Update"), _autoUpdate);
        }
    }
}