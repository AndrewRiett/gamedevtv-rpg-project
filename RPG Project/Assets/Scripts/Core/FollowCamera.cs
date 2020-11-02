using UnityEngine;

namespace PRG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target = null;
        [SerializeField] float camSpeed = 0;

        void LateUpdate()
        {
            // moving the camera slightly within LateUpdate() to load the player's animation in advance
            transform.position = Vector3.Lerp(transform.position, target.position, camSpeed * Time.deltaTime);
        }
    }
}
