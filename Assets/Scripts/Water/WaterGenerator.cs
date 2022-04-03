using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour {

    [SerializeField] private WavePoint wavePointPrefab;
    int _waveLength;
    WavePoint[] _wave;

    public void InitWaveVisual(int xLength) {
        _waveLength = xLength;
        _wave = new WavePoint[_waveLength];
        for (int i = 0; i < _waveLength; i++) {
            WavePoint newPoint = Instantiate(wavePointPrefab);
            newPoint.Init(new Vector2(i ,0), transform);
            newPoint.CreatePoints();
            _wave[i] = newPoint;
        }
        
    }

    public void SendWaveData(int[] waveData) {
        for (int i = 0; i < waveData.Length; i++) {
            AdjacentY adj = new AdjacentY();
            adj.hasLeft = i == 0 ? false : true;
            adj.hasRight = i == waveData.Length - 1 ? false : true;

            if(adj.hasLeft) adj.leftY = waveData[i - 1];
            if(adj.hasRight) adj.rightY = waveData[i + 1];
            _wave[i].UpdateY(waveData[i], i, adj);
        }
    }
    
}
