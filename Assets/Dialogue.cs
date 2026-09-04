using UnityEngine;
using TMPro; // Diperbaiki dari TMpro menjadi TMPro

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue(); // Ditambah titik koma (;)
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    System.Collections.IEnumerator TypeLine()
    {
        // Diperbaiki: ToCharArray() dan c kecil
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c; // Diperbaiki dari C menjadi c
            yield return new WaitForSeconds(textSpeed); // Diperbaiki dari WaitForSecond menjadi WaitForSeconds
        }
    }

    void NextLine()
    {
        // Diperbaiki: Lenght menjadi Length
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine()); // Diperbaiki dari Typeline menjadi TypeLine
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}