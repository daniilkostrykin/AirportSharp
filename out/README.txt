=== Шпаргалка по тестированию API AirportApp ===

1. POST /api/flights
   Берем тело из POST_api_flights_request.json.
   Копируем полученный ID рейса.

2. POST /api/passengers
   Создаем двух пассажиров: обычного и VIP (файлы POST_api_passengers_*_request.json).
   Копируем их ID.

3. POST /api/checkin
   Отправляем обычного пассажира на место "1A" (файл POST_api_checkin_request.json).
   Результат: успех, место "1A" забронировано.

4. POST /api/checkin (Проверка логики)
   Пытаемся посадить VIP-пассажира на ТО ЖЕ место "1A".
   Результат: 400 Bad Request ("Место 1A уже занято").

5. POST /api/checkin (VIP овербукинг)
   Пытаемся посадить VIP-пассажира на несуществующее место, например "99Z".
   Результат: успех! Система сгенерирует виртуальное место, так как у клиента флаг isVip = true.

6. DELETE /api/checkin/{ticketId}
   Отменяем регистрацию первого пассажира.
   Смотрим GET /api/flights — кол-во availableSeats должно снова увеличиться!

