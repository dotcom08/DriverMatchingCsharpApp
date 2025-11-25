
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

```

## Tests

```bash
dotnet test src/DriverMatching.Tests/
```

## Benchmark Results

The following benchmark results compare the performance of all four search algorithms across different dataset sizes:

![Benchmark Results](/src/images/image1.png)
![Benchmark Results](/src/images/image2.png)

### Использование памяти

- **Linear Search**: Высокое использование памяти для больших наборов данных
- **Grid Search**: Умеренное использование памяти
- **KD-Tree**: Низкое использование памяти после построения дерева
- **Priority Queue**: Умеренное использование памяти

### Рекомендации по выбору алгоритма

- **Маленькие наборы данных (< 1,000)**: Linear Search или Priority Queue
- **Средние наборы данных (1,000 - 10,000)**: Grid Search или KD-Tree
- **Большие наборы данных (> 10,000)**: KD-Tree
- **Частые поиски**: KD-Tree (после первоначального построения)