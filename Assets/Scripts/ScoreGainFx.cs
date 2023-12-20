using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

public class ScoreGainFx : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Vector3[] endPoints;
    [SerializeField] private TextMeshProUGUI text;

    public IObjectPool<ScoreGainFx> Origin;

    public void SetValue(int value)
    {
        text.SetText($"+{value}");
    }

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float time = 0;
            
        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(endPoints[0], endPoints[1], EasingFunctions.EaseOutCirc(time / duration));
                
            yield return null;
            time += Time.deltaTime;
        }

        Origin.Release(this);
    }

        
      

}