using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSprinkles : MonoBehaviour
{
    public int numberOfEachSprinkles;
    public float radius;
    [SerializeField] GameObject[] sprinkleTypes;

    private void Start()
    {
        PlaceSprinkles();
    }

    void PlaceSprinkles()
    {
        foreach(GameObject sprinklePrefab in sprinkleTypes)
        {
            for (int i = 0; i < numberOfEachSprinkles; i++)
            {
                Transform sprinkle = Instantiate(sprinklePrefab, Random.onUnitSphere * radius, Quaternion.identity, transform).transform;
                sprinkle.LookAt(Vector3.zero);
            }
        }
    }

}
