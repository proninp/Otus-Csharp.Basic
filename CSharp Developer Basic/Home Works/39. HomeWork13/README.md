# Домашняя работа №13

LINQ операторы

## Цель
1. Напишите свой метод расширения с названием "`Top`" для коллекции `IEnumerable`, принимающий значение `Х` от 1 до 100 и возвращающий заданное количество процентов от выборки с округлением количества элементов в большую сторону.
То есть для списка `var list = new List{1,2,3,4,5,6,7,8,9};`
`list.Top(30)` должно вернуть 30% элементов от выборки по убыванию значений, то есть `[9,8,7] (33%)`, а не `[9,8] (22%)`.
Если переданное значение больше 100 или меньше 1, то выбрасывать `ArgumentException`.

2. Напишите перегрузку для метода "`Top`", которая принимает ещё и поле, по которому будут отбираться топ `Х` элементов. Например, для `var list = new List{...}`, вызов `list.Top(30, person => person.Age)` должен вернуть 30% пользователей с наибольшим возрастом в порядке убывания оного.

## Описание/Пошаговая инструкция выполнения домашнего задания:

1. Создайте дженерик метод расширения для `IEnumerable`, возвращающий коллекцию, на которой был вызван;
2. Ограничьте количество элементов выходной коллекции;
3. Создайте дженерик перегрузку метода `Top`, добавив для этого одним из параметров функцию, принимающую `T` и возвращающую `int`;
4. Сделайте код-ревью (напишите свой отзыв) на одну из работ других студентов. Ссылки можете попросить в слаке. Для первого студента этот пункт опциональный (хотя и желательный), так как пока нет других работ.

## Критерии оценки
* Пункты 1-2: 5 баллов
* Пункт 3: 4 балла
* Пункт 4: 1 балл
* Минимальный проходной балл: 6.

## Решение

Код метоа Main

```cs
var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
var listPart = list.Top(30);
Console.WriteLine(string.Join(", ", listPart));

var persons = new List<Person>()
{

	new Person("Bjarne", 72),
	new Person("James", 68),
	new Person("Bill", 67),
	new Person("Linus", 53),
	new Person("Guido", 67),
	new Person("Brendan", 62)
};
var personsPart = persons.Top(50, p => p.Age);
Console.WriteLine(string.Join(", ", personsPart));
```

Вывод программы

```shell
9, 8, 7
Bjarne with age: 72, James with age: 68, Bill with age: 67
```