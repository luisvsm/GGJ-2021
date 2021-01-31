using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLeafLitter : MonoBehaviour
{
    public List<GameObject> leafLitterPrefabs;
    private static int leafLitterSpawnCount;

    public float minWaitTime = 3f;
    public float maxWaitTime = 5f;
    public int maxToSpawn = 10;

    private float nextSpawnTime = 0;

    public float sizeX, sizeY;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (nextSpawnTime < Time.time)
        {
            SpawnStuff();
            nextSpawnTime = Time.time + Random.Range(minWaitTime, maxWaitTime);
        }
    }

    private Vector3 getRandomPosition()
    {
        return new Vector3(
            Random.Range(-sizeX / 2, sizeX / 2),
            Random.Range(-sizeY / 2, sizeY / 2),
            0f
        );
    }
    GameObject GetObjectToSpawn()
    {
        return leafLitterPrefabs[Random.Range(0, leafLitterPrefabs.Count)];
    }

    void SpawnStuff()
    {
        if (leafLitterSpawnCount < maxToSpawn)
        {
            GameObject newLeafLitter = Instantiate(GetObjectToSpawn(), transform.position + getRandomPosition(), Quaternion.identity);
            newLeafLitter.transform.parent = transform.parent;
            leafLitterSpawnCount++;
        }
    }

    public static void LeafLitterWasDestroyed()
    {
        leafLitterSpawnCount--;
    }

#if UNITY_EDITOR
    private Vector2[] drawPoints = new Vector2[4];
    private Vector2 temporaryVector;

    // The following code only needs to run in Editor, and is used to display the bounding box on where text can be moved to
    void setPointPosition(int i, float x, float y)
    {
        temporaryVector = new Vector2(x / 2, y / 2);
        temporaryVector = transform.rotation * temporaryVector;
        drawPoints[i] = new Vector2(transform.position.x, transform.position.y) + temporaryVector;
    }

    void OnDrawGizmosSelected()
    {
        // Set bounding box points
        setPointPosition(0, sizeX, sizeY);
        setPointPosition(1, -sizeX, sizeY);
        setPointPosition(2, sizeX, -sizeY);
        setPointPosition(3, -sizeX, -sizeY);

        // Draw bounding boxes in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawLine(drawPoints[0], drawPoints[1]);
        Gizmos.DrawLine(drawPoints[0], drawPoints[2]);

        Gizmos.DrawLine(drawPoints[3], drawPoints[1]);
        Gizmos.DrawLine(drawPoints[3], drawPoints[2]);
        // syncText.areaSize
    }
#endif

}
