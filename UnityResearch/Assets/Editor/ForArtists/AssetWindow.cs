using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetWindow : EditorWindow
{
   
   [MenuItem("Tools/AssetWindow")]
   private static void ShowWindow()
   {
      EditorWindow window = GetWindow<AssetWindow>();
      window.titleContent = new GUIContent("Panel资源管理");
      window.Show();
   }

   private void OnGUI()
   {
      
   }
}
