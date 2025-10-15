# Hexagonal

Este repositorio contiene un ejemplo de arquitectura hexagonal. Utiliza capas claramente separadas para dominio, infraestructura y puertos, con el objetivo de facilitar el mantenimiento y las pruebas.

## Estructura

- `domain/`: Modelos y lógica de negocio.
- `application/`: Casos de uso que coordinan las operaciones del dominio.
- `infrastructure/`: Adaptadores para persistencia, servicios externos y entrada/salida.

## Objetivo

El propósito principal es servir como referencia para comprender cómo aplicar principios de arquitectura hexagonal en proyectos de software.
