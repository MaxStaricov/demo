# DateTimeHumanizeStrategy Demo (C#)

Минимальный демонстрационный проект, использующий **только** подсистему
[Humanizer / DateTimeHumanizeStrategy](https://github.com/Humanizr/Humanizer/tree/main/src/Humanizer/DateTimeHumanizeStrategy).

## Что внутри

```
DateTimeHumanizeStrategyDemo.sln
src/Humanizer.DateTimeHumanizeStrategy/   <- скопированная подсистема (13 файлов, без изменений)
    DateTimeHumanizeAlgorithms.cs          ядро: DefaultHumanize / PrecisionHumanize
    IDateTimeHumanizeStrategy.cs           интерфейсы стратегий (x4)
    IDateOnlyHumanizeStrategy.cs
    ITimeOnlyHumanizeStrategy.cs
    IDateTimeOffsetHumanizeStrategy.cs
    Default*HumanizeStrategy.cs            дефолтные стратегии (x4)
    Precision*HumanizeStrategy.cs          precision-стратегии (x4)
    HumanizeExtensions.cs                  удобные .Humanize() над стратегиями (демо-обёртка)
    GlobalUsings.cs                        global using System(.Globalization) — как в оригинале
    Shim/                                  минимальные заглушки внешних зависимостей:
        Tense.cs, TimeUnit.cs              enum'ы из Humanizer (скопированы 1-в-1)
        HumanizerShim.cs                   IFormatter / DefaultFormatter / Configurator
demo/Demo.csproj + Program.cs              консольная демонстрация
```

Примечание: подсистема стратегий в Humanizer вызывает `Configurator.GetFormatter(culture)`
для локализации результата. Чтобы демо оставалось «только про стратегии», слой локализации
заменён крошечным английским форматтером (`Shim/HumanizerShim.cs`) с теми же сигнатурами.
Никаких NuGet-зависимостей нет.

## Запуск

```bash
dotnet run --project demo
```

### Требования к .NET

Проект таргетит **net10.0** — достаточно установленного .NET 10 SDK/Runtime
(например, Homebrew: `/opt/homebrew/Cellar/dotnet`). `global.json` содержит
`rollForward: latestMajor`, поэтому подойдёт любой свежий SDK 10.x.

Если у вас только .NET 8, замените `<TargetFramework>net10.0</TargetFramework>`
на `net8.0` в двух csproj-файлах — исходный код от этого не зависит
(стратегии используют только API, доступный с .NET 6):

```bash
sed -i '' 's|<TargetFramework>net10.0</TargetFramework>|<TargetFramework>net8.0</TargetFramework>|' \
  demo/Demo.csproj src/Humanizer.DateTimeHumanizeStrategy/Humanizer.DateTimeHumanizeStrategy.csproj
dotnet run --project demo
```

## Что демонстрируется

1. `DefaultDateTimeHumanizeStrategy` — классический «2 days ago / in 1 hour».
2. `PrecisionDateTimeHumanizeStrategy(0.75 / 0.85 / 0.5)` — то же расстояние при разной точности округления.
3. Прямой вызов `strategy.Humanize(input, comparisonBase, culture)` — сравнение двух произвольных дат.
4. Стратегии для `DateOnly`, `TimeOnly`, `DateTimeOffset`.
5. Собственная стратегия через `IDateTimeHumanizeStrategy` (русский вариант) — расширяемость подсистемы.

Целевой фреймворк: .NET 8.0 (DateOnly/TimeOnly требуют net6+; файлы защищены `#if NET6_0_OR_GREATER`).

## UML-диаграммы

В каталоге [`docs/`](docs/) лежат исходники (PlantUML) и отрендеренные картинки:

- `docs/sequence-diagram.puml` — диаграмма последовательности (`SequenceDiagram.png/.svg`)
- `docs/communication-diagram.puml` — диаграмма коммуникации (`CommunicationDiagram.png/.svg`)

Нумерация сообщений (1.x–5.x) на обеих диаграммах одинаковая и соответствует
пяти сегментам демо выше. Перегенерация — см. `docs/README.md`.
