using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CarControl
{
    [Serializable]
    public class CarData
    {
        public int score;
        public int wave;
        public int maxScore;
        public void SetMaxScore() => maxScore = Mathf.Max(maxScore, score);
    }
    public class IndicatorState : Singleton<IndicatorState>
    {
        public CarData carData = new (){score = 1, wave = 1, maxScore = 1};
        [HideInInspector] public CarController currentCar;
        [SerializeField] private Material indicatorMaterial;
        [Header("Grade")]
        [SerializeField] private Transform gradeParent;
        [SerializeField] private TextMeshPro gradeText;
        private const string GradeSymbol = "°";

        [Space(15)] [Header("Combo")]
        [SerializeField] private Material comboMaterial;
        [SerializeField] private TextMeshPro score, comboX;
        [SerializeField] private float comboTime;
        
        private static readonly int Fade = Shader.PropertyToID("_Fade");
        
        protected override void AwakeSingleton()
        {
            comboMaterial.SetFloat(Fade, 0.025f);
            
            carData.wave = 1;
            carData.score = 1;
            
            indicatorMaterial.SetFloat(Fade, 0f);
            gradeParent.SetActive(false);
            gradeText.SetActive(false);
            score.transform.parent.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (!currentCar)
                return;
            
            if (gradeText.gameObject.activeSelf)
                RefreshGrade(Mathf.Abs((int)currentCar.VelocityAngle));
        }

        public void ShowIndicators()
        {
            if (_comboTimer != null)
            {
                StopCoroutine(_comboTimer);
                comboMaterial.SetFloat(Fade, 1);
                RefreshWave();
                return;
            }
            
            gradeText.text = 0 + GradeSymbol;
            comboX.text = "X" + carData.wave;
            score.text = carData.score.ToString();
            
            indicatorMaterial.SetFloat(Fade, 0f);

            gradeParent.SetActive(true);
            gradeText.SetActive(true);
            score.transform.parent.gameObject.SetActive(true);
            
            comboMaterial.DOFloat(1f, Fade, .5f);
            indicatorMaterial.DOFloat(1f, Fade, .7f);
        }

        private void HideIndicators()
        {
            indicatorMaterial.DOFloat(0, Fade, .7f).OnComplete(() =>
            {
                gradeParent.SetActive(false);
                gradeText.SetActive(false);
                score.transform.parent.gameObject.SetActive(false);
            });
            
            carData.SetMaxScore();
            carData.wave = 1;
            carData.score = 1;
            _comboTimer = null;
        }

        private void RefreshGrade(int grade)
        {
            gradeText.text = grade + GradeSymbol;
        }

        public void RefreshScore()
        {
            carData.score += 1 * carData.wave;
            score.text = carData.score.ToString();
        }

        private void RefreshWave()
        {
            carData.wave++;
            comboX.text = "X" + carData.wave;
        }

        public void StopTimer()
        {
            if (_comboTimer != null)
                StopCoroutine(_comboTimer);
            _comboTimer = StartCoroutine(ComboTimer());
        }
        
        // UI: Control Combo Timer
        private Coroutine _comboTimer;
        private IEnumerator ComboTimer() // Time for combo
        {
            var totalTime = 0f;
            comboMaterial.SetFloat(Fade, 1);

            while (totalTime <= comboTime)
            {
                comboMaterial.SetFloat(Fade, 1 - totalTime / comboTime);
                totalTime += Time.deltaTime;
                yield return null;
            }
            comboMaterial.SetFloat(Fade, 0.025f);
            HideIndicators();
        }
    }
}
