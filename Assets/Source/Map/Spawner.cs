using Racing.Game.Management;
using UnityEngine;

/// <summary>
/// Spawns players into the scene
/// </summary>
public class Spawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject AIPrefab;

    private float horizontalSpace = 2f;
    private float verticalSpace = 4f;

    private void Awake()
    {
        for (int i = 0; i < GameManager.Data.TotalPlayerCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                i % 2 == 0 ? transform.position.x - horizontalSpace : transform.position.x, //x
                transform.position.y, //y
                transform.position.z - (verticalSpace * i) //z
                );

            if (i + 1 == GameManager.Data.PlayerStartPosition)
                Instantiate(PlayerPrefab, spawnPos, transform.rotation);
            else
                Instantiate(AIPrefab, spawnPos, transform.rotation);
        }
    }
}
