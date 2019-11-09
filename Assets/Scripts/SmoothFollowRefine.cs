using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollowRefine : MonoBehaviour
	{

		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

        private float leftBound = -17.5f;
        private float rightBound = 17.5f;
        private float bottomBound = -3;
        private float topBound = 2;

		// Use this for initialization
		void Start() {
            transform.position = new Vector3(0, 0, -10);
        }

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;


            float newx = target.transform.position.x;
            float newy = target.transform.position.y;

            if (target.transform.position.x <= leftBound || target.transform.position.x >= rightBound
                || target.transform.position.y <= bottomBound || target.transform.position.y >= topBound)
            {
                if (target.transform.position.x <= leftBound)
                {
                    newx = leftBound;
                    //transform.position = new Vector3(leftBound, target.transform.position.y, transform.position.z);
                }

                else if (target.transform.position.x >= rightBound)
                {
                    newx = rightBound;
                    //transform.position = new Vector3(rightBound, target.transform.position.y, transform.position.z);
                }

                else if (target.transform.position.y <= bottomBound)
                {
                    newy = bottomBound;
                    //transform.position = new Vector3(target.transform.position.x, bottomBound, transform.position.z);
                }

                else if (target.transform.position.y >= topBound)
                {
                    newy = topBound;
                    //transform.position = new Vector3(target.transform.position.x, topBound, transform.position.z);
                }
                transform.position = new Vector3(newx, newy, transform.position.z);
      
            }

            else
            {
                // Calculate the current rotation angles
                var wantedRotationAngle = target.eulerAngles.y;
                var wantedHeight = target.position.y + height;

                var currentRotationAngle = transform.eulerAngles.y;
                var currentHeight = transform.position.y;

                // Damp the rotation around the y-axis
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

                // Damp the height
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

                // Convert the angle into a rotation
                var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

                // Set the position of the camera on the x-z plane to:
                // distance meters behind the target
                transform.position = target.position;
                transform.position -= currentRotation * Vector3.forward * distance;

                // Set the height of the camera
                transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

                // Always look at the target
                transform.LookAt(target);
            }
		}
	}
}