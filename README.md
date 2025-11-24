
# Driver Matching System

Система подбора ближайших водителей для заказов.

## Алгоритмы поиска

1. **Linear Search** - линейный поиск с сортировкой по расстоянию

2. **Grid Search** - поиск по расширяющейся сетке

3. **KD-Tree** - эффективная древовидная структура для пространственного поиска

4. **Priority Queue** - оптимизированный поиск ближайших соседей

## Запуск

```bash
dotnet run src/DriverMatching.Console/

## TESTS

```bash
dotnet test src/DriverMatching.Tests/