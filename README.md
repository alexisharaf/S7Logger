# S7Logger

Программа переодически считывает значения из неоптимизированных блоков данных ПЛК Siemens и записывает их в базу данных Postgresql. 
Настройка производится в файле appsettings.json.

Формат файла: 
````
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },

  "ConnectionStrings": {
    "DbConnectionString": "Host=localhost; Database=plc_log; Username=postgres; Password=qwerty; "

  },

  "Timer": {
    "hour": "0",
    "minute": "0",
    "second": "3",
    "millisecond": "500"

  },


  "Plc": {
    "plc_name": "plc_flow_control",
    "plc_ip": "192.168.3.242",
    "plc_rack": "0",
    "plc_slot": "1"
  },

  "Parameters": [
    {
      "parameter_name": "count",
      "db": "1000",
      "offset": "0",
      "bitnumber": "0",
      "parameter_type": "int4"

    },
    {
      "parameter_name": "flow1",
      "db": "1000",
      "offset": "4",
      "bitnumber": "0",
      "parameter_type": "float4"

    },
    {
      "parameter_name": "param3",
      "db": "1000",
      "offset": "8",
      "bitnumber": "0",
      "parameter_type": "float4"

    },
    {
      "parameter_name": "param4",
      "db": "1000",
      "offset": "12",
      "bitnumber": "0",
      "parameter_type": "float8"

    },
    {
      "parameter_name": "param5",
      "db": "1000",
      "offset": "20",
      "bitnumber": "0",
      "parameter_type": "float8"

    },
    {
      "parameter_name": "param6",
      "db": "1000",
      "offset": "28",
      "bitnumber": "0",
      "parameter_type": "bool"

    },
    {
      "parameter_name": "param7",
      "db": "1000",
      "offset": "28",
      "bitnumber": "1",
      "parameter_type": "bool"

    }
  ]

 
}
````

### Секция "ConnectionStrings" задает параметры подключения к базе данных.

- Параметр "Host" задает имя машины или IP адрес сервера базы данных.
- Параметр "Database" задает название базы данных в которой будет созданы таблицы для записи значений. На момент начала работы программы база данных дложна быть создана на сервере БД.
- Параметр "Username" задает имя пользователя для подключения к базе данных. На момент запуска программы пользователь должен быть создан в БД.
- Параметр "Password" задает пароль пользователя для подключения к базе данных.

Пользователь должен иметь права на создание таблиц в базе данных, добавления в нее колонок и добавления данных.

### Секция "Timer" задает переодичность опроса ПЛК.

- Параметер "hour" задает количество часов в переодичности опроса.
- Параметер "minute" задает количество минут в переодичности опроса.
- Параметер "second" задает количество минут в переодичности опроса.
- Параметер "millisecond" задает количество миллисекунд в переодичности опроса.

### Секция "Plc" задает параметры подключения к ПЛК.

- Параметр  "plc_name" задает имя ПЛК. При запуске программа проверяе и в случае отсутсвия создает в базе данных таблицу с именем совпадающем с эим значением. В данную таблицу будут записываться считанные с ПЛК параметры.
- Параметр "plc_ip" задает IP адрес ПЛК.
- Параметр "plc_rack" задает адрес корзины ПЛК.
- Параметр "plc_slot" задает слот ПЛК.

Для чтения ПЛК используется библиотека [Sharp7](https://github.com/fbarresi/Sharp7).

Информация по настройке ПЛК и параметров связи можно найти на сайте библиотеки по [адресу](https://github.com/fbarresi/Sharp7/wiki).

### Секция "Parameters" задает параметры переменных для чтения с ПЛК.
- Параметр "parameter_name" задает название переменной значение которой будет считано с ПЛК.
- Параметр "parameter_type" задает тип переменной значение которой будет считано с ПЛК.
     
  Поддерживаемые типы парметров:
  - bool  бинарное значение
  - int2  2х байтное целое
  - int4  4х байтное целое 
  - int8  8и байтное целое
  - float4  4х байтное с плавающей точкой
  - float8  8и байтное с плавающей точкой
  
- Параметр "db" задает номер неоптимизированного блока банных из которого нужно считывать переменную.
- Параметр "offset" задает смещение от начала неоптимизированного блока банных по которому расположена переменная.
- Параметр "bitnumber" задает номер бита при чтении значения типа bool. Для остальных типов данных значение должно быть равно 0.

При запуске, программа проверяет, и случае отсутствия создает в таблице с именем равным значению "plc_name" колонку с названием записанным в "parameter_name" и типом "parameter_type".
Далее программа начинает считывать данные с ПЛК и записывать их в БД с временной меткой.

### Описание блока данных в ПЛК

Файл конфигурации приведенный в примере соответсвует следующему блоку данных в ПЛК:

```
DATA_BLOCK "Data_block_1"
{ S7_Optimized_Access := 'FALSE' }
VERSION : 0.1
NON_RETAIN
   STRUCT 
      "counter" : DInt;
      flow1 : Real;
      param3 : Real;
      Param4 : LReal;
      param5 : LReal;
      bool1 : Bool;
      bool2 : Bool;
   END_STRUCT;


BEGIN

END_DATA_BLOCK

```

В процессе работы будет создана следующая таблица:

```
CREATE TABLE IF NOT EXISTS public.plc_flow_control
(
    time_stamp timestamp without time zone NOT NULL DEFAULT now(),
    count integer,
    flow1 real,
    param3 real,
    param4 double precision,
    param5 double precision,
    param6 boolean,
    param7 boolean
)

```
