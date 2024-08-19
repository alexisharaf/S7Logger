### Запуск программы в качестве system.d service

Программа может быть запущена как system.d сервис в Линукс.

Для этого неоходимо сделать следующее:

- Скопировать файл S7Logger.service в /etc/systemd/system/
- Исправвить S7Logger.service, указав путь до файла S7Logger.

- Выполнить 

> sudo systemctl daemon-reload

проверить

> sudo systemctl status S7Logger.service

Должен показать что сервис найден, но не запущен.
запсутить:

> sudo systemctl start S7Logger.service

остановить:

> sudo systemctl stop S7Logger.service

Сделать запускаемым при запуске системы:

> sudo systemctl enable S7Logger.service  
