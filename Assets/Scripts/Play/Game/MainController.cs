using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private KeyCode timeScaleUpKey = KeyCode.KeypadPlus;
        [SerializeField] private KeyCode timeScaleDownKey = KeyCode.KeypadMinus;
        [SerializeField] private float timeScaleIncrement = 1;
        private void Awake()
        {
            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 125);
        }

        private void Update()
        {
            if (Input.GetKeyDown(timeScaleUpKey))
                Time.timeScale += timeScaleIncrement;
            if (Input.GetKeyDown(timeScaleDownKey))
                Time.timeScale -= timeScaleIncrement;
        }

    }
}