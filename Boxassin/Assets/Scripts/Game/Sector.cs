using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour {
    public int m_IndexX, m_IndexZ;
    public List<Transform> arr_InOBJ {get;set;}

    public void SetIndex(int _indexX, int _indexZ) {
        m_IndexX = _indexX;
        m_IndexZ = _indexZ;
    }

    private void Awake() {
        arr_InOBJ = new List<Transform>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!arr_InOBJ.Contains(other.transform)) {
            arr_InOBJ.Add(other.transform);
            other.transform.GetComponent<Detecter>().InSector(m_IndexX, m_IndexZ);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (arr_InOBJ.Contains(other.transform))
            arr_InOBJ.Remove(other.transform);
    }
}
