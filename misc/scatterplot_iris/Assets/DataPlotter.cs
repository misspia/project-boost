using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlotter : MonoBehaviour {

    public string inputfile;

    private List<Dictionary<string, object>> pointList;
    List<string> columnList;

    // column indicies
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    // full column names
    public string xName;
    public string yName;
    public string zName;

    public GameObject PointPrefab;
    public GameObject PointHolderPrefab;

    public float plotScale = 10.0f;

	void Start () {
        pointList = CSVReader.Read(inputfile);
        columnList = new List<string>(pointList[1].Keys);

        PrintColumnNames();
        AssignColumnNames();
        InstantiatePoints();
	}
    private void PrintColumnNames()
    {
        Debug.Log("There are " + columnList.Count + " columns in the CSV");

        foreach (string key in columnList)
        {
            Debug.Log("Column name is " + key);
        }
    }
    private void AssignColumnNames()
    {
        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];
    }
    private void InstantiatePoints()
    {
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);

        for (var i = 0; i < pointList.Count; i++)
        {

            float x = FindNormalizedPos(Convert.ToSingle(pointList[i][xName]), xMin, xMax);
            float y = FindNormalizedPos(Convert.ToSingle(pointList[i][yName]), yMin, yMax);
            float z = FindNormalizedPos(Convert.ToSingle(pointList[i][zName]), zMin, zMax);

            GameObject dataPoint = Instantiate(
                PointPrefab,
                new Vector3(x, y, z) * plotScale,
                Quaternion.identity
            );
            dataPoint.transform.parent = PointHolderPrefab.transform;

            string dataPointName =
                pointList[i][xName] + " " +
                pointList[i][yName] + " " +
                pointList[i][zName];
            dataPoint.transform.name = dataPointName;

            dataPoint.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);
        }
    }
    private float FindMaxValue(string columnName)
    {
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        for(var i = 0; i < pointList.Count; i ++)
        {
            float currValue = Convert.ToSingle(pointList[i][columnName]);
            maxValue = Mathf.Max(maxValue, currValue);
        }
        return maxValue;
    }
    private float FindMinValue(string columnName)
    {
        float minValue = Convert.ToSingle(pointList[0][columnName]);

        for (var i = 0; i < pointList.Count; i++)
        {
            float currValue = Convert.ToSingle(pointList[i][columnName]);
            minValue = Mathf.Min(minValue, currValue);
        }
        return minValue;
    }
    private float FindNormalizedPos(float pos, float min, float max)
    {
        return (pos - min) / (max - min);
    }


}



