using UnityEngine;
using System.Collections;

public class AndroidVibrationDemoSceneManager : MonoBehaviour {	
	public long[] Pattern = { 200, 100, 300, 200, 200 };
	
	private int _duration = 1000;
	
	void OnGUI() {		
		GUI.skin.label.fontSize = 45;
		GUI.skin.label.fontStyle = FontStyle.Bold;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height * 0.2f), "Android Vibration Plugin");
		
		GUI.skin.label.fontSize = 40;
		GUI.skin.label.alignment = TextAnchor.LowerCenter;
		Rect layoutRect = new Rect(Screen.width * 0.05f, Screen.height * 0.2f, Screen.width * 0.9f, Screen.height * 0.6f);
		GUILayout.BeginArea (layoutRect, GUI.skin.box);
		
		GUILayout.BeginVertical();
		
		GUILayout.Space(25f);
		
		GUILayout.Label("Has Vibrator: " + VibrationManager.HasVibrator());
		
		GUILayout.FlexibleSpace();
		
		_duration = (int) GUILayout.HorizontalSlider(_duration, 100f, 5000f);
		if(GUILayout.Button("Vibrate " + _duration + " milliseconds")) {
			VibrationManager.Vibrate(_duration);
		}
		
		GUILayout.Space(25f);
		
		if(GUILayout.Button("Vibrate with pattern (no loop)")) {
			VibrationManager.Vibrate(Pattern, 0, 0);
		}
		
		GUILayout.Space(25f);
		
		if(GUILayout.Button("Vibrate with pattern (3 times)")) {
			VibrationManager.Vibrate(Pattern, 0, 2);
		}
		
		GUILayout.Space(25f);
		
		if(GUILayout.Button("Vibrate with pattern (loop)")) {
			VibrationManager.Vibrate(Pattern, 0);
		}
		
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button("Stop all vibrations")) {
			VibrationManager.Cancel();
		}
		 
		GUILayout.EndVertical();
		
		GUILayout.EndArea();
	}
	
}
