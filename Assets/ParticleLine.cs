using UnityEngine;
using System.Collections;

public class ParticleLine : MonoBehaviour {
	
	public AudioSource audio;
	public CamScript camScript;
	
	private ParticleSystem.Particle[] points;
	private float size = 3;
	private float currentSize = 3;
	private int resolution = 8;
	private int currentResolution;
	private int xResScale = 256;
	private float[] samples;
	private float maxHeight = 0;
	private int offset;
	private bool showGui = true;
	
	private readonly float defaultSize = 3;
	private readonly int defaultResolution = 8;
	
	void Start()
	{
		samples = new float[xResScale];
		offset = (int)(256 * 0.1);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0) && !showGui)
		{
			showGui = true;
			camScript.ShowGui = true;
		}
		
		if (currentResolution != resolution)
		{
			currentResolution = resolution;
			CreatePoints();	
		}
		if (currentSize != size)
		{
			currentSize = size;
			CreatePoints();
		}
		
		audio.GetSpectrumData(this.samples,0,FFTWindow.BlackmanHarris);
		
		for (int i = 0; i < points.Length; i++)
		{
			Vector3 p = points[i].position;
			p.y = CalculateValue(p);
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y / maxHeight / audio.volume;			
			points[i].color = c;
		}

		particleSystem.SetParticles(points, points.Length);
	}
	
	void CreatePoints()
	{
		points = new ParticleSystem.Particle[(xResScale * resolution) * resolution];

		int i = 0;
		for (int x = 0; x < xResScale; x++)
		{
			for (int z = 0; z < resolution; z++)
			{
				points[i].position = new Vector3(x, 0f, z);
				points[i].color = new Color((float)x / xResScale, 0, (float)z / resolution);
				points[i++].size = size;
			}
		}	
	}
	
	void OnGUI()
	{
		if (showGui)
		{
			audio.volume = GUI.HorizontalSlider(new Rect(10, 10, 200, 30), audio.volume, 0, 1);
			GUI.Label(new Rect(20, 20, 200, 30), "Volume");
			resolution = (int)GUI.HorizontalSlider(new Rect(10, 50, 200, 30), resolution, 1, 16);
			GUI.Label(new Rect(20, 60, 200, 30), "Depth");
			size = GUI.HorizontalSlider(new Rect(10, 90, 200, 30), size, 1, 16);
			GUI.Label(new Rect(20, 100, 200, 30), "Weight");
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
	
	float CalculateValue(Vector3 p)
	{
		int i = (int)p.x;
		int index = i-offset;
		if (index < 0)
		{
			return samples[i]*audio.volume * (50 + i * i) / 6;
		}
		float y = samples[index]*audio.volume * (50 + i * i) / 3.5f;
		if (y > maxHeight)
		{
			maxHeight = y;
		}
			
		return y;
	}
}
