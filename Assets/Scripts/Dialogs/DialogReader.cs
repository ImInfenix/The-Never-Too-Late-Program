using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogReader : MonoBehaviour
{
    public static List<IDeactivable> deactivableComponents;

    [SerializeField]
    private DialogArea dialogArea;

    public void Read(Dialog dialog, IEnumerator actionToDoAfter = null, bool giveControlsBack = true)
    {
        StartCoroutine(DisplayDialog(dialog, actionToDoAfter, giveControlsBack));
    }

    private IEnumerator DisplayDialog(Dialog dialog, IEnumerator actionToDoAfter, bool giveControlsBack)
    {
        foreach (IDeactivable deactivable in deactivableComponents)
            deactivable.DisableComponent();

        dialogArea.gameObject.SetActive(true);

        IEnumerator linesIt = dialog.Lines.GetEnumerator();
        while (linesIt.MoveNext())
        {
            dialogArea.DisplayText(Dialog.FillTextWithValues($"{linesIt.Current}"));

            yield return null;

            while (!(Input.GetKeyDown(GameManager.Instance.keyboardLayout.Interaction) || Input.GetMouseButtonDown(0)))
                yield return null;
        }

        dialogArea.ClearText();

        dialogArea.gameObject.SetActive(false);

        if (giveControlsBack)
            foreach (IDeactivable deactivable in deactivableComponents)
                deactivable.EnableComponent();

        yield return actionToDoAfter;
    }

    public static void Register(IDeactivable deactivable)
    {
        if (deactivableComponents == null)
            deactivableComponents = new List<IDeactivable>();

        deactivableComponents.Add(deactivable);
    }

    public static void Unregister(IDeactivable deactivable)
    {
        if (deactivableComponents == null)
            return;

        deactivableComponents.Remove(deactivable);
    }
}
