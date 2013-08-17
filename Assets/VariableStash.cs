using UnityEngine;
using System.Collections;

public class VariableStash : MonoBehaviour {
	
	public CamScript camScript;
	public AudioSource audio;

	[HideInInspector]
	public float size = 3;
	[HideInInspector]
	public int resolution = 8;
	[HideInInspector]
	public bool showGui = true;
		
	private readonly float defaultSize = 3;
	private readonly int defaultResolution = 8;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !showGui)
		{
			showGui = true;
		}	
	}
	
	void OnGUI()
	{
		if (showGui)
		{
			audio.volume = GUI.HorizontalSlider(new Rect(10, 10, 200, 30), audio.volume, 0, 1);
			GUI.Label(new Rect(20, 20, 200, 30), "Volume");
			resolution = (int)GUI.HorizontalSlider(new Rect(10, 50, 200, 30), resolution, 1, 32);
			GUI.Label(new Rect(20, 60, 200, 30), "Radius");
			size = GUI.HorizontalSlider(new Rect(10, 90, 200, 30), size, 1, 16);
			GUI.Label(new Rect(20, 100, 200, 30), "Thickness");
			if (GUI.Button(new Rect(10, 140, 200, 30), "Hide Controls"))
			{
				showGui = false;	
				camScript.ShowGui = false;
			}
			if (GUI.Button(new Rect(10, 190, 200, 30), "Toggle Display"))
			{
				camScript.ToggleDisplay();
			}
			if (GUI.Button(new Rect(10, 240, 200, 30), "Reset to Defaults"))
			{
				ResetDefaults();
			}
		}
	}
		
	void ResetDefaults()
	{
		size = defaultSize;
		resolution = defaultResolution;
		audio.volume = 1;
	}
}
