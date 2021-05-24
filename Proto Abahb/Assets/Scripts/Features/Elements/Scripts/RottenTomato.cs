using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenTomato : MonoBehaviour
{
    public bool isDirty;

    public Material material;

    public Color cleanColor;
    public Color dirtyColor;

    public float colorSpeed;

    public LayerMask layerMask;

    private void Start()
    {
        material.color = isDirty ? dirtyColor : cleanColor;
    }

    private void CleanTomato()
    {
        if (isDirty)
        {
            StartCoroutine(LerpDirt());
            isDirty = false;
        }
    }

    IEnumerator LerpDirt()
    {
        for (float ft = 0f; ft <= 1f; ft += Time.deltaTime * colorSpeed)
        {
            Color _color = Color.Lerp(dirtyColor, cleanColor, ft);
            material.color = _color;
            Debug.LogWarning(_color);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("science chienne");

        if (layerMask == (layerMask | (1 << other.gameObject.layer)))
        {
            CleanTomato();
        }
    }
}
