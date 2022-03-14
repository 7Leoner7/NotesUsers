# NotesUsers
## Развёртывание сайта с помощью docker'ов.
1. Скачайте и установите DockerHub на свой компьютер (На Ubuntu: https://hub.docker.com/editions/community/docker-ce-server-ubuntu На Windows: https://hub.docker.com/editions/community/docker-ce-desktop-windows)
2. Откройте командную строку и введите в консоль следующие команды:
	2.1 docker pull tasksimbirsoft/noutesusers
	2.2 docker pull tasksimbirsoft/postgres
	2.3 docker run --name ClientDB -p 5432:5432 -e POSTGRES_USER=dbuser -e POSTGRES_PASSWORD=<ваш пароль> -e POSTGRES_DB=db -e PGDATA=/var/lib/postgresql/data/pgdata -d -v "/absolute/path/to/directory-with-data":/var/lib/postgresql/data -v "/absolute/path/to/directory-with-init-scripts":/docker-entrypoint-initdb.d tasksimbirsoft/postgres:13.3
	2.4 docker run -d -p <ваш ip-адрес>:5000:80 --name UserNotes tasksimbirsoft/notesusers
3. После данных действий зайдите на сайт по адресу http://<ваш ip-адрес>:5000
4. Дальше сайт запросит строку для подключения к базе данных. Вы должны ввести такую строку: Host=<ваш ip-адрес>;Port=5432;Username=dbuser;Password=<ваш пароль>;Database=db;
5. ВСЁ ГОТОВО👍😁

## Развёртывание сайта через docker-compose
1. Пункт точно такой же, что и первый пункт в развёртывании сайта
2. Скачайте с гит хаба файл 7Leoner7/NotesUsers/notesusers_docker-compose.yml (Перед выполнением следующего пункта переместите файл в отдельную пустую папку)
3. Введите команду в cmd: docker-compose  -f "<полный путь к файлу notesusers_docker-compose.yml>" -p <придумайте название данной коллекции docker'ов> --ansi never up -d
4. Пункт точно такой же, что и четвёртый пункт в развёртывании сайта
5. Всё
### Примечание: 
Данные подключения к базе данных можно взять из файла notesusers_docker-compose.yml

## О сайте.
Сайт сделан на языке C# на фреймворке ASP.NET. Использовал только возможности данного фреймворка + библиотека для работы с PostgreSQL. Этот сайт является первым моим проектом на фреймворке ASP.NET. Вообщем, в этом первом проекте я получил немало опыта в веб-разработке.

## Возможные ошибки в проекте.
Ошибок в самом проекте я не нашёл(а те которые нашёл исправил), но возможно такое, что дальше 4 пункта развёртывания сайта вы можете не продвинуться. Причин может быть несколько:
1. Неправильно выполнили пункт 2.3
2. Неправильно ввели свой ip-адрес
3. Программа не может создать две таблицы в базе данных
В третьем случае придётся вручную создавать базу данных:
	3.1 docker pull adminer
	3.2 docker run -d -p 8080:8080 adminer
	3.3 подключиться к adminer по адресу https://localhost:8080
	3.4 Подключиться к базе данных по серверу <ваш ip-адрес>:5432 по пользователю dbuser с паролем <ваш пароль> к базе данных db.
	3.5 Нажмите на кнопку SQL-запрос и вставьте команду, которая находится ниже(копируйте без ковычек):
"CREATE TABLE IF NOT EXISTS public.client (
    login text NOT NULL,
    password text NOT NULL
) WITH(oids = false);
CREATE SEQUENCE IF NOT EXISTS notes_id_seq INCREMENT 1 MINVALUE 1;
CREATE TABLE IF NOT EXISTS public.notes (
    id integer DEFAULT nextval('notes_id_seq') NOT NULL,
    note text NOT NULL,
    login text NOT NULL,
    head text NOT NULL,
    CONSTRAINT notes_pkey PRIMARY KEY(id)
) WITH(oids = false);"
	3.6 После этих действий вернитесь к пункту 4 по развёртке сайта.
