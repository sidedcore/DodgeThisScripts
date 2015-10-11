using UnityEngine;

public class SpawnPointElement : MonoBehaviour
{
    private bool isEnabled = false;
    public SpawnTeamID SpawnTeamPoint = SpawnTeamID.FreeForAll;

    private void Awake()
    {
        MeshRenderer[] MR = GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer mr in MR)
        {
            mr.enabled = isEnabled;
        }
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        MeshRenderer[] MR = GetComponentsInChildren<MeshRenderer>(true);
        foreach (MeshRenderer mr in MR)
        {
            mr.enabled = isEnabled;
        }
    }
}