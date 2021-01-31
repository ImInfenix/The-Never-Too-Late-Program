using UnityEngine;

[CreateAssetMenu(fileName ="NewLayout", menuName ="Game Data/Keyboard Layout")]
public class KeyboardLayout : ScriptableObject
{
    [Header("Movement")]
    [SerializeField]
    private KeyCode forward;
    public KeyCode Forward { get { return forward; } }
    [SerializeField]
    private KeyCode left;
    public KeyCode Left { get { return left; } }
    [SerializeField]
    private KeyCode backward;
    public KeyCode Backward { get { return backward; } }
    [SerializeField]
    private KeyCode right;
    public KeyCode Right { get { return right; } }
    [SerializeField]
    private KeyCode interaction;
    public KeyCode Interaction { get { return interaction; } }

    public string GetMovingKeysToString()
    {
        return $"{forward}{left}{backward}{right}";
    }
}
