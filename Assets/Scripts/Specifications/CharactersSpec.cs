using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpec : MonoBehaviour
{
    public struct Character
    {     
        public int maxMane;
        public int maxHealth;
        public string startWeapon;
        public string[] skins;
        public int price;
    }

    public Dictionary<string, Character> characters = new Dictionary<string, Character>();

    void Awake()
    {
        characters.Add("Knight", new Character()
        {
            maxMane = 2500,
            maxHealth = 150,
            startWeapon = "Pistol",
            skins = new string[] { "Knight" }
        });

        characters.Add("Mage", new Character()
        {
            maxMane = 200,
            maxHealth = 50,
            startWeapon = "Staff",
            skins = new string[] { "Mage" },
            price = 200
        });
    }
}
