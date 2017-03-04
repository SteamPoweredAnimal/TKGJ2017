using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CanEditMultipleObjects, CustomEditor(typeof(NonDrawnGraphic), false)]
public class NonDrawnGraphicEditor : GraphicEditor
{
	public override void OnInspectorGUI ()
	{
		base.serializedObject.Update();
		EditorGUILayout.PropertyField(base.m_Script, new GUILayoutOption[0]);
		// skipping AppearanceControlsGUI
		base.RaycastControlsGUI();
		base.serializedObject.ApplyModifiedProperties();
	}
}