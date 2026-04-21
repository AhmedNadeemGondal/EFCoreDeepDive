# EFCoreDeepDive

## Purpose
This repository is a technical exploration of **Entity Framework Core (EF Core)**, focusing on its role as a modern Object-Database Mapper (ORM) for .NET. The project moves beyond basic CRUD operations to examine how EF Core translates C# code into optimized SQL, manages entity states, and handles complex relational mappings.

## Core EF Core Pillars

### 1. Modeling and Mapping
Explores the various ways to define the relationship between the domain model and the database schema:
* **Fluent API vs. Data Annotations:** Utilizing `OnModelCreating` for advanced configurations that go beyond simple attribute-based mapping.
* **Relationship Mapping:** Implementing One-to-One, One-to-Many, and Many-to-Many relationships with explicit foreign key configurations.
* **Shadow Properties:** Defining properties that exist in the database but not in the C# class.

### 2. Querying Mechanics
Deep dive into how LINQ queries are translated and executed:
* **Loading Patterns:** Comparing **Eager Loading** (`.Include()`), **Explicit Loading**, and **Lazy Loading**.
* **Tracking vs. No-Tracking:** Optimizing read-only scenarios using `.AsNoTracking()` to reduce memory overhead and improve performance.
* **Raw SQL Queries:** Executing stored procedures or complex SQL strings while still mapping results to entities.

### 3. Change Tracking and Persistence
Understanding the lifecycle of an entity within the `DbContext`:
* **Entity States:** Monitoring how EF Core tracks `Added`, `Unchanged`, `Modified`, and `Deleted` states.
* **SaveChanges Workflow:** Understanding the atomic nature of `SaveChanges()` and how it handles transaction management.
* **Concurrency Control:** Implementing Optimistic Concurrency using `RowVersion` or tokens to prevent data overwrites.

### 4. Migrations and Schema Management
* **Code-First Workflow:** Managing database versioning through the EF Core CLI and Package Manager Console.
* **Migration Customization:** Manually editing migration files to include custom SQL or indexes.
* **Seeding Data:** Implementing initial data population within the migration logic.

## Implementation Highlights
* **DbContext Configuration:** Best practices for configuring connection strings and logging via `OnConfiguring`.
* **Repository Pattern Integration:** Abstracting EF Core logic to ensure the business layer remains decoupled from the data access technology.
* **Performance Tuning:** Identifying and solving common pitfalls like the N+1 query problem.


### Credits
Credit to **Nitish Kaushik**. Check out his [video series](https://www.youtube.com/watch?v=k0SDlRYMByE&list=PLak2C883P4cUfJpBRakIIIonC83w64mtV&index=1) for the original walkthrough.