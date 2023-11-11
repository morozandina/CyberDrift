using System;
using UnityEngine;

namespace CarControl
{
    public class DriftControl : MonoBehaviour
    {
        private static IndicatorState IndicatorState => IndicatorState.Instance;
        private CarController _carController;

        private bool _isDrift = false;

        private void Awake()
        {
            _carController = GetComponent<CarController>();
        }

        private void Start()
        {
            if (IndicatorState)
                IndicatorState.currentCar = _carController;
        }

        private void FixedUpdate()
        {
            if (_carController.VelocityAngle is > 35 or < -35 && _carController.CurrentMaxSlip > .7f) // Drift
            {
                if (IndicatorState)
                    IndicatorState.RefreshScore();
                if (!_isDrift)
                    Drift();
            }
            else
            {
                if (_isDrift)
                    CancelDrift();
            }
        }

        private void Drift()
        {
            _isDrift = true;
            // TODO: Start Drifting Event
            if (IndicatorState)
                IndicatorState.ShowIndicators();
        }

        private void CancelDrift()
        {
            _isDrift = false;
            // TODO: Stop Drifting Event
            if (IndicatorState)
                IndicatorState.StopTimer();
        }
    }
}
