using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace HomeWork5;

public class Quadcopter : IFlyingRobot, IChargeable
{
    private readonly List<string> _components;

    public Quadcopter()
    {
        _components = new() {"rotor1", "rotor2", "rotor3", "rotor4"};
    }
    
    public void Charge()
    {
        Console.WriteLine("Charging...");
        Thread.Sleep(TimeSpan.FromSeconds(3));
        Console.WriteLine("Charged!");
    }

    string IChargeable.GetInfo()
    {
        return "I am IChargeable from Quadcopter";
    }

    string IRobot.GetInfo()
    {
        return "I am IRobot from Quadcopter";
    }

    public IList GetComponents() => _components;
}