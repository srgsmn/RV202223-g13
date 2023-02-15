using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGVideoSelector : MonoBehaviour
{
    [SerializeField] private CrossfadeController fader;

    private void Awake()
    {
        if (fader == null)
        {
            Debug.LogError($"{GetType().Name}.cs > Unlinked Crossfade Controller");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
