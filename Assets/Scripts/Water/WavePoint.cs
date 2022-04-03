using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AdjacentY {
    public bool hasRight;
    public bool hasLeft;
    public int rightY;
    public int leftY;
}

public class WavePoint : MonoBehaviour {
    [SerializeField] private GameObject _pointTemplate;


    private Vector3 _position;
    private Vector3 _LPosition;
    private Vector3 _RPosition;

    private Vector3 _basePosition;
    private Vector3 _baseLPosition;
    private Vector3 _baseRPosition;


    private Transform _parent;
    private GameObject visualPoint;
    private GameObject leftPoint;
    private GameObject rightPoint;

    private Coroutine WaveMovement;

    private bool firstSet = false;

    public void Init(Vector2 centralPosition, Transform waveParent) {
        _position = new Vector3(centralPosition.x, 0, centralPosition.y);
        transform.localPosition = _position;
        transform.parent = waveParent;
    }

    public void CreatePoints() {
        visualPoint = Instantiate(
            _pointTemplate,
            new Vector3(_position.x, 0, _position.y),
            Quaternion.identity
        );
        visualPoint.name = "visualPoint";
        visualPoint.transform.parent = transform;

        leftPoint = Instantiate(
            _pointTemplate,
            new Vector3(_position.x - 0.5f, 0, _position.y),
            Quaternion.identity
        );
        leftPoint.name = "leftPoint";
        leftPoint.transform.parent = transform;

        rightPoint = Instantiate(
            _pointTemplate,
            new Vector3(_position.x + 0.5f, 0, _position.y),
            Quaternion.identity
        );
        rightPoint.name = "rightPoint";
        rightPoint.transform.parent = transform;
    }

    public void UpdateY(int newY, int currentX, AdjacentY adjValues) {
        _position = new Vector3(currentX, 0, newY + 0.5f);

        _LPosition = new Vector3(
            currentX - 0.5f,
            0,
            adjValues.hasLeft ? Mathf.Lerp(newY, adjValues.leftY, .5f) + 0.5f : newY + 0.5f
        );

        _RPosition = new Vector3(
            currentX + 0.5f,
            0,
            adjValues.hasRight ? Mathf.Lerp(newY, adjValues.rightY, .5f) + 0.5f : newY + 0.5f
        );

        if(!firstSet) {
            visualPoint.transform.localPosition = _position;
            leftPoint.transform.localPosition = _LPosition;
            rightPoint.transform.localPosition = _RPosition;
            firstSet = true;
        }

        _basePosition = visualPoint.transform.localPosition;
        _baseLPosition = leftPoint.transform.localPosition;
        _baseRPosition = rightPoint.transform.localPosition;

        if(WaveMovement != null) StopCoroutine(WaveMovement);
        WaveMovement = StartCoroutine(MoveWave());
    }

    IEnumerator MoveWave() {
        float timeElapsed = 0f;
        float lerpDuration = 1f;

        while(timeElapsed < lerpDuration) {
            visualPoint.transform.localPosition = Vector3.Lerp(_basePosition, _position, timeElapsed / lerpDuration);
            leftPoint.transform.localPosition = Vector3.Lerp(_baseLPosition, _LPosition, timeElapsed / lerpDuration);
            rightPoint.transform.localPosition = Vector3.Lerp(_baseRPosition, _RPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
