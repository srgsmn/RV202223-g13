using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;

public class StartContentManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cards; //0-2 demos, 3 (length-1) browse
    [SerializeField] private int activeCard = -1;

    [SerializeField] private List<Animator> animators;

    private void Awake()
    {
        EventSubscriber();

        foreach (GameObject go in cards)
        {
            animators.Add(go.GetComponent<Animator>());
        }
    }

    private void OnDisable()
    {
        if(activeCard>0 && activeCard<animators.Count)
        animators[activeCard].SetBool("open", !animators[activeCard].GetBool("open"));

        activeCard = -1;
    }

    private void OnDestroy()
    {
        EventSubscriber(false);
    }

    private void EventSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            StartMenuButton.OnEnvSelect += DisplayCard;
        }
        else
        {
            // Events to unsubscribe
            StartMenuButton.OnEnvSelect -= DisplayCard;
        }
    }

    private void DisplayCard(Environments env)
    {
        switch (env)
        {
            case Environments.Demo1:
                //newCard = 0;
                StartCoroutine(ChangeCardState(0));

                break;

            case Environments.Demo2:
                //newCard = 1;
                StartCoroutine(ChangeCardState(1));

                break;

            case Environments.Demo3:
                //newCard = 2;
                StartCoroutine(ChangeCardState(2));

                break;

            case Environments.Browsed:
                //newCard = 3;
                StartCoroutine(ChangeCardState(3));

                break;
        }

        /*
        if (activeCard != -1)
            StartCoroutine(ChangeCardState(activeCard));

        activeCard = newCard;

        ChangeCardState(activeCard);
        */
    }

    IEnumerator ChangeCardState(int newCard)
    {
        float stw = 0f;
        if (activeCard != -1)
        {
            animators[activeCard].SetBool("open", !animators[activeCard].GetBool("open"));
            stw = .5f;
        }

        yield return new WaitForSeconds(stw);

        animators[newCard].SetBool("open", !animators[newCard].GetBool("open"));

        activeCard = newCard;
    }
}