using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public Transform ScoreBoardRect;

    [SerializeField]
    private GameObject EntryPrefab;

    private void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Create();
        }
    }

    private void Create()
    {
        ScoreBoardRect.ClearChildren();
        for (int i = 0; i < ServerDetails.TotalConnections(); i++)
        {
            GameObject EntryClone = (GameObject)Instantiate(EntryPrefab, Vector3.zero, Quaternion.identity);
            EntryClone.transform.SetParent(ScoreBoardRect);
            EntryClone.transform.localScale = Vector3.one;
            EntryClone.GetComponent<ScoreBoardEntry>().Created("BoB", 4, 10);
        }
    }
}