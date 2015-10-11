using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardEntry : MonoBehaviour
{
    [SerializeField]
    private Text Username;

    [SerializeField]
    private Text Kills;

    [SerializeField]
    private Text Deaths;

    public void Created(string _Username, int _Kills, int _Deaths)
    {
        Username.text = _Username;
        Kills.text = _Kills.ToString();
        Deaths.text = _Deaths.ToString();
    }

    public void SetUsername(string _Username)
    {
        Username.text = _Username;
    }

    public void SetKills(string _Kills)
    {
        Kills.text = _Kills;
    }

    public void SetDeaths(string _Deaths)
    {
        Deaths.text = _Deaths;
    }
}