SELECT 
    f."FlightNumber",
    a_dep."IataCode" AS "Departure",
    a_arr."IataCode" AS "Destination",
    f."Status", 
    f."AvailableSeats"
FROM "Flights" f
JOIN "Airports" a_dep ON f."OriginAirportId" = a_dep."Id"
JOIN "Airports" a_arr ON f."DestinationAirportId" = a_arr."Id"
ORDER BY f."DepartureTimeUtc";