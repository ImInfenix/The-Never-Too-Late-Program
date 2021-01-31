using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider))]
public class ConveyorBeltPart : MonoBehaviour
{
    public ConveyorBelt ParentConveyorBelt { get; private set; }
    public ConveyorBeltPart Previous { get; private set; }
    public ConveyorBeltPart Next { get; private set; }
    public Vector3 CarryablesAnchor { get { return PositionMapper.GetCubeRealWorldPosition(transform.position + transform.up); } }

    protected float size;

    public void Initialize(ConveyorBelt conveyorBelt, ConveyorBeltPart previous, float size)
    {
        ParentConveyorBelt = conveyorBelt;
        if (previous != null)
        {
            Previous = previous;
            previous.Next = this;
        }
        this.size = size;
#if UNITY_EDITOR
        OnValidate();
#endif
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CarryableObject.commonTag))
        {
            CarryableObject co = other.GetComponent<CarryableObject>();
            co.CurrentConveyorBeltPart = this;
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(CarryableObject.commonTag))
        {
            CarryableObject co = other.GetComponent<CarryableObject>();
            if (co.CurrentConveyorBeltPart != this)
                return;

            if (IsAlignedAlongAxis(co.gameObject))
                co.Move(transform.forward, ParentConveyorBelt.Speed);
            else
            {
                co.MoveToAlign(CarryablesAnchor, ParentConveyorBelt.Speed);
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CarryableObject.commonTag))
        {
            CarryableObject co = other.GetComponent<CarryableObject>();
            if (co.CurrentConveyorBeltPart != this)
                return;

            co.CurrentConveyorBeltPart = null;
        }
    }

    private bool IsAlignedAlongAxis(GameObject g)
    {
        return (ParentConveyorBelt.GetTransferAxis()) switch
        {
            ConveyorBelt.TransferAxis.X => PositionMapper.Equals(CarryablesAnchor.z, g.transform.position.z),
            ConveyorBelt.TransferAxis.Z => PositionMapper.Equals(CarryablesAnchor.x, g.transform.position.x),
            _ => false,
        };
    }

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (gameObject.scene.name == null || gameObject.scene.name == gameObject.name)
            return;

        if (!PositionMapper.IsAligned(gameObject, true))
        {
            Debug.LogWarning($"Object {name} is not aligned: {transform.position}");
        }
    }
#endif
}
