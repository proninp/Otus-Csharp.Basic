# Домашняя работа №9

Делегаты, Event-ы, добавляем асинхронное выполнение

## Цель
Тренируемые навыки:

* работа с событиями
* избегание блокировок через асинхронные вызовы

## Описание/Пошаговая инструкция выполнения домашнего задания:

1. Напишите класс `ImageDownloader`. В этом классе должен быть метод `Download`, который скачивает картинку из интернета. Для загрузки картинки можно использовать примерно такой код: https://dotnetfiddle.net/5oT1Hi
```cs
// Откуда будем качать
string remoteUri = "https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg";
// Как назовем файл на диске
string fileName = "bigimage.jpg";
// Качаем картинку в текущую директорию
var myWebClient = new WebClient();
Console.WriteLine("Качаю \"{0}\" из \"{1}\" .......\n\n", fileName, remoteUri);
myWebClient.DownloadFile(remoteUri, fileName);
Console.WriteLine("Успешно скачал \"{0}\" из \"{1}\"", fileName, remoteUri);
```

2. Создайте экземпляр этого класса и вызовите скачивание большой картинки, например, https://effigis.com/wp-content/uploads/2015/02/Iunctus_SPOT5_5m_8bit_RGB_DRA_torngat_mountains_national_park_8bits_1.jpg
В конце работы программы выведите в консоль "Нажмите любую клавишу для выхода" и ожидайте нажатия любой клавиши.

3. Добавьте события: в классе `ImageDownloader` в начале скачивания картинки и в конце скачивания картинки выкидывайте события (`event`) `ImageStarted` и `ImageCompleted` соответственно.
В основном коде программы подпишитесь на эти события, а в обработчиках их срабатываний выводите соответствующие уведомления в консоль: "Скачивание файла началось" и "Скачивание файла закончилось".

4. Сделайте метод `ImageDownloader.Download` асинхронным. Если Вы скачивали картинку с использованием `WebClient.DownloadFile`, то используйте теперь `WebClient.DownloadFileTaskAsync` - он возвращает `Task`.
В конце работы программы выводите теперь текст "Нажмите клавишу `A` для выхода или любую другую клавишу для проверки статуса скачивания" и ожидайте нажатия любой клавиши. Если нажата клавиша `A` - выходите из программы. В противном случае выводите состояние загрузки картинки (`True` - загружена, `False` - нет). Проверить состояние можно через вызов `Task.IsCompleted`.
Поздравляю! Ваша загрузка картинки работает асинхронно с основным потоком консоли.

## Критерии оценки
* Пункты 1-3: +4 балла
* Пункт 4: +3 баллов
* Для сдачи достаточно 7 баллов.

## Решение

Вывод программы:
```shell
Скачивание файла началось
Качаю "bigimage.jpg" из "https://vastphotos.com/files/uploads/photos/10551/beautiful-mountain-photo-vast-xl.jpg?v=20220712073521" .......


Нажмите клавишу A для выхода или любую другую клавишу для проверки статуса скачивания
q
bigimage.jpg не загружен
Скачивание файла закончилось
Успешно скачал "bigimage.jpg" из "https://vastphotos.com/files/uploads/photos/10551/beautiful-mountain-photo-vast-xl.jpg?v=20220712073521"
q
bigimage.jpg загружен
A
```
![](https://github.com/proninp/Otus-Edu/blob/main/CSharp%20Developer%20Basic/Home%20Works/30.%20HomeWork09/HomeWork09/resources/HomeWork09%20Demo.png?raw=true)
