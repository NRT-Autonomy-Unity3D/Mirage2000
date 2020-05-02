using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class SilantroActuator : MonoBehaviour {

	//MOVEMENT
	[HideInInspector]public float currentActuationLevel,targetActuationLevel,moveSpeed = 0.2f;
	//ANIMATION
	[HideInInspector]public Animator actuatorAnimator;
	[HideInInspector]public string animationName =  "Engine Nozzle";
	[HideInInspector]public int animationLayer = 0;

	//DRAG
	[HideInInspector]public bool generatesDrag;
	[HideInInspector]public float dragFactor = 0.01f,currentDragFactor;
	[HideInInspector]public bool invertMotion;


	void Start(){if(actuatorAnimator != null){actuatorAnimator.speed = 0f;}}
	// ---------------------------------------------------CONTROLS-------------------------------------------------------------------------------------------------------
	//OPEN
	public void EngageActuator()
	{ if (invertMotion) { targetActuationLevel = 1; } else { targetActuationLevel = 0; } }
	//CLOSE
	public void DisengageActuator()
	{ if (invertMotion) { targetActuationLevel = 0; } else { targetActuationLevel = 1; } }


	// ----------------------------------------------------------------------------------------------------------------------------------------------------------
	void Update () {
		//ADJUST CONTROL VARIABLE
		if(currentActuationLevel != targetActuationLevel){currentActuationLevel = Mathf.MoveTowards(currentActuationLevel,targetActuationLevel,Time.deltaTime*moveSpeed);}
		if (invertMotion) { if (generatesDrag) { currentDragFactor = (1-currentActuationLevel) * dragFactor; } }
		else { if (generatesDrag) { currentDragFactor = (currentActuationLevel) * dragFactor; } }
		
	
		//ANIMATE
		if(actuatorAnimator != null){actuatorAnimator.Play(animationName,animationLayer,currentActuationLevel);}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(SilantroActuator))]
public class ActuatorEditor: Editor
{
	Color backgroundColor;
	Color silantroColor = new Color(1,0.4f,0);
	SilantroActuator actuator;SerializedObject actuatorObjet;

	void OnEnable()
	{
		actuator = (SilantroActuator)target;
		actuatorObjet = new SerializedObject (actuator);
	}

	public override void OnInspectorGUI()
	{
		backgroundColor = GUI.backgroundColor;
		DrawDefaultInspector ();EditorGUI.BeginChangeCheck();actuatorObjet.Update ();
		//
		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Animation Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.actuatorAnimator = EditorGUILayout.ObjectField ("Nozzle Animator", actuator.actuatorAnimator, typeof(Animator), true) as Animator;
		GUILayout.Space (3f);
		actuator.animationLayer = EditorGUILayout.IntField("Animation Layer",actuator.animationLayer);
		GUILayout.Space (3f);
		actuator.animationName = EditorGUILayout.TextField("Animation Name",actuator.animationName);

		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Actuation Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.moveSpeed = EditorGUILayout.Slider("Actuation Speed",actuator.moveSpeed,0,1);
		GUILayout.Space (3f);
		EditorGUILayout.LabelField("Actuation level",(actuator.currentActuationLevel *100f).ToString("0.00") + " %");
		GUILayout.Space(5f);
		actuator.invertMotion = EditorGUILayout.Toggle("Invert Motion", actuator.invertMotion);

		GUILayout.Space (10f);
		GUI.color = silantroColor;
		EditorGUILayout.HelpBox ("Drag Configuration", MessageType.None);
		GUI.color = backgroundColor;
		GUILayout.Space (3f);
		actuator.generatesDrag = EditorGUILayout.Toggle ("Generates Drag", actuator.generatesDrag);
		if(actuator.generatesDrag)
		{
			GUILayout.Space (3f);
			actuator.dragFactor = EditorGUILayout.FloatField("Drag Factor", actuator.dragFactor);
			GUILayout.Space (3f);
			EditorGUILayout.LabelField("Current DragFactor",actuator.currentDragFactor.ToString("0.000"));
		}
		
		if (EditorGUI.EndChangeCheck ()) {Undo.RegisterCompleteObjectUndo (actuatorObjet.targetObject, "Actuator Change");}
		if (GUI.changed) {
			EditorUtility.SetDirty (actuator);
			EditorSceneManager.MarkSceneDirty (actuator.gameObject.scene);
		}
		actuatorObjet.ApplyModifiedProperties();
	}
}
#endif
