using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldThePlayer : MonoBehaviour {
    [Tooltip("The number of seconds that the shield remains active")] [SerializeField] float duration;
    [Tooltip("")]
    [SerializeField]
    float minTimeRandom;

    [SerializeField]
    float maxTimeRandom;

    [SerializeField]
    float maxTimeShow;

    [SerializeField]
    GameObject shield; // המגן שמופיע על השחקן

    void Start()
    {
        InvokeRepeating("ChangePosition", 0, Random.Range(minTimeRandom, maxTimeRandom)); // calls ChangePosition() every ... secs
        shield.SetActive(false); // כיבוי המגן
    }

    void ChangePosition()
    {
        if ((int)Random.Range(0, maxTimeShow) == 0)
        {
            float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            transform.position = spawnPosition;
        }
        else
            transform.position = new Vector2(-10, -10);
    }

    // בודק האם משהו התנגש במגן
    private void OnTriggerEnter2D(Collider2D other) {
         if (other.tag == "Player") { // אם המשהו הזה הוא השחקן
            Debug.Log("Shield triggered by player");
            var destroyComponent = other.GetComponent<DestroyOnTrigger2D>();
            other.GetComponent<Animation>().Play(); // הפעלת האנימציה של העיגול סביב השחקן
            if (destroyComponent) {
                destroyComponent.StartCoroutine(ShieldTemporarily(destroyComponent));
                // NOTE: If you just call "StartCoroutine", then it will not work, 
                //       since the present object is destroyed!
                Destroy(gameObject);  // Destroy the shield itself - prevent double-use
            }
        } else {
            Debug.Log("Shield triggered by "+other.name);
        }
    }
    private IEnumerator ShieldTemporarily(DestroyOnTrigger2D destroyComponent) {
        shield.SetActive(true);
        destroyComponent.enabled = false;
        for (float i = duration; i > 0; i--) {
            Debug.Log("Shield: " + i + " seconds remaining!");
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Shield gone!");
        shield.SetActive(false);
        destroyComponent.enabled = true;
    }
}
