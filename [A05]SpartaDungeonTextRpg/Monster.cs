using _A05_SpartaDungeonTextRpg;
// Monster.cs
using System;
using System.Reflection.Emit;

public class Monster
{
    public string Name { get; }
    public double Hp { get; private set; }
    public int Attack { get; }
    public int Level { get; }
    public bool IsDead => Hp <= 0;

    public Monster(string name, int health, int attack ,  int level)
    {
        Name = name;
        Hp = health;
        Attack = attack;
        Level = level;
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp < 0)
            Hp = 0;
    }
}
