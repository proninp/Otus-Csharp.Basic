# Домашняя работа №16

Dapper / Linq2sql

## Цель
Получить навык работы с БД из программы, получения выборки и последующей ее обработки.
Получить опыт работы с ORM Dapper (Linq2Db)

## Описание/Пошаговая инструкция выполнения домашнего задания:

0. Определиться с `ORM`: `Dapper` (`Linq2Db`).
1. Выбрать какую БД использовать (из задания `Sql запросы` или `Кластерный индекс`), написать строку подключения к БД и использовать ее для подключения. (опираться можно на пример из материалов)
2. Создать классы, которые описывают таблицы в БД
3. Используя `ORM` выполнить простые запросы к каждой таблице, выполнить параметризованные запросы к каждой таблице (без `JOIN`) - 2-3 запроса на таблицу.
4. Значения параметров для фильтрации можно как задавать из консоли, так и значениями переменных в коде. (пример `GetStudent`)
5. Выполнить все запросы, из выбранного ранее задания с передачей параметров.


## Критерии оценки
* 0-3: 9 баллов
* 4: +1 балл

## Решение

Код метоа Main

```cs
using var db = new NpgsqlConnection(AppConfig.DbConnectionString);

ColoredPrint("********** Customers **********");
var customers = new Customers(db);
var customersList = customers.GetCustomers();
customersList?.ForEach(Console.WriteLine);
Console.WriteLine(customers.GetCustomerById(1));

ColoredPrint("********** Products **********");
var products = new Products(db);
var productsList = products.GetProducts();
productsList?.ForEach(Console.WriteLine);
Console.WriteLine(products.GetProductById(2));

ColoredPrint("********** Orders **********");
var orders = new Products(db);
var ordersList = products.GetProducts();
ordersList?.ForEach(Console.WriteLine);
Console.WriteLine(orders.GetProductById(3));
```

Вывод программы

```shell
********** Customers **********
Customer ID - 1, FirstName - Александр, LastName - Друзь, Age - 68
Customer ID - 2, FirstName - Максим, LastName - Поташев, Age - 54
Customer ID - 3, FirstName - Виктор, LastName - Сиднев, Age - 68
Customer ID - 4, FirstName - Андрей, LastName - Козлов, Age - 62
Customer ID - 5, FirstName - Дмитрий, LastName - Авдеенко, Age - 46
Customer ID - 6, FirstName - Алесь, LastName - Мухин, Age - 47
Customer ID - 7, FirstName - Михаил, LastName - Мун, Age - 48
Customer ID - 8, FirstName - Михаил, LastName - Скипский, Age - 44
Customer ID - 9, FirstName - Алексей, LastName - Капустин, Age - 65
Customer ID - 10, FirstName - Юлия, LastName - Лазарева, Age - 40
Customer ID - 1, FirstName - Александр, LastName - Друзь, Age - 68
********** Products **********
Product ID - 1, Name - PC1, Description - PC1 Description, StockQuantity - 20, Price - 600,0
Product ID - 2, Name - Printer1, Description - Printer1 Description, StockQuantity - 15, Price - 90,0
Product ID - 3, Name - Laptop1, Description - Laptop1 Description, StockQuantity - 5, Price - 800,0
Product ID - 4, Name - PC2, Description - PC2 Description, StockQuantity - 15, Price - 750,0
Product ID - 5, Name - Printer2, Description - Printer2 Description, StockQuantity - 10, Price - 110,0
Product ID - 6, Name - Laptop2, Description - Laptop2 Description, StockQuantity - 10, Price - 1000,0
Product ID - 7, Name - PC3, Description - PC3 Description, StockQuantity - 10, Price - 900,0
Product ID - 8, Name - Printer3, Description - Printer3 Description, StockQuantity - 5, Price - 250,0
Product ID - 9, Name - Laptop3, Description - Laptop3 Description, StockQuantity - 5, Price - 1200,0
Product ID - 10, Name - PC4, Description - PC4 Description, StockQuantity - 20, Price - 1000,0
Product ID - 11, Name - Printer4, Description - Printer4 Description, StockQuantity - 25, Price - 100,0
Product ID - 12, Name - Laptop4, Description - Laptop4 Description, StockQuantity - 10, Price - 950,0
Product ID - 2, Name - Printer1, Description - Printer1 Description, StockQuantity - 15, Price - 90,0
********** Orders **********
Product ID - 1, Name - PC1, Description - PC1 Description, StockQuantity - 20, Price - 600,0
Product ID - 2, Name - Printer1, Description - Printer1 Description, StockQuantity - 15, Price - 90,0
Product ID - 3, Name - Laptop1, Description - Laptop1 Description, StockQuantity - 5, Price - 800,0
Product ID - 4, Name - PC2, Description - PC2 Description, StockQuantity - 15, Price - 750,0
Product ID - 5, Name - Printer2, Description - Printer2 Description, StockQuantity - 10, Price - 110,0
Product ID - 6, Name - Laptop2, Description - Laptop2 Description, StockQuantity - 10, Price - 1000,0
Product ID - 7, Name - PC3, Description - PC3 Description, StockQuantity - 10, Price - 900,0
Product ID - 8, Name - Printer3, Description - Printer3 Description, StockQuantity - 5, Price - 250,0
Product ID - 9, Name - Laptop3, Description - Laptop3 Description, StockQuantity - 5, Price - 1200,0
Product ID - 10, Name - PC4, Description - PC4 Description, StockQuantity - 20, Price - 1000,0
Product ID - 11, Name - Printer4, Description - Printer4 Description, StockQuantity - 25, Price - 100,0
Product ID - 12, Name - Laptop4, Description - Laptop4 Description, StockQuantity - 10, Price - 950,0
Product ID - 3, Name - Laptop1, Description - Laptop1 Description, StockQuantity - 5, Price - 800,0
```