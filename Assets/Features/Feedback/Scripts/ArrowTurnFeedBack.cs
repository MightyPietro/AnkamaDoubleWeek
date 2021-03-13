﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTurnFeedBack : MonoBehaviour
{
    [SerializeField] private GameObject _selfGameObject;
    [SerializeField] private Transform _selfTransform;
    [SerializeField] private Animation _selfAnimation;
    [SerializeField] private MeshRenderer _selfRend;

    private Quaternion _startRotation;
    private Vector3 _startPosition;

    private void Start()
    {
        _startRotation = _selfTransform.rotation;
        _startPosition = _selfTransform.localPosition;
        SetActive(false);
    }

    private void Update()
    {
        /*Vector3 camPos = Camera.main.transform.position;
        Vector3 selfPos = _selfTransform.position;
        Vector3 lookAtPos = new Vector3(camPos.x - selfPos.x, selfPos.y, camPos.z - selfPos.z);
        _selfTransform.rotation = Quaternion.LookRotation(lookAtPos);    */

        _selfTransform.rotation = _startRotation;
    }

    public void SetColor(Color color)
    {
        _selfGameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
    }

    public void SetActive(bool enable)
    {
        _selfRend.enabled = enable;
        if (enable)
        {
            _selfTransform.position = _startPosition;
            _selfAnimation.Play();
        }
        else
        {
            _selfTransform.localPosition = _startPosition + new Vector3(0, -0.5f,0);
            _selfAnimation.Stop();
        }

    }
}
