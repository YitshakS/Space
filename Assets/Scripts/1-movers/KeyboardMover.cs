using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This component moves its object when the player clicks the arrow keys.
 */
public class KeyboardMover: MonoBehaviour {
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 1f;

    void Update() {
        float horizontal = Input.GetAxis("Horizontal"); // +1 if right arrow is pushed, -1 if left arrow is pushed, 0 otherwise
        float vertical = Input.GetAxis("Vertical");     // +1 if up arrow is pushed, -1 if down arrow is pushed, 0 otherwise
        Vector3 movementVector = new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
        transform.position += movementVector;

        //get the screen position
        Vector3 scrPos = Camera.main.WorldToScreenPoint(transform.position);
        //Check if we are too far left
        if (scrPos.x < 0) TeleportRight(scrPos);
        //check if we are too far right
        if (scrPos.x > Screen.width) TeleportLeft(scrPos);


        // אם הגענו לגבול נעביר את החללית לצד השני
        //if (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, transform.position.y)).x)
        //    Debug.Log("right");
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector2(0, transform.position.y));
        //transform.Translate(movementVector);
        // NOTE: "Translate(movementVector)" uses relative coordinates - 
        //       it moves the object in the coordinate system of the object itself.
        // In contrast, "transform.position += movementVector" would use absolute coordinates -
        //       it moves the object in the coordinate system of the world.
        // It makes a difference only if the object is rotated.
    }
    
    // אם השחקן מתנגש בקיר
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Boom");
        if (other.tag == "Wall")
            transform.position = new Vector2(transform.position.x - Input.GetAxis("Horizontal"), transform.position.y - Input.GetAxis("Vertical"));
    }

    //TeleportRight moves character from it's current x position to x position 10 pixels left from the right edge of the screen   
    void TeleportRight(Vector3 scrPos)
    {

        //this is the position on the screen we want to move the character too, we only want to change it's x-coordinate
        Vector3 goalScrPos = new Vector3(Screen.width - 10, scrPos.y, scrPos.z);

        //translate goal screen position to world position
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(goalScrPos);

        //move player
        transform.position = targetWorldPos;

    }

    //TeleportRight moves character from it's current x position to x position 10 pixels left from the right edge of the screen   
    void TeleportLeft(Vector3 scrPos)
    {

        //this is the position on the screen we want to move the character too, we only want to change it's x-coordinate
        Vector3 goalScrPos = new Vector3(0 + 10, scrPos.y, scrPos.z);

        //translate goal screen position to world position
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(goalScrPos);

        //move player
        transform.position = targetWorldPos;

    }
}
