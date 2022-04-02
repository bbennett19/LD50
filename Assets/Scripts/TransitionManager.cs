using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [SerializeField]
    private GameObject _computerScreenLoading;
    [SerializeField]
    private GameObject _bigScreenLoading;

    private void Awake()
    {
        Instance = this;
    }

    public void ComputerScreenTransition(GameObject currentScreen, GameObject newScreen)
    {
        StartCoroutine(ExecuteComputerScreenTransition(currentScreen, newScreen));
    }

    private IEnumerator ExecuteComputerScreenTransition(GameObject currentScreen, GameObject newScreen)
    {
        currentScreen.SetActive(false);
        _computerScreenLoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        _computerScreenLoading.SetActive(false);
        newScreen.SetActive(true);
    }
}
