using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public TextMeshProUGUI textContext;

    public void SetResult(ResultInfo info)
    {
        textContext.text =
            "Init Escape Time : " + info.InitEscapeTime + "\n" +
            "Average Escape Time : " + info.AvgEscapeTime + "\n" +
            "Last Escape Time : " + info.LastEscapeTime + "\n" +
            "Death Count : " + info.DeathCnt + "\n" +
            "Death Rate : " + info.DeathRate.ToString("F1");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
