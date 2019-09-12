
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Mike Bédard
public class Statistic
{

    private int statistic_id;
    private int seed;
    private long timestamp;
    private int bunny_quantity;
    private int fox_quantity;
    private int bunny_death_quantity;
    private int fox_death_quantity;
    private int bunny_hunger_death_quantity;
    private int bunny_thirst_death_quantity;
    private int bunny_eaten_death_quantity;
    private int fox_hunger_death_quantity;
    private int fox_thirst_death_quantity;

    public int StatisticId
    {
        get => statistic_id;
        set => statistic_id = value;
    }

    public int Seed
    {
        get => seed;
        set => seed = value;
    }

    public long Timestamp
    {
        get => timestamp;
        set => timestamp = value;
    }

    public int BunnyQuantity
    {
        get => bunny_quantity;
        set => bunny_quantity = value;
    }

    public int FoxQuantity
    {
        get => fox_quantity;
        set => fox_quantity = value;
    }

    public int BunnyDeathQuantity
    {
        get => bunny_death_quantity;
        set => bunny_death_quantity = value;
    }

    public int FoxDeathQuantity
    {
        get => fox_death_quantity;
        set => fox_death_quantity = value;
    }

    public int BunnyHungerDeathQuantity
    {
        get => bunny_hunger_death_quantity;
        set => bunny_hunger_death_quantity = value;
    }

    public int BunnyThirstDeathQuantity
    {
        get => bunny_thirst_death_quantity;
        set => bunny_thirst_death_quantity = value;
    }

    public int BunnyEatenDeathQuantity
    {
        get => bunny_eaten_death_quantity;
        set => bunny_eaten_death_quantity = value;
    }

    public int FoxHungerDeathQuantity
    {
        get => fox_hunger_death_quantity;
        set => fox_hunger_death_quantity = value;
    }

    public int FoxThirstDeathQuantity
    {
        get => fox_thirst_death_quantity;
        set => fox_thirst_death_quantity = value;
    }

    public Statistic() : this(0,0,0,0,0,0,0,0,0,0,0,0)
    {
    }
    public Statistic(int statisticId, int seed, long timestamp, int bunnyQuantity, int foxQuantity, int bunnyDeathQuantity, int foxDeathQuantity, int bunnyHungerDeathQuantity, int bunnyThirstDeathQuantity, int bunnyEatenDeathQuantity, int foxHungerDeathQuantity, int foxThirstDeathQuantity)
    {
        statistic_id = statisticId;
        this.seed = seed;
        this.timestamp = timestamp;
        bunny_quantity = bunnyQuantity;
        fox_quantity = foxQuantity;
        bunny_death_quantity = bunnyDeathQuantity;
        fox_death_quantity = foxDeathQuantity;
        bunny_hunger_death_quantity = bunnyHungerDeathQuantity;
        bunny_thirst_death_quantity = bunnyThirstDeathQuantity;
        bunny_eaten_death_quantity = bunnyEatenDeathQuantity;
        fox_hunger_death_quantity = foxHungerDeathQuantity;
        fox_thirst_death_quantity = foxThirstDeathQuantity;
    }

    public Statistic(int seed, long timestamp, int bunnyQuantity, int foxQuantity, int bunnyDeathQuantity, int foxDeathQuantity, int bunnyHungerDeathQuantity, int bunnyThirstDeathQuantity, int bunnyEatenDeathQuantity, int foxHungerDeathQuantity, int foxThirstDeathQuantity) 
        : this(0, seed, timestamp, bunnyQuantity, foxQuantity, bunnyDeathQuantity, foxDeathQuantity,
            bunnyHungerDeathQuantity, bunnyThirstDeathQuantity, bunnyEatenDeathQuantity, foxHungerDeathQuantity,
            foxThirstDeathQuantity)
    {
    }
}

