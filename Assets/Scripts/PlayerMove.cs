using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    
    [SerializeField] private Animator _animator;
    
    [SerializeField] private float _speed = 5f; // Скорость перемещения персонажа
    private bool facingRight = true; // Флаг для определения направления персонажа
    private Animator animator; // Ссылка на компонент Animator

    void Start()
    {
        // Получаем ссылку на компонент Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Получаем ввод от пользователя
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Создаем вектор движения
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);

        // Применяем движение к персонажу
        transform.Translate(movement * _speed * Time.deltaTime, Space.World);

        // Если персонаж движется влево и смотрит вправо, или наоборот, меняем направление
        if (moveHorizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            Flip();
        }

        // Устанавливаем переменную isRun в зависимости от движения
        bool isRunning = movement.magnitude > 0;
        animator.SetBool("isRun", isRunning);
    }

    // Функция для изменения направления персонажа
    void Flip()
    {
        // Меняем флаг направления
        facingRight = !facingRight;

        // Меняем масштаб по оси X, чтобы повернуть персонажа
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
