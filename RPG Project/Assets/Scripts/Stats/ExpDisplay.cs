using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExpDisplay : MonoBehaviour
    {
        Experiance experiance;

        private void Awake()
        {
            experiance = GameObject.FindWithTag("Player").GetComponent<Experiance>();
        }

        private void Update()
        {
            GetComponent<Text>().text = experiance.GetExperiancePoints().ToString("F0");
        }
    }
}