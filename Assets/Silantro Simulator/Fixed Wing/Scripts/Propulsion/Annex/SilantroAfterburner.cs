using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilantroAfterburner : MonoBehaviour {

	public SilantroNozzleActuator dataSource;
	public SilantroTurboFan connectedEngine;

	float targetXScale, currentXScale,baseXScale,targetYScale, currentYScale,baseYScale;
	float targetZScale,currentZScale,baseZScale;
	float actualXScale, actualYScale, actualZScale;

	//STATE 1
	public float state1XScale = 1273.28f, state1YScale = 1273.28f, state1ZScale = 1273.28f, state1Alpha = 0.012f;
	//STATE 2
	public float state2XScale = 1510.57f, state2YScale = 1510.57f, state2ZScale = 1734,state2Alpha = 0.447f;


	public GameObject flameObject;
	public Material flameMaterial;
	public Color baseColor,flameColor;

	public float currentValue;
	[Range(0, 0.5f)] public float flameAlpha, targetAlpha, alphaSpeed = 0.1f;public float scaleSpeed = 1f;
	
	void Start () {
		baseXScale = flameObject.transform.localScale.x;
		baseYScale = flameObject.transform.localScale.y;
		baseZScale = flameObject.transform.localScale.z;
		//COLLECT COLOR
		baseColor = flameMaterial.GetColor("_TintColor");
	}
	
	void Update () {

		if(dataSource != null)
		{
			currentValue = dataSource.currentActuationLevel * connectedEngine.coreFactor;
		}
		else { currentValue = connectedEngine.coreFactor; }
		

		if(connectedEngine != null)
		{
			if (connectedEngine.active)
			{
				if (connectedEngine.afterburnerOperative == true) { targetAlpha = state2Alpha* connectedEngine.coreFactor; targetXScale = state2XScale; targetYScale = state2YScale;targetZScale = state2ZScale; }//STATE 2
				if(connectedEngine.afterburnerOperative == false) { targetAlpha = state1Alpha*connectedEngine.coreFactor; targetXScale = state1XScale; targetYScale = state1YScale; targetZScale = state1ZScale; }//STATE 1
			}
			else
			{
				targetAlpha = 0f;
				targetXScale = baseXScale;targetYScale = baseYScale;targetZScale = baseZScale;
			}
				
		}

		//COLOR
		flameAlpha = Mathf.MoveTowards(flameAlpha, targetAlpha, Time.deltaTime * alphaSpeed);
		flameColor = new Color(baseColor.r, baseColor.g, baseColor.b, flameAlpha);
		//SCALE 
		currentXScale = baseXScale + ((targetXScale - baseXScale) * connectedEngine.coreFactor);
		currentYScale = baseYScale + ((targetYScale - baseYScale) * connectedEngine.coreFactor);
		currentZScale = baseZScale + ((targetZScale - baseZScale) * connectedEngine.coreFactor);

		actualXScale = Mathf.MoveTowards(actualXScale, currentXScale, Time.deltaTime * scaleSpeed);
		actualYScale = Mathf.MoveTowards(actualYScale, currentYScale, Time.deltaTime * scaleSpeed);
		actualZScale = Mathf.MoveTowards(actualZScale, currentZScale, Time.deltaTime * scaleSpeed);


		//SET
		flameObject.transform.localScale = new Vector3(actualXScale, actualYScale, actualZScale);
		flameMaterial.SetColor("_TintColor", flameColor);
	}
}
