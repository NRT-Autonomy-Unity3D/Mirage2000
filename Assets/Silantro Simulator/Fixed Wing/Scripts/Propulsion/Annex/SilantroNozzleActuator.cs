using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroNozzleActuator : MonoBehaviour {

	//VARIABLES
	[HideInInspector]public float currentActuationLevel;
	float targetActuationPoint;
	[HideInInspector]public float actuationSpeed = 0.2f;
	//CONNECTIONS
	[HideInInspector]public SilantroTurboFan connectedFanEngine;
	[HideInInspector]public SilantroTurboJet connectedJetEngine;

	public enum EngineType{TurboFan,TurboJet}
	[HideInInspector]public EngineType engineType = EngineType.TurboFan;
	[HideInInspector]public Animator nozzleAnimator;

	[HideInInspector]public string animationName =  "Engine Nozzle";
	[HideInInspector]public int animationLayer = 0;
	[HideInInspector]public bool inverseActuation;


	void Start(){if(nozzleAnimator != null){nozzleAnimator.speed = 0f;}}
	[HideInInspector]public float actualActuation;
	
	// Update is called once per frame
	void Update () {
		if(engineType == EngineType.TurboFan)
		{
			if(connectedFanEngine != null)
			{
				if(connectedFanEngine.afterburnerOperative){ targetActuationPoint = connectedFanEngine.coreFactor; ;}
				else{targetActuationPoint = connectedFanEngine.coreFactor* connectedFanEngine.FuelInput/2f;}
				if(!connectedFanEngine.active){targetActuationPoint = 0f;}
			}
		}
		if(engineType == EngineType.TurboJet)
		{
			if(connectedJetEngine != null)
			{
				if(connectedJetEngine.afterburnerOperative){targetActuationPoint = 1f;}
				else{targetActuationPoint = connectedJetEngine.coreFactor*connectedJetEngine.FuelInput/2f;}
				if(!connectedJetEngine.active){targetActuationPoint = 0f;}
			}
		}

		//MOVE
		currentActuationLevel = Mathf.MoveTowards(currentActuationLevel,targetActuationPoint,Time.deltaTime*actuationSpeed);
		if(!inverseActuation){actualActuation = currentActuationLevel;}else{actualActuation = 1-currentActuationLevel; }
		//ANIMATE
		if(nozzleAnimator != null){nozzleAnimator.Play(animationName,animationLayer,actualActuation);}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SilantroNozzleActuator))]
public class NozzleActuatorEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = new Color(1,0.4f,0);
	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		//
		DrawDefaultInspector ();EditorGUI.BeginChangeCheck();
		serializedObject.Update ();
		SilantroNozzleActuator actuator = (SilantroNozzleActuator)target;
	
		GUILayout.Space(2f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Lever Type", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.engineType = (SilantroNozzleActuator.EngineType)EditorGUILayout.EnumPopup("Engine Type",actuator.engineType);

		if(actuator.engineType == SilantroNozzleActuator.EngineType.TurboFan)
		{
			GUILayout.Space (5f);
			actuator.connectedFanEngine = EditorGUILayout.ObjectField ("Connected Turbofan", actuator.connectedFanEngine, typeof(SilantroTurboFan), true) as SilantroTurboFan;
		}
		if(actuator.engineType == SilantroNozzleActuator.EngineType.TurboJet)
		{
			GUILayout.Space (5f);
			actuator.connectedJetEngine = EditorGUILayout.ObjectField ("Connected Turbojet", actuator.connectedJetEngine, typeof(SilantroTurboJet), true) as SilantroTurboJet;
		}

		GUILayout.Space (10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Animation Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.nozzleAnimator = EditorGUILayout.ObjectField ("Nozzle Animator", actuator.nozzleAnimator, typeof(Animator), true) as Animator;
		GUILayout.Space (3f);
		actuator.animationLayer = EditorGUILayout.IntField("Animation Layer",actuator.animationLayer);
		GUILayout.Space (3f);
		actuator.animationName = EditorGUILayout.TextField("Animation Name",actuator.animationName);

		GUILayout.Space (10f);
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Actuation Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.actuationSpeed = EditorGUILayout.Slider("Actuation Speed",actuator.actuationSpeed,0,1);
		GUILayout.Space (3f);
		EditorGUILayout.LabelField("Actuation level",(actuator.currentActuationLevel *100f).ToString("0.00") + " %");

		if (GUI.changed) {
			EditorUtility.SetDirty (actuator);
			EditorSceneManager.MarkSceneDirty (actuator.gameObject.scene);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif