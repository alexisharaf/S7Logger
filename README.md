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

  
Поддерживаемые типы парметров: bool, int2, int4, int8, float4, float8
