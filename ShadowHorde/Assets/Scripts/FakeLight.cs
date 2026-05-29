using UnityEngine;

public class FakeLantern : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}