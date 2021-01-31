using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsHolder : MonoBehaviour
{
    public const string commonTag = "ObjectsHolder";
    public Vector3 CarryablesAnchor { get { return PositionMapper.GetCubeRealWorldPosition(transform.position + transform.up * .5f) + transform.up * .125f; } }

    [Header("Settings"), SerializeField]
    private bool spawnWithObject = false;

    [Header("Editor")]
    [SerializeField]
    private bool alwaysDraw;

    private CarryableObject co;
    
    private void Awake()
    {
        Setup();
        if (spawnWithObject)
            Dispose(LevelManager.Instance.RequireSpawn());
    }

    public void Setup()
    {
        transform.position = PositionMapper.GetCubeRealWorldPosition(transform.position, true);
    }

    public CarryableObject PickUp()
    {
        CarryableObject res = co;
        co = null;
        return res;
    }

    public bool Dispose(CarryableObject co)
    {
        if(this.co == null && co != null)
        {
            this.co = co;
            co.transform.SetParent(LevelManager.Instance.CarryableObjectsDefaultParent);
            co.transform.SetPositionAndRotation(CarryablesAnchor, Quaternion.identity);
            return true;
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = CustomColors.transparentGrey;
        Gizmos.DrawSphere(CarryablesAnchor, .25f);
    }

    private void OnDrawGizmos()
    {
        if (alwaysDraw)
            OnDrawGizmosSelected();
    }
#endif

}
