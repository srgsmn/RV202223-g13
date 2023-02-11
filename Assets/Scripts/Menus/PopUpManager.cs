using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private void Awake()
    {
        EventsSubscriber();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            PopUpButton.DeleteGO += HidePopUp;
            PopUpButton.MoveGO += HidePopUp;
        }
        else
        {
            // Events to unsubscribe
            PopUpButton.DeleteGO -= HidePopUp;
            PopUpButton.MoveGO -= HidePopUp;
        }
    }

    private void HidePopUp()
    {
        //gameObject.SetActive(false);  //Quale è meglio???
        Destroy(gameObject);
    }
}
