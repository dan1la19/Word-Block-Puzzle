using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDeleteWord : MonoBehaviour
{
    private FieldBehaviour fieldBehaviour;

    private void Start()
    {
        fieldBehaviour = transform.parent.GetComponent<FieldBehaviour>();
    }

    public void Click()
    {
        var visited = new HashSet<int>();

        if (fieldBehaviour.IndexesLetters.Contains(fieldBehaviour.IndexesTransforms[transform]))
        {
            var index = fieldBehaviour.IndexesTransforms[transform];
            DeleteLetters(visited, index);
        }
        fieldBehaviour.Saving.Save();
    }

    private void DeleteLetters(HashSet<int> visited, int index)
    {
        var length = Config.FieldSize;
        if (index < 0 || index > length * length) return;

        if (!visited.Contains(index) && fieldBehaviour.IndexesLetters.Contains(index))
        {
            visited.Add(index);
            fieldBehaviour.DeleteLetter(index);
            fieldBehaviour.IndexesLetters.Remove(index);

            if ((index + 1) % length != length && (index + 1) % length != -1)
                DeleteLetters(visited, index + 1);
            if ((index - 1) % length != -1 && (index - 1) % length != length)
                DeleteLetters(visited, index - 1);
            if ((index + length) / length != length)
                DeleteLetters(visited, index + length);
            if ((index - length) / length != -1)
                DeleteLetters(visited, index - length);
        }
    }
}
