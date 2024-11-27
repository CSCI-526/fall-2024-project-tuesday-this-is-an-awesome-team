using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryVisualizer : MonoBehaviour
{
    public GameObject pointPrefab;
    public int numberOfPoints = 30;
    public float timeStep;

    private GameObject[] points;

    // Start is called before the first frame update
    void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
            points[i].transform.localScale = new Vector3(1, 1, 1);
            points[i].SetActive(false);
        }
    }

    public void ShowTrajectory(Vector2 startPosition, Vector2 startVelocity, float gravityScale)
    {
        Vector2 gravity = Physics2D.gravity * gravityScale;
        float timeStep = Time.fixedDeltaTime;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i * timeStep;
            Vector2 position = startPosition + startVelocity * t + 0.5f * gravity * t * t;

            points[i].transform.position = position;
            points[i].transform.localScale = new Vector3(1, 1, 1);
            points[i].SetActive(true);
        }
    }

    public void HideTrajectory()
    {
        foreach (GameObject point in points)
        {
            point.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
