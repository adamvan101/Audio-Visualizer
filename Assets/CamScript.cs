using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour {
	
	public bool ShowGui = true;
	public Vector3[] Positions;
	
	private GameObject[] visualizers;
	private int _visualizerIndex;
	
	void Start()
	{
		visualizers = GameObject.FindGameObjectsWithTag("Visualizer");
		foreach (GameObject go in visualizers)
		{
			go.SetActive(false);
		}
		
		transform.position = Positions[_visualizerIndex];
		visualizers[0].SetActive(true);
	}
	
	void OnGUI()
	{
		if (ShowGui)
		{
			GUILayout.BeginArea(new Rect(400, 10, 1000, 40));
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Restart", GUILayout.Width(60)))
			{
				audio.Play();
				audio.time = 0;
			}
			GUILayout.Space(10);
			if (GUILayout.Button("Pause", GUILayout.Width(60)))
			{
				audio.Pause();
			}	
			GUILayout.Space(10);
			if (GUILayout.Button("Play", GUILayout.Width(60)))
			{
				if (!audio.isPlaying)
				{
					audio.Play();
				}
			}	
			GUILayout.Space(10);
			
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}	
	
	public void ToggleDisplay()
	{
		visualizers[_visualizerIndex].SetActive(false);
		_visualizerIndex++;
		if (_visualizerIndex >= visualizers.Length)
		{
			_visualizerIndex = 0;
		}
		
		visualizers[_visualizerIndex].SetActive(true);
		transform.position = Positions[_visualizerIndex];
	}
}
