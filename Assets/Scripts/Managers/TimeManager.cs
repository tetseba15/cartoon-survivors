using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public event Action OnTimeUp; 

    [Header("Win Condition")]
    [SerializeField] private float targetMinutes = 7f; 
    private float targetSeconds;

    private float elapsedTime = 0f;

    public float ElapsedTime => elapsedTime;

    private bool isTimeReached = false;

    public int Minutes => Mathf.FloorToInt(elapsedTime / 60);
    public int Seconds => Mathf.FloorToInt(elapsedTime % 60);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        targetSeconds = targetMinutes * 60f; 
    }

    private void Update()
    {
        if (isTimeReached || GameManager.Instance.IsGameOver) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= targetSeconds && !isTimeReached)
        {
            isTimeReached = true;
            OnTimeUp?.Invoke(); 
        }
    }
}