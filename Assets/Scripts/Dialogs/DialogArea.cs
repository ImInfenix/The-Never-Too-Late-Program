using UnityEngine;
using Text = TMPro.TMP_Text;

public class DialogArea : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image placeHolder;
    [SerializeField]
    private Text textArea;

    public void DisplayText(string textToDisplay)
    {
        textArea.text = textToDisplay;
    }

    public void ClearText()
    {
        textArea.text = "";
    }
}
