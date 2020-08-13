using System.Collections;
using UnityEngine;
using TMPro;
public class LoadingTextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    private int count = 0;
    void Start()
    {
        if (count == 4)
        {
            count = 0;
        }

        StartCoroutine(updateText());
    }

    IEnumerator updateText()
    {
        switch (count)
        {
            case 0:
                text.text = "Loading";
                break;
            case 1:
                text.text = "Loading.";
                break;
            case 2:
                text.text = "Loading..";
                break;
            case 3:
                text.text = "Loading...";
                break;
            default:
                break;
        }
        count += 1;
        yield return new WaitForSeconds(0.2f);
        Start();
    }
}
