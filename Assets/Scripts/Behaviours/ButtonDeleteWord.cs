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
        //fieldBehaviour.DeleteWords();
        var list = new List<HashSet<Transform>>();
        foreach (var word in fieldBehaviour.words2and3letters)
        {
            if (word.Contains(transform))
            {
                foreach (var fieldCell in word)
                {
                    fieldBehaviour.DeleteLetter(fieldCell);
                }
                list.Add(word);
            }
        }
        foreach(var e in list)
        {
            fieldBehaviour.words2and3letters.Remove(e);
        }
    }
}
