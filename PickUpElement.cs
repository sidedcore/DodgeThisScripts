using UnityEngine;

public class PickUpElement : MonoBehaviour
{
    public bool Enabled = true;

    public PickUpType PT = PickUpType.Health;

    private void Update()
    {
        if (Enabled)
        {
            transform.GetChild(0).transform.Rotate(Vector3.up * Time.deltaTime, 0.6f, Space.World);
        }
    }

    public void EnablePickup()
    {
        Enabled = true;
        transform.EnableChildren();
    }

    public void DisablePickup()
    {
        Enabled = false;
        transform.HideChildren();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (Network.isServer)
        {
            if (other.tag == "Player")
            {
                this.transform.parent.parent.GetComponent<PowerUpControl>().AddtoList(this.gameObject, other.gameObject);
                //SendMessageUpwards("AddToList", this.gameObject, );
                DisablePickup();
            }
        }
    }
}

public enum PickUpType
{
    Health = 0,
    Energy = 1
}