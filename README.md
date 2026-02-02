# Centro Deportivo – Aplicación WPF (MVVM)

Aplicación de escritorio desarrollada en **WPF** siguiendo el patrón **MVVM**, orientada a la gestión de un centro deportivo.  
Permite administrar **socios**, **actividades** y **reservas**, utilizando **Entity Framework** para la persistencia de datos.

## Funcionalidades
- Gestión de socios
- Gestión de actividades y control de aforo
- Gestión de reservas:
  - Alta, modificación y eliminación
  - Validación de fechas
  - Control de aforo por actividad
- Interfaz basada en DataGrid con binding y selección de elementos

## Arquitectura
El proyecto sigue el patrón **MVVM**:
- **Model**: entidades y acceso a datos
- **View**: vistas WPF (XAML)
- **ViewModel**: lógica de presentación y comandos

## Ubicación del proyecto
El archivo de solución (`.sln`) se encuentra en la siguiente carpeta: CentroDeportivo

