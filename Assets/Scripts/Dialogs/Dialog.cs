using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Game Data/Dialog")]
public class Dialog : ScriptableObject
{
    private const char openBracket = '{';
    private const char closeBracket = '}';

    [SerializeField, TextArea(1, 5)]
    private List<string> lines;
    public List<string> Lines { get { return lines; } }

    public static string FillTextWithValues(string text)
    {
        List<int> indexes = new List<int>();
        IEnumerator it = text.GetEnumerator();

        int currentIndex = 0;
        while (it.MoveNext())
        {
            char currentChar = (char)it.Current;
            if (currentChar.Equals(openBracket) || currentChar.Equals(closeBracket))
                indexes.Add(currentIndex);
            currentIndex++;
        }

        if (indexes.Count == 0)
            return text;

        int[] occurences = indexes.ToArray();

        List<string> toFill = new List<string>();
        int lastIndex = -1;
        foreach (int i in indexes)
        {
            if (lastIndex == -1)
            {
                lastIndex = i;
                continue;
            }

            toFill.Add(text.Substring(lastIndex, i - lastIndex + 1));
            lastIndex = -1;
        }

        foreach (string key in toFill)
        {
            text = text.Replace(key, key switch
            {
                "{MovingKeys}" => GameManager.Instance.keyboardLayout.GetMovingKeysToString(),
                _ => "{No value found}",
            });
        }

        return text;
    }
}
