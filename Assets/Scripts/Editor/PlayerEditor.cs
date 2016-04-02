namespace Assets.Scripts.Editor
{
    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(Player))]
    public class PlayerEditor : Editor
    {
        #region methods

        public override void OnInspectorGUI()
        {
            var player = (Player)target;
            serializedObject.Update();

            // PlayerId
            player.Id = (PlayerId)EditorGUILayout.EnumPopup(new GUIContent("Id"), player.Id);
            player.Points = EditorGUILayout.IntField(new GUIContent("Points"), player.Points);
            EditorGUILayout.Space();

            // Camera
            EditorGUILayout.PrefixLabel(new GUIContent("Camera:"));
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            {
                // Camera Distance
                player.CameraDistance = EditorGUILayout.Slider(new GUIContent("Distance"), player.CameraDistance, player.MinCameraDistance, player.MaxCameraDistance);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                {
                    player.MinCameraDistance = EditorGUILayout.FloatField(new GUIContent("Minimum"), player.MinCameraDistance);
                    player.MaxCameraDistance = EditorGUILayout.FloatField(new GUIContent("Maximum"), player.MaxCameraDistance);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                // Camera Angle
                player.CameraRotation = EditorGUILayout.FloatField(new GUIContent("Angle"), player.CameraRotation);
                EditorGUILayout.Space();

                // Camera Speed
                EditorGUILayout.PrefixLabel(new GUIContent("Speed:"));
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                {
                    player.CameraDistanceSpeed = EditorGUILayout.FloatField(new GUIContent("Distance"), player.CameraDistanceSpeed);
                    player.CameraSpeed = EditorGUILayout.FloatField(new GUIContent("Angle"), player.CameraSpeed);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();

                // Camera Angle
                player.LookAtOffset = EditorGUILayout.Vector3Field(new GUIContent("Offset"), player.LookAtOffset);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Moving
            EditorGUILayout.PrefixLabel(new GUIContent("Moving:"));
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            {
                player.InputType = (InputType)EditorGUILayout.EnumPopup(new GUIContent("Input"), player.InputType);
                player.Speed = EditorGUILayout.FloatField(new GUIContent("Speed"), player.Speed);
                player.RotationSpeed = EditorGUILayout.FloatField(new GUIContent("Rotation Speed"), player.RotationSpeed);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Pearls
            EditorGUILayout.PrefixLabel(new GUIContent("Pearls:"));
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            {
                player.DropSpeed = EditorGUILayout.FloatField(new GUIContent("Drop Speed"), player.DropSpeed);
                player.GrabOffset = EditorGUILayout.Vector3Field(new GUIContent("Grab Offset"), player.GrabOffset);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}