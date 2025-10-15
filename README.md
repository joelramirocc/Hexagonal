# Hexagonal

Este repositorio contiene un ejemplo de arquitectura hexagonal. Utiliza capas claramente separadas para dominio, infraestructura y puertos, con el objetivo de facilitar el mantenimiento y las pruebas.

## Estructura propuesta de la solución

La solución se organiza en dos microservicios principales dentro de una misma solución: **Inventario** y **Ventas**. Cada
microservicio sigue los principios de arquitectura hexagonal, separando las capas de API, Aplicación, Dominio e
Infraestructura. Además, se incluye un proyecto de bloques comunes para compartir contratos o utilidades transversales y
una carpeta de pruebas unitarias por cada microservicio.

```
Hexagonal.sln
├── src/
│   ├── BuildingBlocks/
│   │   └── Common/
│   │       └── .gitkeep
│   ├── Inventory/
│   │   ├── Inventory.Api/
│   │   │   └── .gitkeep
│   │   ├── Inventory.Application/
│   │   │   └── .gitkeep
│   │   ├── Inventory.Domain/
│   │   │   └── .gitkeep
│   │   └── Inventory.Infrastructure/
│   │       └── .gitkeep
│   └── Sales/
│       ├── Sales.Api/
│       │   └── .gitkeep
│       ├── Sales.Application/
│       │   └── .gitkeep
│       ├── Sales.Domain/
│       │   └── .gitkeep
│       └── Sales.Infrastructure/
│           └── .gitkeep
└── tests/
    ├── Inventory.Tests/
    │   └── .gitkeep
    └── Sales.Tests/
        └── .gitkeep
```

Esta estructura sirve como base para crear proyectos de ASP.NET Core en `*.Api`, bibliotecas de clases en `*.Application`,
`*.Domain` y `*.Infrastructure`, y pruebas unitarias en los directorios bajo `tests/`.

## Objetivo

El propósito principal es servir como referencia para comprender cómo aplicar principios de arquitectura hexagonal en proyectos de software.
