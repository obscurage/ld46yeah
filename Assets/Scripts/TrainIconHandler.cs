using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainIconHandler : MonoBehaviour
{
    public GameObject trainIcon;
    public GameObject railEndObject;
    Vector2 railEndPos;
    Vector2 railStart;
    GameManager gm;
    float totalDistance;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        railEndPos = railEndObject.transform.position;
        railStart = trainIcon.transform.position;
        totalDistance = Vector2.Distance(railStart, railEndPos);
    }

    // Update is called once per frame
    void Update()
    {
        float distancePercentage = gm.distanceTravelled / gm.totalDistance;
        print(distancePercentage);
        trainIcon.transform.position = new Vector2(railStart.x + totalDistance * distancePercentage / 100, trainIcon.transform.position.y);
    }
}
