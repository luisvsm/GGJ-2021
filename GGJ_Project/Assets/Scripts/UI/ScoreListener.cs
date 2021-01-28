
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListener : MonoBehaviour {

    public TextMeshProUGUI _scoreCounter;
    private static string SCORE_FOMATTER = "Score: {0}";

	// Use this for initialization
	void Start () {
        PlayerState.Instance.OnScoreUpdate += OnScoreChanged;
        _scoreCounter.text = string.Format(SCORE_FOMATTER, 0);
	}

    private void OnScoreChanged (int newScore) {
        _scoreCounter.text = string.Format(SCORE_FOMATTER, newScore);
	}

    void OnDestroy() {
        PlayerState.Instance.OnScoreUpdate -= OnScoreChanged;
    }
}
