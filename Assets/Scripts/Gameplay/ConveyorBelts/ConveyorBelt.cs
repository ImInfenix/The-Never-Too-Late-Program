using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ConveyorBelt : MonoBehaviour
{
    public const string commonTag = "ConveyorBelt";
    public enum TransferType { Normal, Entrance, Exit, Both }
    public enum TransferAxis { X, Z }
    public enum TransferDirection { Negative = -1, Positive = 1 }

    [Header("Type")]
    [SerializeField]
    private TransferType transferType = TransferType.Normal;
    [SerializeField, Tooltip("Only used if the conveyor belt has an exit.")]
    private bool isFinisher = false;
    [SerializeField]
    private TransferAxis transferAxis = TransferAxis.X;
    public TransferAxis GetTransferAxis() { return transferAxis; }
    [SerializeField]
    private TransferDirection transferDirection = TransferDirection.Positive;
    public TransferDirection GetTransferDirection() { return transferDirection; }

    [Header("Caracteristics")]
    [SerializeField]
    private float singleElementSize;
    [SerializeField, Range(2, 30)]
    private int size;
    private int Count { get { return size - 1; } }
    [SerializeField]
    private Vector3 spawnOffset;
    [SerializeField]
    private float speed = 0.5f;
    public float Speed { get { return speed; } }

    [Header("Prefabs")]
    [SerializeField]
    private GameObject conveyorBeltPartPrefab;
    [SerializeField]
    private GameObject conveyorBeltEntrancePrefab;
    [SerializeField]
    private GameObject conveyorBeltExitPrefab;
    [SerializeField]
    private GameObject conveyorBeltFinisherPrefab;

    [Header("Editor")]
    public bool alwaysDraw = false;

    private List<ConveyorBeltPart> parts;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        transform.position = PositionMapper.GetCubeRealWorldPosition(transform.position, true);
        Vector3 direction = GetDirection();
        transform.LookAt(transform.position + direction);
        GenerateParts(direction);
    }

    private void GenerateParts(Vector3 direction)
    {
        parts = new List<ConveyorBeltPart>();

        if (transferType == TransferType.Entrance || transferType == TransferType.Both)
            Generatepart(conveyorBeltEntrancePrefab, direction, 0);
        else
            Generatepart(conveyorBeltPartPrefab, direction, 0);


        for (int i = 1; i < Count; i++)
        {
            Generatepart(conveyorBeltPartPrefab, direction, i);
        }

        if (transferType == TransferType.Exit || transferType == TransferType.Both)
            Generatepart(isFinisher ? conveyorBeltFinisherPrefab : conveyorBeltExitPrefab, direction, Count);
        else
            Generatepart(conveyorBeltPartPrefab, direction, Count);

        var it = parts.GetEnumerator();
        ConveyorBeltPart previous = null;
        while (it.MoveNext())
        {
            it.Current.Initialize(this, previous, singleElementSize);
            previous = it.Current;
        }

        if (transferType == TransferType.Exit || transferType == TransferType.Both)
        {
            parts[Count].GetComponent<ConveyorBeltExit>().SetIsFinisher(isFinisher);
        }
    }

    private void Generatepart(GameObject prefab, Vector3 direction, int index)
    {
        ConveyorBeltPart newPart = Instantiate(prefab).GetComponent<ConveyorBeltPart>();
        newPart.transform.SetParent(transform);
        newPart.transform.position = transform.position + direction * index * singleElementSize + spawnOffset;
        newPart.transform.LookAt(newPart.transform.position + direction);
        newPart.name = $"Part {index}";
        parts.Add(newPart);
    }

    private Vector3 GetDirection()
    {
        Vector3 directionVector = Vector3.zero;
        if (transferAxis == TransferAxis.X)
        {
            directionVector = Vector3.right;
        }
        else if (transferAxis == TransferAxis.Z)
        {
            directionVector = Vector3.forward;
        }

        return directionVector * (int)transferDirection;
    }

    public void ClearHierarchy()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 directionVector = GetDirection();
        Vector3 drawStartPosition = transform.position + transform.up;
        Vector3 drawEndPosition = drawStartPosition + directionVector * Count * singleElementSize;

        Gizmos.color = (transferType == TransferType.Entrance || transferType == TransferType.Both) ? Color.green : CustomColors.transparentGreen;
        Gizmos.DrawSphere(drawStartPosition, singleElementSize * .5f);
        Gizmos.color = (transferType == TransferType.Exit || transferType == TransferType.Both) ? isFinisher ? Color.red : CustomColors.transparentRed : CustomColors.transparentYellow;
        Gizmos.DrawSphere(drawEndPosition, singleElementSize * .5f);

        Gizmos.color = Color.cyan;
        for (int i = 0; i < size; i++)
        {
            Gizmos.DrawSphere(drawStartPosition + directionVector * i * singleElementSize, singleElementSize * .25f);
        }

        if (!Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(drawStartPosition, drawEndPosition);
        }
    }

    private void OnDrawGizmos()
    {
        if (alwaysDraw)
            OnDrawGizmosSelected();
    }
#endif
}