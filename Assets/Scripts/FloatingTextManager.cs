using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{

    public static FloatingTextManager Instance;    //ΩÃ±€≈Ê
    public GameObject textPrefabs;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string text, Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        GameObject textobj = Instantiate(textPrefabs, transform);
        textobj.transform.position = screenPos;

        TextMeshProUGUI temp = textobj.GetComponent<TextMeshProUGUI>();

        if (temp != null )
        {
            temp.text = text;

            StartCoroutine(AnimateText(textobj));
        }
    }

    private IEnumerator AnimateText(GameObject textobj)
    {
        float duration = 1f;
        float timer = 0;

        Vector3 startPos = textobj.transform.position;
        TextMeshProUGUI temp = textobj.GetComponent<TextMeshProUGUI>();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            textobj.transform.position = startPos + Vector3.up * (progress * 50f);

            if (temp != null)
            {
                temp.alpha = 1 - progress;
            }
            yield return null;
        }
        Destroy(textobj);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
