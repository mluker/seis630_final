# Weather API Endpoints

## API Endpoints Overview

```mermaid
flowchart TB
    %% Main grouping
    subgraph "Location Endpoints"
        L1["GET /api/locations"]
        L2["GET /api/locations/{id}"]
        L3["GET /api/locations/{id}/weather"]
        L4["POST /api/locations"]
        L5["DELETE /api/locations/{id}"]
    end

    subgraph "Weather Endpoints"
        W1["GET /api/weather"]
        W2["GET /api/weather/{id}"]
        W3["POST /api/weather"]
        W4["DELETE /api/weather/{id}"]
    end

    subgraph "Report Endpoints"
        R1["GET /api/reports/average-by-location"]
        R2["GET /api/reports/hottest-locations"]
        R3["GET /api/reports/coldest-locations"]
        R4["GET /api/reports/temperature-trends"]
    end

    %% Styling
    classDef get fill:#c8e6c9,stroke:#2e7d32,color:#000
    classDef post fill:#ffecb3,stroke:#ff8f00,color:#000
    classDef delete fill:#ffcdd2,stroke:#c62828,color:#000

    class L1,L2,L3,W1,W2,R1,R2,R3,R4 get
    class L4,W3 post
    class L5,W4 delete
```

## Detailed Endpoint Descriptions

### Location Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/locations` | Retrieve all location records |
| GET    | `/api/locations/{id}` | Get a specific location by ID |
| GET    | `/api/locations/{id}/weather` | Get all weather data for a specific location |
| POST   | `/api/locations` | Create a new location (with duplicate checking) |
| DELETE | `/api/locations/{id}` | Delete a location (with optional cascade parameter) |

### Weather Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/weather` | Retrieve all weather data with location details |
| GET    | `/api/weather/{id}` | Get a specific weather record by ID |
| POST   | `/api/weather` | Create a new weather record (with optional new location) |
| DELETE | `/api/weather/{id}` | Delete a specific weather record |

### Report Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET    | `/api/reports/average-by-location` | Get average temperatures by location |
| GET    | `/api/reports/hottest-locations` | Get top 10 hottest locations |
| GET    | `/api/reports/coldest-locations` | Get top 10 coldest locations |
| GET    | `/api/reports/temperature-trends` | Get temperature trends over the last 7 days |

## HTTP Methods Legend

- 🟢 **GET** - Retrieve data
- 🟠 **POST** - Create new records
- 🔴 **DELETE** - Remove records