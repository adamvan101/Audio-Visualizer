using UnityEngine;
using System.Collections;

public class ParticleHourglass : MonoBehaviour {
	
	public bool isTop;
	public bool isLeft;
	public VariableStash vStash;
	public AudioSource audio;	
	
	private float currentSize;
	private int currentResolution;
	private ParticleSystem.Particle[] points;
	private Vector3[] cannedPoints;
	private int xResScale = 256;
	private float[] samples;

	void Start()
	{
		currentSize = vStash.size;
		currentResolution = vStash.resolution;
		samples = new float[currentResolution * 2 + 2];
		CreatePoints();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentResolution != vStash.resolution)
		{
			currentResolution = vStash.resolution;
			samples = new float[currentResolution * 2 + 2];
			CreatePoints();	
		}
		
		if (currentSize != vStash.size)
		{
			currentSize = vStash.size;
			CreatePoints();
		}
		
		samples = audio.GetSpectrumData(currentResolution * 2 + 1, 0, FFTWindow.BlackmanHarris);
		
		for (int i = 0; i < points.Length/2; i++)
		{
			Vector3 p = cannedPoints[i];
			p.y = CalculateValue(p);
			points[i].position = p;
			Color c = points[i].color;
			c.g = p.y;
			points[i].color = c;
			if (p.y != 0)
			{	
				p.y = -p.y;
				points[points.Length - i - 1].position = p; 	
				c.g = p.y;
				points[points.Length - i - 1].color = c;
			}
		}

		particleSystem.SetParticles(points, points.Length);
	}
	
	void CreatePoints()
	{
		points = new ParticleSystem.Particle[((currentResolution + 1) * (currentResolution + 1) * 2) * 2 + currentResolution];
		cannedPoints = new Vector3[(currentResolution + 1) * (currentResolution + 1) * 2 + currentResolution];

		int i = 0;
		for (int r = 1; r <= currentResolution; r++)
		{
			for (float x = -r; x <= r; x+= 0.5f)
			{
				float z = Mathf.Sqrt(Mathf.Abs(r*r - x*x));
				if (isLeft)
				{
					cannedPoints[i] = new Vector3(x, 0, z);
				}
				else
				{
					cannedPoints[i] = new Vector3(-x, 0, -z);	
				}
				points[i].color = new Color((float)x / currentResolution, 0, (float)z / currentResolution);
				points[i++].size = currentSize;
			}
		}
	}
	
	float CalculateValue(Vector3 p)
	{
		int i = (int)(currentResolution + p.x);
		float r = samples[i]*audio.volume * (50 + i * i) / 3.5f;
		float y = Mathf.Sqrt(Mathf.Abs(r*r - p.x*p.x - p.z*p.z));
	
		if (isTop)
		{
			return y;		
		}
		else
		{
			return -y;	
		}
	}
}
