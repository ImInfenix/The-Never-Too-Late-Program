using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectsInteraction : MonoBehaviour, IDeactivable
{
    public Vector3 CubePosition { get { return PositionMapper.GetCubeRealWorldPosition(transform.position); } }
    public Vector3 CubeFrontPosition { get { return PositionMapper.GetCubeRealWorldPosition(transform.position + transform.forward); } }

    private KeyCode interactionKey = KeyCode.E;

    [Header("Caracteristics")]
    [SerializeField]
    private SpriteRenderer selector;
    private Vector3 selectorOffset = -Vector3.up * .5f + Vector3.up * 0.0001f;
    [SerializeField, Min(1)]
    private int capacity = 5;

    [Header("Editor")]
    [SerializeField]
    private bool alwaysDraw;

    Stack<CarryableObject> carriedObjects;

    private void Awake()
    {
        carriedObjects = new Stack<CarryableObject>();
        ReadKeyboardLayout();
        Register();
    }

    private void Update()
    {
        selector.transform.position = CubeFrontPosition + selectorOffset;

        if (Input.GetKeyDown(interactionKey) || Input.GetMouseButtonDown(0))
        {
            List<Collider> foundObjects = new List<Collider>(Physics.OverlapSphere(CubeFrontPosition, .1f));

            CarryableObject foundCarryableObject = null;

            var it = foundObjects.GetEnumerator();
            while (it.MoveNext())
            {
                if (it.Current.CompareTag(CarryableObject.commonTag) && foundCarryableObject == null)
                {
                    foundCarryableObject = it.Current.GetComponent<CarryableObject>();
                }

                if (it.Current.CompareTag(ConveyorBelt.commonTag))
                {
                    InteractWithConveyorBelt(it.Current.GetComponent<ConveyorBeltPart>());
                    return;
                }

                if (it.Current.CompareTag(ObjectsHolder.commonTag))
                {
                    InteractWithObjectsHolder(it.Current.GetComponent<ObjectsHolder>());
                    return;
                }

                if (it.Current.CompareTag(Phone.commonTag))
                {
                    it.Current.GetComponent<Phone>().InteractWith();
                    return;
                }
            }

            if (foundCarryableObject != null)
                PickUp(foundCarryableObject);
            else if (it.Current == null)
                Dispose(CubeFrontPosition);
        }
    }

    private void PickUp(CarryableObject co)
    {
        if (carriedObjects.Count >= capacity)
            return;

        co.gameObject.transform.SetParent(transform);
        carriedObjects.Push(co);
        co.gameObject.transform.SetPositionAndRotation(transform.position + Vector3.up * (carriedObjects.Count + 1) * .75f, transform.rotation);
        co.OnPickup();
    }

    private void Dispose(Vector3 disposePosition)
    {
        if (carriedObjects.Count == 0 || CubeFrontPosition == CubePosition)
            return;

        CarryableObject co = carriedObjects.Pop();
        if (co == null)
        {
            Dispose(disposePosition);
            return;
        }
        co.gameObject.transform.SetParent(LevelManager.Instance.CarryableObjectsDefaultParent);
        co.transform.SetPositionAndRotation(disposePosition, Quaternion.identity);
        co.OnDispose();
    }

    private void InteractWithConveyorBelt(ConveyorBeltPart cbp)
    {
        if (cbp as ConveyorBeltEntrance != null || cbp as ConveyorBeltExit != null)
            return;

        Vector3 center = cbp.transform.position + cbp.transform.up;
        Vector3 halfExtents = new Vector3(0.4f, 0.4f, 0.4f);
        List<Collider> foundObjects = new List<Collider>(Physics.OverlapBox(center, halfExtents));

        Collider closer = null;

        var it = foundObjects.GetEnumerator();
        while (it.MoveNext())
        {
            if (!it.Current.CompareTag(CarryableObject.commonTag))
                continue;

            if (closer == null || Vector3.Distance(center, it.Current.transform.position) < Vector3.Distance(center, closer.transform.position))
                closer = it.Current;
        }

        if (closer == null)
        {
            Dispose(cbp.CarryablesAnchor);
        }
        else
        {
            PickUp(closer.GetComponent<CarryableObject>());
        }
    }

    private void InteractWithObjectsHolder(ObjectsHolder oh)
    {
        if (carriedObjects.Count > 0 && oh.Dispose(carriedObjects.Peek()))
        {
            carriedObjects.Pop();

            return;
        }
        else
        {
            CarryableObject co = oh.PickUp();
            if(co != null)
            {
                PickUp(co);
            }
        }
    }

    private void ReadKeyboardLayout()
    {
        KeyboardLayout kbl = GameManager.Instance.keyboardLayout;
        interactionKey = kbl.Interaction;
    }

    private void OnDestroy()
    {
        Unregister();
    }

    public void Register()
    {
        DialogReader.Register(this);
    }

    public void Unregister()
    {
        DialogReader.Unregister(this);
    }

    public void EnableComponent()
    {
        enabled = true;
    }

    public void DisableComponent()
    {
        enabled = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (alwaysDraw)
            OnDrawGizmosSelected();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = CustomColors.transparentMagenta;
        Gizmos.DrawCube(CubePosition, Vector3.one);
        Gizmos.color = CustomColors.transparentBlue;
        Gizmos.DrawCube(CubeFrontPosition, Vector3.one);
    }
#endif
}
