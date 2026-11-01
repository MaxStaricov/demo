# UML-диаграммы демонстрационного проекта

Исходники диаграмм (PlantUML) — их можно редактировать и перегенерировать:

| Файл | Диаграмма |
|---|---|
| `sequence-diagram.puml` | Диаграмма последовательности (Sequence) |
| `communication-diagram.puml` | Диаграмма коммуникации (Communication) |
| `SequenceDiagram.png/.svg`, `CommunicationDiagram.png/.svg` | Отрендеренные изображения |

## Как перегенерировать

```bash
# нужен Java + Graphviz (dot)
curl -sLO https://github.com/plantuml/plantuml/releases/download/v1.2025.4/plantuml-1.2025.4.jar
java -jar plantuml-1.2025.4.jar -charset UTF-8 -tpng -tsvg *.puml
```

Линейки сообщений на обеих диаграммах пронумерованы одинаково (1.x, 2.x, ...),
чтобы по ним легко сопоставлялись два представления одного и того же сценария:

1. `DateTime.Humanize()` через `HumanizeExtensions` → `DefaultDateTimeHumanizeStrategy` → `DateTimeHumanizeAlgorithms.DefaultHumanize` → `Configurator.GetFormatter` → `IFormatter.DateHumanize`
2. `PrecisionDateTimeHumanizeStrategy` → `DateTimeHumanizeAlgorithms.PrecisionHumanize` (округление с precision)
3. Прямой вызов `strategy.Humanize(a, b, null)` с произвольной базой сравнения
4. `DateOnly` / `TimeOnly` / `DateTimeOffset` — свои стратегии и overload'ы алгоритмов
5. Кастомная `RussianishStrategy : IDateTimeHumanizeStrategy` — обход подсистемы алгоритмов (демонстрация расширяемости)
