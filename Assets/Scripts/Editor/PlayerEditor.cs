﻿namespace Assets.Scripts.Editor
{
    using Assets.Scripts.Bullets;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(Player))]
    public class PlayerEditor : Editor
    {
        private bool _camera;

        #region methods

        public override void OnInspectorGUI()
        {
            var player = (Player)target;
            serializedObject.Update();

            // PlayerId
            player.Id = (PlayerId)EditorGUILayout.EnumPopup(new GUIContent("Id"), player.Id);

            player.Points = EditorGUILayout.IntField(new GUIContent("Points"), player.Points);
            player.BulletsFab = (BulletsProps)EditorGUILayout.ObjectField(new GUIContent("Bullet"), player.BulletsFab, typeof(BulletsProps), true);
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
                player.CameraRotation = EditorGUILayout.Slider(new GUIContent("Angle"), player.CameraRotation, player.MinCameraRotation, player.MaxCameraRotation);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                {
                    player.MinCameraRotation = EditorGUILayout.FloatField(new GUIContent("Minimum"), player.MinCameraRotation);
                    player.MaxCameraRotation = EditorGUILayout.FloatField(new GUIContent("Maximum"), player.MaxCameraRotation);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
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
                player.JumpSpeed = EditorGUILayout.FloatField(new GUIContent("Jumping"), player.JumpSpeed);
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
                player.GrabLock = EditorGUILayout.FloatField(new GUIContent("Grab Lock"), player.GrabLock);
                player.GrabOffset = EditorGUILayout.Vector3Field(new GUIContent("Grab Offset"), player.GrabOffset);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            // Attack
            EditorGUILayout.PrefixLabel(new GUIContent("Attack:"));
            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            {
                player.PushDistance = EditorGUILayout.FloatField(new GUIContent("Push Distance"), player.PushDistance);
                player.MaxBullets = EditorGUILayout.IntField(new GUIContent("Max Bullets"), player.MaxBullets);
                player.BulletCoolTime = EditorGUILayout.FloatField(new GUIContent("Bullet Cool Time"), player.BulletCoolTime);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}