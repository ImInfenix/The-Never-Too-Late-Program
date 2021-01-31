using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpdatableInformation : MonoBehaviour
{
    private static List<UpdatableInformation> updatableInformations;

    public static void UpdateAll()
    {
        foreach (UpdatableInformation info in updatableInformations)
            info.UpdateInformations();
    }

    protected virtual void Awake()
    {
        if (updatableInformations == null)
            updatableInformations = new List<UpdatableInformation>();
        updatableInformations.Add(this);
        UpdateInformations();
    }

    protected abstract void UpdateInformations();

    protected virtual void OnDestroy()
    {
        updatableInformations.Remove(this);
    }
}
