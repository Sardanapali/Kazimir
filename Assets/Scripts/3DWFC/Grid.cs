﻿using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour {

    private GridCell[,,] cells;

    private const int BatchSize = 100;
    private int nbBatches;
    private int batchesFinished;


    private void Start () {
    }

    public void Init(GameObject model, float gridCellSize) {

        var gridCellPrefab = Resources.Load("Prefabs/GridCell");

        var modelSize = FindMaxVectorPos(model);

        var nbCells = (int) (modelSize / gridCellSize);
        cells = new GridCell[nbCells, nbCells, nbCells];
        var xi = 0;
        var yi = 0;
        var zi = 0;

        for (var x = -modelSize; x < modelSize; x += gridCellSize) {
            for (var y = -modelSize; y < modelSize; y += gridCellSize) {
                for (var z = -modelSize; z < modelSize; z += gridCellSize) {
                    var gridCellObj = Instantiate(gridCellPrefab) as GameObject;
                    var gCell = gridCellObj.GetComponent<GridCell>();

                    gridCellObj.transform.parent = transform;

                    var initPoint = new Vector3(x + gridCellSize / 2, y + gridCellSize / 2, z + gridCellSize / 2);
                    gCell.Init(initPoint, gridCellSize);

                    //Add cell to the cells.
                    cells[xi, yi, zi] = gCell;

                    zi++;
                }
                yi++;
            }
            xi++;
        }

        //Determine the number of batches
        nbBatches = cells.Length/ BatchSize;
    }

    private void Update() {
        if (batchesFinished < nbBatches) {
            for (var i = batchesFinished * BatchSize; i < batchesFinished * BatchSize + BatchSize; i++) {
                /*
                foreach (var voxel in cells[i].ContainedVoxels) {
                    voxel.GetComponent<MeshRenderer>().enabled = false;
                }
*/
                //cells[i].transform.position = new Vector3(Random.Range(-50, 0), Random.Range(-50, 0), Random.Range(-50, 0));
            }
            batchesFinished++;
            Debug.Log("Processed: " + batchesFinished * BatchSize + "/" + cells.Length);
        }
    }

    private static float FindMaxVectorPos(GameObject model) {
        float modelSize = 0;
        foreach (Transform child in model.transform) {
            var absValue = Math.Abs(modelSize);

            if (Math.Abs(child.position.x) > absValue) {
                modelSize = Math.Abs(child.position.x);
            }

            if (Math.Abs(child.position.y) > absValue) {
                modelSize = Math.Abs(child.position.y);
            }

            if (Math.Abs(child.position.z) > absValue) {
                modelSize = Math.Abs(child.position.z);
            }
        }

        return modelSize;
    }


    public void ReArrange() {
        foreach (var cell in cells) {
            cell.transform.position = new Vector3(Random.Range(-50, 0), Random.Range(-50, 0), Random.Range(-50, 0));
        }
    }

    private IEnumerator ReArrangeStart() {
        yield return new WaitForSeconds(10);
        ReArrange();
    }

}
