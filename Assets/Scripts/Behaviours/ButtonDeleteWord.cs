using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (fieldBehaviour.indexesLetters.Contains(fieldBehaviour.indexesTransforms[transform]))
        {
            var index = fieldBehaviour.indexesTransforms[transform];
            DeleteLetters(visited, index);
        } 
    }

    private void DeleteLetters(HashSet<int> visited, int index)
    {
        var length = Config.FieldSize;
        if (index < 0 || index > length * length) return;

        if (!visited.Contains(index) && fieldBehaviour.indexesLetters.Contains(index))
        {
            visited.Add(index);
            fieldBehaviour.DeleteLetter(index);
            fieldBehaviour.indexesLetters.Remove(index);
            fieldBehaviour.UpdateScore(1);

            if (index + 1 % length != length - 1)
                DeleteLetters(visited, index + 1);
            if (index - 1 % length != 0)
                DeleteLetters(visited, index - 1);
            if (index + length / length != length - 1)
                DeleteLetters(visited, index + length);
            if (index - length / length != 0)
                DeleteLetters(visited, index - length);
        }
    }
}
