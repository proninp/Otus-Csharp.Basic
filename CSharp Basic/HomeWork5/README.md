# �������� ������ �5

����������

## ����
� ���� �� �� ��������� ������ ������ �����������, �� ������������ � ���������� ������� �� ���������.

## ��������/��������� ���������� ���������� ��������� �������:
1. ������� ��������� IRobot � ��������� �������� string GetInfo() � List GetComponents(), � ����� string GetRobotType() � ��������� �����������, ������������ �������� "I am a simple robot.".
2. ������� ��������� IChargeable � �������� void Charge() � string GetInfo().
3. ������� ��������� IFlyingRobot ��� ��������� IRobot � ��������� ����������� GetRobotType(), ������������ ������ "I am a flying robot.".
4. ������� ����� Quadcopter, ����������� IFlyingRobot � IChargeable. � �� ������� ������ ����������� List _components = new List {"rotor1","rotor2","rotor3","rotor4"} � ���������� ��� �� ������ GetComponents().
5. ����������� ����� Charge() ������ ������ � ������� "Charging..." � ����� 3 ������� "Charged!". �������� � 3 ������� ����������� ����� Thread.Sleep(3000).
6. ����������� ��� ������ ����������� � ������. �� ����� ������ ���������� ���� "throw new NotImplementedException();"
7. � ��� �������� ����� �����, ������� ��� ������������� ��� ���������� ��������� �������.

## �������� ������
* ����� �1 - 3 �����;
* ����� �2 - 2 �����;
* ����� �3 - 2 �����;
* ����� �4 - 1 ����;
* ����� �5 - 1 ����;
* ����� �6 - 1 ����;

��� ����� ���������� 6 ������.

## �������

```cs
var quadcopter = new Quadcopter();

HomeWork1.Program.PrintConsole("********** Quadcopter **********");
quadcopter.Charge();
Console.WriteLine(string.Join(", ", ((List<string>)quadcopter.GetComponents()).ToArray()));
Console.WriteLine();

HomeWork1.Program.PrintConsole("********** IChargeable **********");
((IChargeable)quadcopter).Charge();
Console.WriteLine(((IChargeable)quadcopter).GetInfo());
Console.WriteLine();

HomeWork1.Program.PrintConsole("********** IFlyingRobot **********");
Console.WriteLine(((IFlyingRobot)quadcopter).GetRobotType());
Console.WriteLine(string.Join(", ", ((List<string>)((IFlyingRobot)quadcopter).GetComponents()).ToArray()));
Console.WriteLine(((IFlyingRobot)quadcopter).GetInfo());
Console.WriteLine();

HomeWork1.Program.PrintConsole("********** IRobot **********");
Console.WriteLine(((IRobot)quadcopter).GetRobotType());
Console.WriteLine(string.Join(", ", ((List<string>)((IRobot)quadcopter).GetComponents()).ToArray()));
Console.WriteLine(((IRobot)quadcopter).GetInfo());
Console.WriteLine();
```

����� ���������:
```
********** Quadcopter **********
Charging...
Charged!
rotor1, rotor2, rotor3, rotor4

********** IChargeable **********
Charging...
Charged!
I am IChargeable from Quadcopter

********** IRobot **********
I am a simple robot.
rotor1, rotor2, rotor3, rotor4
I am IRobot from Quadcopter

********** IFlyingRobot **********
I am a flying robot.
rotor1, rotor2, rotor3, rotor4
I am IRobot from Quadcopter
```