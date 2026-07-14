using System.Collections;
using UnityEngine;

public class Jetpack : Item
{
    private FirstPersonMovement _player;
    private Transform _playerTransform;

    [Header("Jetpack Settings")]
    [SerializeField] private float _height = 4f;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _speed = 10f;

    private bool _isFlying = false;
    private Inventory playerInventory;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        _player = FindAnyObjectByType<FirstPersonMovement>();
        _playerTransform = _player.transform;
        playerInventory = FindAnyObjectByType<Inventory>();
        audioSource = GetComponent<AudioSource>();
    }

    

    void Start()
    {
        TutorialManager.instance.TriggerTutorial(TutorialStep.Use);
    }



    public override void Use()
    {
        if (_isFlying || _player == null) return;

        GetComponent<AudioClip>();

        StartCoroutine(JetpackRoutine());
    }



    private IEnumerator JetpackRoutine()
    {
        _isFlying = true;
        
        audioSource.Play();

        float startY = _playerTransform.position.y;
        float targetY = startY + _height;

        while (_playerTransform.position.y < targetY)
        {
            _player.SetVerticalVelocity(_speed);

            yield return null;
        }

        float hoverElapsed = 0f;

        while (hoverElapsed < _duration)
        {
            // annulla completamente gravità e caduta
            _player.SetVerticalVelocity(0f);

            hoverElapsed += Time.deltaTime;

            yield return null;
        }

        _isFlying = false;

        // Unequip and destroy the jetpack
        playerInventory.RemoveEquippedItem();
    }
}
