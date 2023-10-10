using UnityEngine;

public class Brick : ObserverSubject
{
    // Called whenever a trigger has entered this objects BoxCollider2D. The value 'col' is the Collider2D object that has interacted with this one
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ball")
        {
            NotifyObservers(GameUpdates.BrickDestroyed, gameObject.GetInstanceID());
            Destroy(gameObject);
        }
    }
}