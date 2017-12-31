using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FrameRater : MonoBehaviour {

    public int FrameRateLimit = 60;
    private float frameCount = 0;
    public Text FPSCount;
    public float TimeInterval = 0.1f;

    private YieldInstruction waitForTime;

    void Start() {
        waitForTime = new WaitForSeconds(TimeInterval);
        StartCoroutine(changeFramerate());
    }

    IEnumerator changeFramerate() {
        yield return waitForTime;
        Application.targetFrameRate = FrameRateLimit;

        while (true) {

            if (FPSCount != null) {
                FPSCount.text = (frameCount / TimeInterval).ToString();
                FPSCount.color = Color.Lerp(Color.red, Color.green, Mathf.Clamp((float) frameCount, 20f, 60f) / 40);
                frameCount = 0;
            }

            yield return waitForTime;

        }

    }

    void Update() {
        frameCount++;
    }

}

