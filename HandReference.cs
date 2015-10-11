using UnityEngine;

public class HandReference : MonoBehaviour
{
    [SerializeField]
    private Transform BoneToFollow;

    [SerializeField]
    private GameObject[] BallsInHand = new GameObject[6];

    private void Awake()
    {
        transform.HideChildren();
        BallsInHand[0] = transform.Find("DodgeBall_1").gameObject;
        BallsInHand[1] = transform.Find("DodgeBall_2").gameObject;
        BallsInHand[2] = transform.Find("DodgeBall_3").gameObject;
        BallsInHand[3] = transform.Find("DodgeBall_4").gameObject;
        BallsInHand[4] = transform.Find("DodgeBall_5").gameObject;
        BallsInHand[5] = transform.Find("DodgeBall_6").gameObject;
    }

    void LateUpdate()
    {
        transform.position = BoneToFollow.position;
    }

    public void ShowWeapon(int ballID)
    {
        transform.HideChildren();
        int arryNum = ballID -= 1;

        BallsInHand[arryNum].SetActive(true);
        BallsInHand[arryNum].transform.position = this.transform.position;
    }
}