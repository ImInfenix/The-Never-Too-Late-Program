using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConveyorBelt)), CanEditMultipleObjects]
public class ConveyorBeltEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ConveyorBelt[] cbs = new ConveyorBelt[targets.Length];
        for (int i = 0; i < cbs.Length; i++)
        {
            cbs[i] = targets[i] as ConveyorBelt;
        }

        if (GUILayout.Button("Generate"))
        {
            foreach(ConveyorBelt cb in cbs)
                cb.Setup();
        }

        if (GUILayout.Button("Clear"))
        {
            foreach (ConveyorBelt cb in cbs)
                cb.ClearHierarchy();
        }
    }
}