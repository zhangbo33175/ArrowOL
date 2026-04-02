using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 网格管理器（GridManager）
    /// </summary>
    public partial class GridMapManager : MonoBehaviour
    {
         [Header("网格设置")]
    public int gridWidth = 10;      // 网格列数
    public int gridHeight = 10;     // 网格行数
    /*public float cellWidth = 1f;    // 网格单元宽度
    public float cellHeight = 0.5f; // 网格单元高度

    [Header("层级设置")]
    public Transform mapLayer;
    public Transform objectLayer;
    public Transform characterLayer;*/
    

    void Awake()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        m_GridTile = new GridTile[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                m_GridTile[x, y] = new GridTile(x, y);
            }
        }
    }

   

 
    // 放置物体
    public bool PlaceObject(GameObject obj, Vector2Int gridPos, GridType objectType, Transform parent)
    {
        if (GridUtils.IsCellOccupied(gridPos,gridWidth,gridWidth,m_GridTile))
            return false;

        m_GridTile[gridPos.x, gridPos.y].IsOccupied = true;
        m_GridTile[gridPos.x, gridPos.y].Occupant = obj;
        m_GridTile[gridPos.x, gridPos.y].ObjectType = objectType;
        // obj.transform.position = GridUtils.GridToWorld(gridPos);
        // obj.transform.SetParent(parent);
        return true;
    }

    // 移除物体
    public void RemoveObject(Vector2Int gridPos)
    {
        if (gridPos.x < 0 || gridPos.x >= gridWidth || gridPos.y < 0 || gridPos.y >= gridHeight)
            return;

        Destroy(m_GridTile[gridPos.x, gridPos.y].Occupant);
        m_GridTile[gridPos.x, gridPos.y].IsOccupied = false;
        m_GridTile[gridPos.x, gridPos.y].Occupant = null;
        m_GridTile[gridPos.x, gridPos.y].ObjectType = 0;
    }

    // 绘制网格 Gizmos
    void OnDrawGizmos()
    {
        if (m_GridTile == null)
            GenerateGrid();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Vector3 worldPos = GridUtils.GridToWorld(new Vector2Int(x, y));
                // Gizmos.color = m_GridTile[x, y].IsOccupied ? Color.red : Color.green;
                // Gizmos.DrawWireCube(worldPos, new Vector3(cellWidth, cellHeight, 0));
            }
        }
    }
    }
}