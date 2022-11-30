using UnityEngine;

namespace DefaultNamespace
{
    public class Border : MonoBehaviour
    {
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Collision with object - ", col);
        }
        
    }
}