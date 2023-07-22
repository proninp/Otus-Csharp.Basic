using System.Collections;

namespace HomeWork5;

public interface IRobot
{
    public string GetInfo();
    public IList GetComponents();
    public string GetRobotType() => "I am a simple robot.";
}