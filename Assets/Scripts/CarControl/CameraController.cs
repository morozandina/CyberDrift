using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace CarControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraPreset camerasPreset = new CameraPreset(); //Camera presets
        [SerializeField] private CarController targetCar;
        private float _sqrMinDistance;
        
        //The target point is calculated from velocity of car.
        private Vector3 TargetPoint
        {
            get
            {
                if (targetCar == null)
                    return transform.position;
                
                var result = targetCar.RB.velocity * camerasPreset.velocityMultiplier;
                result += targetCar.transform.position;
                result.y = 0;
                return result;
            }
        }
        
        private IEnumerator Start ()
        {
            while (targetCar == null)
            {
                yield return null;
            }

            UpdateActiveCamera();
            transform.position = TargetPoint;
        }
        
        private void Update ()
        {
            if (camerasPreset.enableRotation && (TargetPoint - transform.position).sqrMagnitude >= _sqrMinDistance)
            {
                Quaternion rotation = Quaternion.LookRotation (TargetPoint - transform.position, Vector3.up);
                camerasPreset.cameraHolder.rotation = Quaternion.Lerp (camerasPreset.cameraHolder.rotation, rotation, Time.deltaTime * camerasPreset.setRotationSpeed);
            }

            transform.position = Vector3.LerpUnclamped (transform.position, TargetPoint, Time.deltaTime * camerasPreset.setPositionSpeed);
        }
        
        private void UpdateActiveCamera ()
        {
            _sqrMinDistance = camerasPreset.minDistanceForRotation * 2;

            if (!camerasPreset.enableRotation ||
                !((TargetPoint - transform.position).sqrMagnitude >= _sqrMinDistance)) return;
            
            var rotation = Quaternion.LookRotation (TargetPoint - transform.position, Vector3.up);
            camerasPreset.cameraHolder.rotation = rotation;
        }
        
        
        [System.Serializable]
        private class CameraPreset
        {
            public Transform cameraHolder; //Parent fo camera.
            public float setPositionSpeed = 1; //Change position speed.
            public float velocityMultiplier; //Velocity of car multiplier.

            public bool enableRotation;
            public float minDistanceForRotation = 0.1f; //Min distance for potation, To avoid uncontrolled rotation.
            public float setRotationSpeed = 1; //Change rotation speed.
        }
    }
}