namespace HomeWork5;

public interface IFlyingRobot: IRobot
{
    public new string GetRobotType() => "I am a flying robot.";
}