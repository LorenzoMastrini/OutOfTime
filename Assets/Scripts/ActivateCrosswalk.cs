using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCrosswalk : MonoBehaviour, IInteractable
{
    [SerializeField] List<GameObject> objectsList = new List<GameObject>();
    [SerializeField] List<GameObject> carSpawners = new List<GameObject>();
    [SerializeField] List<CarMovement> carsAlreadyInScene = new List<CarMovement>();
    [SerializeField] GameObject otherActivator;
    [SerializeField] float waitTime = 5f;

    [Header("Materials")]
    MeshRenderer meshRenderer;
    Material[] mats;
    [SerializeField] Material redLightActivator;
    [SerializeField] Material greenLightActivator;
    [SerializeField] Material redArrows;
    [SerializeField] Material greenArrows;

    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; } =
        "The famous \"Zeword\" stoplight: it freezes traffic so you can safely walk through it";



    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mats = meshRenderer.materials;
        meshRenderer.materials = mats;
    }



    // When user interact, start the movement
    public void Interact()
    {
        // Avoid activation if already active
        foreach (GameObject obj in objectsList)
        {
            if (objectsList[0].GetComponent<Scroll>().enabled || objectsList[1].GetComponent<Scroll>().enabled)
                return;
        }

        if (sound != null)
        {
            SoundManager.instance.PlaySound(sound, transform, 0.7f, 1.2f);
        }

        StartCoroutine(ChangeMaterial());

        if (objectsList.Count > 0)
        {
            StartCoroutine(ActivateScroll());
        }

        if (carSpawners.Count > 0)
        {
            StartCoroutine(StopCarsMovement());
        }
    }



    IEnumerator ChangeMaterial()
    {
        // Set green light
        mats[1] = greenLightActivator;
        // This activator
        meshRenderer.materials = mats;
        // Activator on the othrer side of the road
        otherActivator.GetComponent<MeshRenderer>().materials = mats;

        yield return new WaitForSeconds(waitTime);

        // Set red light back
        mats[1] = redLightActivator;
        // This activator
        meshRenderer.materials = mats;
        // Activator on the othrer side of the road
        otherActivator.GetComponent<MeshRenderer>().materials = mats;
    }



    IEnumerator ActivateScroll()
    {
        if (objectsList.Count <= 0)
            yield break;

        // Activate scroll movement
        objectsList[0].GetComponent<Scroll>().enabled = true;
        objectsList[1].GetComponent<Scroll>().enabled = true;

        // Change materials to green
        objectsList[0].GetComponent<MeshRenderer>().material = greenArrows;
        objectsList[1].GetComponent<MeshRenderer>().material = greenArrows;

        yield return new WaitForSeconds(waitTime);

        // Deactivate scroll movement after 5 seconds
        objectsList[0].GetComponent<Scroll>().enabled = false;
        objectsList[1].GetComponent<Scroll>().enabled = false;

        // Change materials back to red
        objectsList[0].GetComponent<MeshRenderer>().material = redArrows;
        objectsList[1].GetComponent<MeshRenderer>().material = redArrows;
    }



    IEnumerator StopCarsMovement()
    {
        // Stop car spawning
        carSpawners[0].GetComponent<CarSpawner>().enabled = false;
        carSpawners[1].GetComponent<CarSpawner>().enabled = false;
        
        // Deactivate global colliders
        BoxCollider normalCollider = carSpawners[0].GetComponents<BoxCollider>()[0];
        BoxCollider triggerCollider = carSpawners[0].GetComponents<BoxCollider>()[1];
        normalCollider.enabled = false;
        triggerCollider.enabled = false;

        // Deactivate movement script of the cars already spawned
        foreach (CarMovement carMovement in carSpawners[0].GetComponentsInChildren<CarMovement>())
        {
            carMovement.enabled = false;
        }

        // Second spawner
        foreach (CarMovement carMovement in carSpawners[1].GetComponentsInChildren<CarMovement>())
        {
            carMovement.enabled = false;
        }

        // Cars already in scene (not created by spawners)
        foreach (CarMovement carMovement in carsAlreadyInScene)
        {
            carMovement.enabled = false;
        }

        yield return new WaitForSeconds(waitTime);

        // Reactivate colliders
        normalCollider.enabled = true;
        triggerCollider.enabled = true;

        // Restart car spawning
        carSpawners[0].GetComponent<CarSpawner>().enabled = true;
        carSpawners[1].GetComponent<CarSpawner>().enabled = true;

        // Reactivate movement of the cars
        foreach (CarMovement carMovement in carSpawners[0].GetComponentsInChildren<CarMovement>())
        {
            carMovement.enabled = true;
        }

        // Second spawner
        foreach (CarMovement carMovement in carSpawners[1].GetComponentsInChildren<CarMovement>())
        {
            carMovement.enabled = true;
        }

        // Cars already in scene (not created by spawners)
        foreach (CarMovement carMovement in carsAlreadyInScene)
        {
            carMovement.enabled = true;
        }
    }



    private void OnDisable()
    {
        // Avoid errore if object is disabled
        if (meshRenderer == null)
            return;

        ResetCrosswalk();
        StopAllCoroutines();
    }



    // Set materials back to red and reactivate car movement
    private void ResetCrosswalk()
    {
        // Semafori
        mats[1] = redLightActivator;

        if (meshRenderer != null)
            meshRenderer.materials = mats;

        if (otherActivator != null)
            otherActivator.GetComponent<MeshRenderer>().materials = mats;

        // Frecce
        if (objectsList.Count >= 2)
        {
            objectsList[0].GetComponent<Scroll>().enabled = false;
            objectsList[1].GetComponent<Scroll>().enabled = false;

            objectsList[0].GetComponent<MeshRenderer>().material = redArrows;
            objectsList[1].GetComponent<MeshRenderer>().material = redArrows;
        }

        // Auto
        if (carSpawners.Count >= 2)
        {
            // Deactivate global colliders
            BoxCollider normalCollider = carSpawners[0].GetComponents<BoxCollider>()[0];
            BoxCollider triggerCollider = carSpawners[0].GetComponents<BoxCollider>()[1];
            normalCollider.enabled = true;
            triggerCollider.enabled = true;

            carSpawners[0].GetComponent<CarSpawner>().enabled = true;
            carSpawners[1].GetComponent<CarSpawner>().enabled = true;

            foreach (CarMovement car in carSpawners[0].GetComponentsInChildren<CarMovement>())
                car.enabled = true;

            foreach (CarMovement car in carSpawners[1].GetComponentsInChildren<CarMovement>())
                car.enabled = true;
        }

        foreach (CarMovement car in carsAlreadyInScene)
        {
            if (car != null)
                car.enabled = true;
        }
    }
}