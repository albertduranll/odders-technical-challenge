using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoxSummoner : MonoBehaviour
{
    public GameObject ObjectToSpawn;

    public int currentBoxes = 0;
    [SerializeField] private int totalBoxToSpawn = 0;

    private float summoningTime = 6f;
    private float rateOfSpawn;

    private List<BoxData> boxes = new List<BoxData>();

    /// <summary>
    /// Method responible for the spawn of a random number of boxes between time interval.
    /// </summary>
    public void SpawnBoxes()
    {
        totalBoxToSpawn = Random.Range(100, 150);
        GameManager.instance.totalBoxes = totalBoxToSpawn;

        rateOfSpawn = summoningTime / totalBoxToSpawn;

        currentBoxes = 0;
        StartCoroutine(BoxSpawner());
    }

    /// <summary>
    /// Enumerator to handle data generation and the boxes creation.
    /// </summary>
    /// <returns></returns>
    public IEnumerator BoxSpawner()
    {
        yield return new WaitUntil(GenerateBoxesData);

        foreach(BoxData boxData in boxes)
        {
            GameObject newTargetBox = Instantiate(ObjectToSpawn, boxData.pos, transform.rotation);
            newTargetBox.transform.rotation = boxData.rot;
            newTargetBox.transform.localScale = boxData.scale;
            newTargetBox.GetComponent<Renderer>().material.SetColor("_BaseColor", boxData.color);
            newTargetBox.tag = "targetBox";

            currentBoxes++;

            yield return new WaitForSeconds(rateOfSpawn);
        }
    }

    public Vector3 GetRandomPositionWithinTransform()
    {
        Vector3 randomPosWithin;
        randomPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomPosWithin = transform.TransformPoint(randomPosWithin * .5f);

        return randomPosWithin;
    }

    public bool GenerateBoxesData()
    {
        for (int i = 0; i < totalBoxToSpawn; i++)
        {
            BoxData newBoxData = new BoxData();
            newBoxData.pos = GetRandomPositionWithinTransform();
            newBoxData.rot = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f));
            float randomScale = Random.Range(.5f, 1.5f);
            newBoxData.scale = new Vector3(randomScale, randomScale, randomScale);
            newBoxData.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

            boxes.Add(newBoxData);
        }

        SortListByScale();

        return true;
    }

    /// <summary>
    /// Method to sort a List by its objects scale.
    /// </summary>
    public void SortListByScale()
    {
        boxes.Sort((x, y) => x.scale.x.CompareTo(y.scale.x));
    }
}

[System.Serializable]
public class BoxData
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;
    public Color color;
}