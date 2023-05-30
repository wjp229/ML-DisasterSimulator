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
            "Init Escape Time : " + info.InitEscapeTime.ToString("F1") + " sec\n" +
            "Average Escape Time : " + info.AvgEscapeTime.ToString("F1") + " sec\n" +
            "Last Escape Time : " + info.LastEscapeTime.ToString("F1") + " sec\n" +
            "Death Count : " + info.DeathCnt + "\n" +
            "Death Rate : " + Mathf.Floor(info.DeathRate * 30) +"%";
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
