using UnityEngine;

[DisallowMultipleComponent, ExecuteAlways]
public class CameraFollower : MonoBehaviour, IDeactivable
{
    public const string wallTag = "Wall";

    [Header("Object to follow")]
    [SerializeField]
    private Transform m_Target;

    [Header("Camera height")]
    [SerializeField]
    private float m_Height;
    [SerializeField]
    private float m_MinHeight = 1f;
    [SerializeField]
    private float m_MaxHeight = 20f;

    [Header("Camera distance")]
    [SerializeField]
    private float m_Distance;
    [SerializeField]
    private float m_MinDistance = 3f;
    [SerializeField]
    private float m_MaxDistance = 20f;
    [SerializeField]
    private float distanceScrollSpeed = 10f;

    [Header("Other settings")]
    [SerializeField]
    private float m_CurrentAngle;

    [SerializeField]
    private float m_smoothDampSpeed = .1f;
    private Vector3 m_smoothDampVelocity;

    [SerializeField]
    private float cameraLookOffset = 1f;

    private void Awake()
    {
        Register();
    }

    private void Start()
    {
        HandleCamera();
    }

    private void Update()
    {
        HandleMouseInputs();
        HandleCamera();
    }

    private void HandleCamera()
    {
        if (!m_Target)
            return;

        Vector3 worldPosition = (Vector3.forward * -m_Distance) + (Vector3.up * m_Height);
        Vector3 rotatedVector = Quaternion.AngleAxis(m_CurrentAngle, Vector3.up) * worldPosition;

        Vector3 flatTargetPosition = m_Target.position;
        flatTargetPosition.y = 0;
        Vector3 finalPosition = flatTargetPosition + rotatedVector;

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref m_smoothDampVelocity, m_smoothDampSpeed);
        transform.LookAt(flatTargetPosition + Vector3.up * cameraLookOffset);
    }

    private void HandleMouseInputs()
    {
        if (Input.GetMouseButton(2))
        {
            float newHeight = m_Height - Input.GetAxis("Mouse Y");
            m_Height = Mathf.Clamp(newHeight, m_MinHeight, m_MaxHeight);
        }
        else
        {
            m_CurrentAngle += Input.GetAxis("Mouse X");
        }

        float newDistance = m_Distance - Input.mouseScrollDelta.y * distanceScrollSpeed;
        m_Distance = Mathf.Clamp(newDistance, m_MinDistance, m_MaxDistance);
    }

    private void OnDestroy()
    {
        Unregister();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = CustomColors.transparentDarkGreen;
        if (m_Target)
        {
            Gizmos.DrawLine(transform.position, m_Target.position);
            Gizmos.DrawSphere(m_Target.position, 1.5f);
        }
        Gizmos.DrawSphere(transform.position, 1.5f);
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
}
