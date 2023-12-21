using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Utils;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerScore : MonoBehaviour
    {
        [SerializeField] private float tickPeriod = 0.2f;
        [SerializeField] private ScoreGainFx gainFx;
        [SerializeField] private StreakFx streakFx;
        [SerializeField] private TextMeshProUGUI scoreText;

        private int _streak;
        private float _lastStreakChangeTime;
        private float _lastResetStreakTime;

        private IObjectPool<ScoreGainFx> _poolScoreGainFx;

        private int Streak
        {
            get => _streak;
            set
            {
                _streak = value;
                StreakEffect();
            }
        }

        private bool _gaining = true;
        private int _score;

        private void Start()
        {
            _poolScoreGainFx = new ObjectPool<ScoreGainFx>(
                createFunc: () =>
                {
                    var s = Instantiate(gainFx, transform);
                    s.Origin = _poolScoreGainFx;
                    return s;
                },
                actionOnGet: s =>
                {
                    s.gameObject.SetActive(true);
                    s.SetValue(_streak);
                },
                actionOnRelease: g => g.gameObject.SetActive(false)
            );

            GetComponentInChildren<TowerCollision>().OnGatesTriggered += StreakIncrease;
            GetComponentInChildren<TowerCollision>().OnObstacleCollided += ResetStreak;
            GetComponent<TowerMove>().OnHasteSwitch += enable => _gaining = enable;
            Streak = 1;
            StartCoroutine(ScoreTick());
        }

        private void ResetStreak()
        {
            if (Time.time - _lastResetStreakTime > 0.5f)
            {
                Streak = 0;
                _lastResetStreakTime = Time.time;
            }
        }

        private void StreakIncrease()
        {
            if (Time.time - _lastStreakChangeTime < 0.5f)
                return;
            Streak++;
            _poolScoreGainFx.Get().SetValue(_streak * 10);
            _score += _streak * 10;
            scoreText.SetText(_score.ToString());
            _lastStreakChangeTime = Time.time;
        }

        private IEnumerator ScoreTick()
        {
            while (true)
            {
                if (_gaining && _streak > 0)
                {
                    _score += Streak;
                    scoreText.SetText(_score.ToString());
                    _poolScoreGainFx.Get();
                }

                yield return WaitForSecondsPool.Get(tickPeriod);
            }
        }

        private void StreakEffect()
        {
            if (_streak > 0)
            {
                streakFx.PlayFx(_streak);
            }
        }
    }
}