using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

[System.Serializable]
public class Condition
{
    [HideInInspector]
    public float curValue;
    public float maxValue;
    public float startValue;
    public float regenRate; // 회복률
    public float decayRate; // 감소율
    public Image uiBar;

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }
    public void Substract(float amount)
    {
        curValue = Mathf.Min(curValue - amount, maxValue);
    }
    
    public float GetPaercentage()
    {
        return curValue / maxValue;
    }
}

public class PlayerConditions : MonoBehaviour, IDamagable
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public float noHungerHealthDecay;

    public UnityEvent onTakeDamage;

    void Start()
    {
        health.curValue = health.startValue;
        hunger.curValue = hunger.startValue;
        stamina.curValue = stamina.startValue;
    }

    void Update()
    {
        stamina.Add(stamina.regenRate * Time.deltaTime);

        if (hunger.curValue > 0.0f)
        {
            hunger.Substract(hunger.decayRate * Time.deltaTime);
        }
        else
        {
            hunger.curValue = 0f;
            health.Substract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0.0f)
        {
            Die();
        }

        health.uiBar.fillAmount = health.GetPaercentage();
        hunger.uiBar.fillAmount = hunger.GetPaercentage();
        stamina.uiBar.fillAmount = stamina.GetPaercentage();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Substract(amount);
        return true;
    }

    public void Die()
    {
        Debug.Log("Player Die");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Substract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
