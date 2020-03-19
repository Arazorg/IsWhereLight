using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersSpec : MonoBehaviour
{
    public struct Character
    {     
        public int maxMane;
        public int maxHealth;
        public string startGun;
        public string[] skins;
        public int price;
    }

    public Dictionary<string, Character> characters = new Dictionary<string, Character>();

    void Awake()
    {
        characters.Add("Knight", new Character()
        {
            maxMane = 100,
            maxHealth = 150,
            startGun = "Pistol",
            skins = new string[] { "Knight" }
        });

        characters.Add("Mage", new Character()
        {
            maxMane = 200,
            maxHealth = 50,
            startGun = "Staff",
            skins = new string[] { "Mage" },
            price = 200
        });
    }
}
