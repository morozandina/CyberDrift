using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CarControl
{
    public class MobileCarControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public InterfaceAnimManager interfaceAnimManager;
        private bool _canControl;

        [Header("Components")]
        public RectTransform sphereForControl;
        public RectTransform parent;
        public Camera cam;
        
        [Header("Car")][Space(15)]
        public CarController controlledCar;
        
        // Controls
        private float _currentY, _currentX;

        public void OnPointerDown(PointerEventData eventData)
        {
            interfaceAnimManager.startAppear();
            _canControl = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            interfaceAnimManager.startDisappear();
            _canControl = false;
        }

        private void FixedUpdate()
        {
            if (!_canControl)
                return;

            var anchoredPosition = GetScreenPoint();
            _currentX = Mathf.Clamp(anchoredPosition.x / 100, -1, 1);
            _currentY = anchoredPosition.y < 0 ? -1 : 1;
            sphereForControl.anchoredPosition = anchoredPosition;
        }
        
        private void Update ()
        {
            if (!_canControl)
            {
                controlledCar.UpdateControls (_currentX, .1f, true);
                return;
            }

            // Apply control for controlled car.
            controlledCar.UpdateControls (_currentX, _currentY, false);
        }

        private Vector2 GetScreenPoint()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, cam, out var anchoredPosition);
            return anchoredPosition;
        }
    }
}
