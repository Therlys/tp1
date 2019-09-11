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
        private Coroutine statisticToDatabaseCoroutine;
        private bool gameRunning = true;
        private void Awake()
        {
            DOTween.Init(false, false, LogBehaviour.ErrorsOnly);
            DOTween.SetTweensCapacity(200, 125);
        }
        
        private void Start()
        {
            statisticToDatabaseCoroutine = StartCoroutine(GenerateStatisticInDatabase());
        }

        private void OnDestroy()
        {
            StopCoroutine(statisticToDatabaseCoroutine);
        }

        private void Update()
        {
            if (Input.GetKeyDown(timeScaleUpKey))
                Time.timeScale += timeScaleIncrement;
            if (Input.GetKeyDown(timeScaleDownKey))
                Time.timeScale -= timeScaleIncrement;
        }

        private IEnumerator GenerateStatisticInDatabase()
        {
            while (gameRunning)
            {
                yield return new WaitForSeconds(1);
                Finder.GetStatisticGenerator().AddStatisticToDatabase();
            }
        }

        private void OnApplicationQuit()
        {
            gameRunning = false;
        }
    }
}