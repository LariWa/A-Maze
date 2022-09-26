using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    public bool isRow;
    public bool moveNegDir;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Init(int index,bool isRow, bool moveNegDir)
    {
        this.index = index;
        this.isRow = isRow;
        this.moveNegDir = moveNegDir;
    }
    public void onClick()
    {
        MazeGenerator.instance.move(index, isRow, moveNegDir);
    }
}
