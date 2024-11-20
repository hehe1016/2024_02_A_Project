using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SurvivalStats : MonoBehaviour
{
    [Header("Hunger Setting")]
    public float maxHunger = 100;   //�ִ� ��ⷮ
    public float currentHunger;   //���� ��ⷮ
    public float HungerDecreaseRate = 1;   //�ʴ� ��� ���ҷ�

    [Header("Space Suit Settings")]
    public float maxSuitDurability = 100;   //�ִ� ���ֺ� ������
    public float currentSuitDurability;   //���� ���ֺ� ������
    public float havestingDamage = 5.0f;   //������ ���ֺ� ������
    public float craftingDamage = 3.0f;   //���۽� ���ֺ� ������

    private bool isGameOver = false;   //���� ���� ����
    private bool isPaused = false;   //�Ͻ����� ����
    private float hungerTimer = 0;   //��� ���� Ÿ�̸�

    // Start is called before the first frame update
    void Start()
    {
        //���� ���۽� ���ݵ��� �ִ��� ���·� ����
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver || isPaused) return;

        hungerTimer += Time.deltaTime;
        if (hungerTimer >= 1.0f) ;       //��� üũ (1�ʸ���)
        {
            currentHunger = Mathf.Max(0, currentHunger - HungerDecreaseRate);
            hungerTimer = 0.0f;

            CheckDeath();
        }
    }

    public void DamageHarvesting()   //������ ������ ���ֺ� ������
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - havestingDamage);  //0 �� ���Ϸ� �� �������� �Ѵ�.
        CheckDeath();
    }
    public void DamageCrafting()   //������ ���۽� ���ֺ� ������
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Max(0, currentSuitDurability - craftingDamage);  //0 �� ���Ϸ� �� �������� �Ѵ�.
        CheckDeath();
    }
    public void EatFood(float amount)     //���� ����� ��� ȸ��
    {
        if (isGameOver || isPaused) return;

        currentHunger = Mathf.Min(maxHunger, currentHunger + amount);   //100 ��ġ �̻� ���� �ʰ� ����

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"��� ȸ�� +{amount}", transform.position + Vector3.up);
        }
    }
    public void RepairSuit(float amount)     //���ֺ� ���� (ũ����Ż�� ������ ���� ŰƮ ���)
    {
        if (isGameOver || isPaused) return;

        currentSuitDurability = Mathf.Min(maxSuitDurability, currentSuitDurability + amount);   //100 ��ġ �̻� ���� �ʰ� ����

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show($"���ֺ� ���� +{amount}", transform.position + Vector3.up);
        }
    }

    private void CheckDeath()   //�÷��̾� ��� ó�� üũ �Լ�
    {
        if (currentHunger <= 0 || currentSuitDurability <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()   //�÷��̾� ��� �Լ�
    {
        isGameOver = true;
        Debug.Log("�÷��̾� ���!");
        //TODO : ��� ó�� �߰� (���� ����UI, ������ ���)
    }

    public float GetHungerPercentage()
    {
        return (currentHunger / maxHunger) * 100;   //�����% ���� �Լ�
    }

    public float GetSuitDurabilityPercentage()
    {
        return (currentSuitDurability / maxSuitDurability) * 100;   //��Ʈ% ���� �Լ�
    }

    public bool IsGameOver()   //���� ���� Ȯ�� �Լ�
    {
        return isGameOver;
    }

    public void ResetStates()   //���� �Լ� �ۼ� (������ �ʱ�ȭ �뵵)
    {
        isGameOver = false;
        isPaused = false;
        currentHunger = maxHunger;
        currentSuitDurability = maxSuitDurability;
        hungerTimer = 0;
    }
}
