# Project Info

This project serves as the backend for a platform idea I had, 
The idea is for a Modular IT System that enables the management of the inventor and the services. It has (sadly lack of time and budget hit).
to be designed to support modular administrative logic and a microservices environment. The project also supports Docker.

The platform is organized as a monorepo with two main setups:
1. **Frontend** - Contains all client interfaces for the platform.
2. **Backend** - Manages the entire project layout.

## Project Tech Flow

The backend uses the CQRS pattern, with operations managed by RabbitMQ (or any other queuing system) via MassTransit. 
The system supports both GraphQL and REST APIs and includes Redis and Elasticsearch. 
It features a self-configurable integrated caching system. All inputs are validated using the FlutterValidation package.

## Folder Structure on Backend

The backend layout is organized simply into:
1. **Domain** - Hosts entities and all database definitions.
2. **Domain.Persistence** - Contains repositories and database context.
3. **Common** - Includes generic files that do not fit elsewhere, such as middleware, enums, etc.
4. **Infrastructure** - An extension of Common, containing more contextual files like GQL files, validations, and generic services.
5. **Processor** - Manages the CQRS workers and handles logic.
6. **Services** - Contains all API and GQL services.
