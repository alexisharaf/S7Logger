# S7Logger

Программа переодически считывает значения из неоптимизированных блоков данных ПЛК Siemens и записывает их в базу данных. 
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
