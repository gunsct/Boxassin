using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detecter : MonoBehaviour
{
    /// <summary>
    /// 20.05.12_유병훈
    /// 20*20 그리드 맵에서 4*4 크기 총 25개 구역으로 나눠 둘레 포함 9구역 내에 있는 대상들만 타겟으로 잡음.
    /// 탐색 대상들을 부채꼴 70도 범위 또는 전방위 4,5 거리 내에서 탐지된 경우 이벤트 발생
    /// </summary>

    public Transform m_Target;
    int m_InSectorIndexX, m_InSectorIndexZ;

    public void InSector(int _indexX, int _indexZ) {
        m_InSectorIndexX = _indexX;
        m_InSectorIndexZ = _indexZ;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
