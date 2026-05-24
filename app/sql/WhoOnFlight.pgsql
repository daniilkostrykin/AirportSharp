SELECT 
    f."FlightNumber", 
    p."FullName", 
    p."IsVip", 
    t."SeatNumber", 
    t."BookingClass", 
    t."PaidPrice"
FROM "Tickets" t
JOIN "Passengers" p ON t."PassengerId" = p."Id"
JOIN "Flights" f ON t."FlightId" = f."Id"
WHERE t."CheckInTimeUtc" IS NOT NULL
ORDER BY t."SeatNumber";