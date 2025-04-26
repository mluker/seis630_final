# Weather API Project

## Overview

This project is a Weather API that collects and analyzes temperature data from different locations across the United States. It's designed to store weather readings from multiple cities and provide insightful reports about temperature trends and patterns.

## What Does This Project Do?

The Weather API allows you to:

- **Store weather data**: Record temperature readings for cities across the country
- **Track location information**: Maintain a database of cities with their state, zip code, and country
- **Generate weather reports**: Create helpful summaries of temperature data
- **Analyze temperature patterns**: Compare temperatures across different regions

## Key Features

### Location Management
- Add new cities to the database
- Retrieve information about existing locations
- Remove locations when they're no longer needed

### Temperature Data
- Record new temperature readings for any location
- View historical temperature data for specific cities
- Delete outdated or incorrect weather records

### Weather Reports
- **Average Temperature by Location**: See the average temperature for each city
- **Hottest Locations**: Find the top 10 warmest cities based on average temperature
- **Coldest Locations**: Find the top 10 coolest cities based on average temperature
- **Temperature Trends**: Track how temperatures have changed over the past 7 days

## Project Structure

The project follows a clean, organized structure:
- **Models**: Defines the data structure for locations and weather readings
- **Endpoints**: Contains the API routes for different features
- **Data**: Handles database connections and initial setup

## How to Use It

### Testing with Insomnia

The project includes an `insomnia.json` file that can be imported into the Insomnia REST client. This provides a ready-to-use collection of API requests for testing all the features.

### Data Model

The project uses a simple data model with two main entities:
- **Location**: Stores information about cities
- **WeatherData**: Records temperature readings for specific locations

The relationship between these entities is visualized in the `data_model_diagram.md` file.

## Technologies Used

This project was built using:
- ASP.NET Core for the API framework
- Entity Framework Core for database interaction
- C# as the programming language
- RESTful API design principles
- Azure SQL

## Class Project Information

This project was developed for the SEIS630 Database Management Systems course at the University of St. Thomas. It demonstrates the practical application of database concepts including:
- Entity-relationship modeling
- SQL query generation (visible in code comments)
- Database design and implementation
- API development with database integration
- Cloud databases