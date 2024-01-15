using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnimateUI : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.1f;
    
    private RectTransform _t;
    
    private void OnEnable()
    {
        _t = GetComponent<RectTransform>();
        Debug.Log("AnimateUI");
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            _t.localScale = (1f + amplitude * Mathf.Sin(3 * Time.time)) * Vector3.one;
            Debug.Log(_t.localScale);
            yield return null;
        }
        
    }
}
