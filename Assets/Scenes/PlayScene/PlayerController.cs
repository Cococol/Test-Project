using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace consoleCommand
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("Total")]
        public int Level = 0;
        public int TotalHealth = 100;
        public int TotalMana = 100;
        public int TotalAttack = 60;
        public int TotalDefence = 20;

        [Header("Exp stuff")]
        public float Exp = 0;
        public float ExpGoal = 100;
        public float RemainingExp;

        [Header("Changing Attributes")]
        public int Health;
        public int Mana;
        public int Attack;
        public int Defence;

        [Header("Gameobjects")]
        public Slider expSlider;
        Vector3 velocity;
        public float speed = 5.0f;
        public Rigidbody rb;
        #endregion

        private void Start()
        {
            //expSlider.value = expSliderCalculator();
            rb = GetComponent<Rigidbody>();
            Exp = RemainingExp;
        }

        void Update()
        {
            Movement();

            if (Exp >= ExpGoal)
            {
                Level++;
                RemainingExp = Exp - ExpGoal;
                ExpGoal *= 1.5f;
                Exp = 0;
                Exp = +RemainingExp;
                StatsUpdate();
                RemainingExp = 0;
                expSlider.value = expSliderCalculator();
            }
        }

        public void Movement()
        {
            #region test movement
            velocity = rb.velocity;

            if (Input.GetKey(KeyCode.D))
            {
                velocity.x = 5;
            }
            else if(Input.GetKeyUp(KeyCode.D))
            {
                velocity.x = 0;
            }

            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -5;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                velocity.x = 0;
            }

            if (Input.GetKey(KeyCode.W))
            {
                velocity.z = 5;
            }
            else if(Input.GetKeyUp(KeyCode.W))
            {
                velocity.z = 0;
            }

            if (Input.GetKey(KeyCode.S))
            {
                velocity.z = -5;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                velocity.z = 0;
            }

            rb.velocity = velocity;
            #endregion
        }

        public void OnExpGain()
        {
            //put If statement here after fixing stuff.
        }

        public void expPress()
        {
            Exp += 100;
            expSlider.value = expSliderCalculator();
        }

        void StatsUpdate()
        {
            Health = 0;
            Mana = 0;
            Attack = 0;
            Defence = 0;

            TotalHealth = 100;
            TotalMana = 100;
            TotalAttack = 60;
            TotalDefence = 20;

            Health = 50 * Level;
            Mana = 20 * Level;
            Attack = 2 * Level;
            Defence = 3 * Level;

            TotalHealth += Health;
            TotalMana += Mana;
            TotalAttack += Attack;
            TotalDefence += Defence;
        }

        public float expSliderCalculator()
        {
            return Exp / ExpGoal;
        }

    }
}
