# Open Telemetry
>OpenTelemetry is a set of APIs, SDKs, tooling and integrations that are designed for the creation and management of telemetry data such as traces, metrics, and logs. The project provides a vendor-agnostic implementation that can be configured to send telemetry data to the backends of your choice. It supports a variety of popular open-source projects including Jaeger and Prometheus. Also, a lot of vendors support OpenTelemetry directly or using the OpenTelemetry Collector.

OpenTelemetry defines 3 concepts when instrumenting an application:
- [Logging](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/logs/overview.md)
  - OpenTelemetry relies on Microsoft.Extensions.Logging.Abstractions to handle logging.
- [Tracing](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/api.md)
  - OpenTelemetry relies on types in System.Diagnostics.* to support tracing.
- [Metrics](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/metrics/api.md)
  - Metrics API are incorporated directly into the .NET runtime itself. So, you can instrument your application by simply depending on System.Diagnostics.*. .NET supports 4 kind of metrics: Counter, ObservableCounter, Histogram, ObservableGauge

## Key Terms

| System.Diagnostics | OpenTelemetry | Meaning |
|:---|:---|:---|
|ActivitySource|Tracer|A source of tracing activity. Normal to have many per app.|
|Activity|TelemetrySpan|Represents a single unit of work recorded during a trace.|
|ActivityContext|SpanContext|The propagation context used to correlate spans.|
|Attributes|Attributes|Searchable properties of a span.|
|Events|Events|Log events and other items recorded during an individual span.|
|Meter|Meter|ActivitySource equivalent, but for metrics|
|Counter|Counter|Monotonic counter used for measuring rates and frequencies.|
|ObservableGauge|ObservableGauge|Async gauge that measures arbitrary values over time.|
|Histogram|Histogram|Used to bucket counters into distributions.|

## Exporting data
- Using the [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/) (recommended)
  - The recommended way is to use the collector to export data. It makes your application back-end agnostic and provides a consistent way to export data for all your applications.
    ```mermaid
    flowchart TB
        classDef appstyle fill:#1995AD,stroke:#fff,stroke-width:1px;
        classDef opentelstyle fill:#F34A4A,stroke:#fff,stroke-width:1px;    
        classDef visstyle fill:#4D648D,stroke:#fff,stroke-width:1px;

        App1(Application #1):::appstyle --Otp--> OTEL(OpenTelemetry)
        App2(API #2):::appstyle  --Otp--> OTEL(OpenTelemetry)
        App3(Background Task #3):::appstyle  --Otp--> OTEL(OpenTelemetry)
        App4(Whatever #4):::appstyle  --Otp--> OTEL(OpenTelemetry)
    
        OTEL(OpenTelemetry) -->|Export| Zk(ZipKin):::visstyle
        OTEL(OpenTelemetry) -->|Export| J(Jaeger):::visstyle
        OTEL(OpenTelemetry) -->|Export| P(Prometheus):::visstyle
        OTEL(OpenTelemetry) -->|Export| Am(AzureMonitor):::visstyle
        OTEL(OpenTelemetry Collector<br>Receive, Process, Export Data):::opentelstyle -->|Export| EA(ElasticAPM):::visstyle
    
    ```
- Exporting to each back-end directly 
  - If you want to export directly to the backends without using the OpenTelemetry Collector, you can use the [NuGet packages OpenTelemetry.Exporter.*](https://www.nuget.org/packages?q=opentelemetry.exporter)
  ```mermaid
    flowchart TB
        classDef appstyle fill:#1995AD,stroke:#fff,stroke-width:1px;
        classDef opentelstyle fill:#F34A4A,stroke:#fff,stroke-width:1px;    
        classDef visstyle fill:#4D648D,stroke:#fff,stroke-width:1px;

        App1(Application #1):::appstyle --> Zk(ZipKin):::visstyle
        App1(Application #1):::appstyle --> J(Jaeger):::visstyle
        App1(Application #1):::appstyle --> P(Prometheus):::visstyle
        App1(Application #1):::appstyle --> EA(ElasticAPM):::visstyle

        App2(Application #2):::appstyle --> Zk(ZipKin):::visstyle
        App2(Application #2):::appstyle --> J(Jaeger):::visstyle
        App2(Application #2):::appstyle --> P(Prometheus):::visstyle
        App2(Application #2):::appstyle --> EA(ElasticAPM):::visstyle
    
    ```
There are multiple ways to deploy [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/). You can check the documentation for all details. In the following section, we'll use docker-compose to start the server and a few back-ends.

##Starting the collector and back-ends
The OpenTelemetry Collector is a generic service. You need to configure it to select how you want to receive, process and export data.

```yaml
otel-collector:
    image: otel/opentelemetry-collector:latest
    command: ["--config=/etc/otel-collector-config.yaml"]
    volumes:
      - ./otel-collector-config.yaml:/etc/otel-collector-config.yaml
      - ./output:/etc/output:rw # Store the logs
    ports:
      - "8888:8888"   # Prometheus metrics exposed by the collector
      - "8889:8889"   # Prometheus exporter metrics
      - "4317:4317"   # OTLP gRPC receiver
    depends_on:
      - zipkin-all-in-one
```
---

## What is ZipKin?
Zipkin is a distributed tracing system. It helps gather timing data needed to troubleshoot latency problems in service architectures.

## What is Jaeger?
Jaeger is open source software for tracing transactions between distributed services. It's used for monitoring and troubleshooting complex microservices environments.

---
## Links:
- [Monitoring a .NET application using OpenTelemetry](https://www.meziantou.net/monitoring-a-dotnet-application-using-opentelemetry.htm)
- [OpenTelemetry Implementations - Getting Started](https://opentelemetry.io/docs/instrumentation/net/getting-started/)
- [Jaeger tracing with OpenTelemetry](https://www.jaegertracing.io/docs/1.21/opentelemetry/)
- [Jaeger: open source, end-to-end distributed tracing](https://www.jaegertracing.io/)
- [ZipKin](https://zipkin.io/)


