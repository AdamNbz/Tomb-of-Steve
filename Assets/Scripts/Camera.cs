using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform player;
    [SerializeField] Vector3 offset;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
