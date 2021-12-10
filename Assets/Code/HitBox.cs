using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {
    [SerializeField] private MeshRenderer _renderer;

    [SerializeField] private GameObject _x;
    [SerializeField] private GameObject _o;

    private int _type = -1;
    public int Type => _type;

    private bool _markerPlaced = false;

    private void Start() {
        _renderer.enabled = false;
    }

    private void OnMouseOver() {
        if (GameManager.Instance.GameEnd || GameManager.Instance.DevMode || _markerPlaced) {
            return;
        }

        _renderer.enabled = true;
    }

    private void OnMouseExit() {
        _renderer.enabled = false;
    }

    private void OnMouseUpAsButton() {
        if (GameManager.Instance.GameEnd || GameManager.Instance.DevMode || _markerPlaced) {
            return;
        }

        _renderer.enabled = false;
        _markerPlaced = true;

        _type = GameManager.Instance.Turn;
        var markerToSpawn = _type == 0 ? _x : _o;
        Instantiate(markerToSpawn, transform);

        GameManager.Instance.MoveMade();

        Debug.Log("compile");
    }
}