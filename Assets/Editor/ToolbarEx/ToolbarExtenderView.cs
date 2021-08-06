using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
 
namespace UnityToolbarExtender.View
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle ToolBarExtenderBtnStyle;
 
		static ToolbarStyles()
		{
			ToolBarExtenderBtnStyle = new GUIStyle("Command")
			{
				fontSize = 12,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Normal,
				fixedWidth = 60
			};
		}
	}
 
	[InitializeOnLoad]
	public class ToolbarExtenderView
	{
		static ToolbarExtenderView()
		{
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUIRight);
		}
		
		static void OnToolbarGUIRight()
		{
			Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale, 0, 10 , new GUIStyle("MiniSliderHorizontal") , new GUIStyle("MinMaxHorizontalSliderThumb") , GUILayout.MinWidth(200),GUILayout.MinHeight(20) );
			if(GUILayout.Button(Time.timeScale.ToString("0.0"), GUILayout.Width(40))){
				Time.timeScale = 1;
			}
		}
	}
 
	static class SceneHelper
	{
		static string sceneToOpen;
 
		public static void StartScene(string scene)
		{
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}
 
			sceneToOpen = scene;
			EditorApplication.update += OnUpdate;
		}
 
		static void OnUpdate()
		{
			if (sceneToOpen == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}
 
			EditorApplication.update -= OnUpdate;
 
			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(sceneToOpen);
				EditorApplication.isPlaying = true;
			}
			sceneToOpen = null;
		}
	}
}