# Steeltoe .NET Open Source Software

[![Build Status](https://dev.azure.com/SteeltoeOSS/Steeltoe/_apis/build/status/Steeltoe.All?branchName=main)](https://dev.azure.com/SteeltoeOSS/Steeltoe/_build/latest?definitionId=4&branchName=main)&nbsp;[![NuGet Version](https://img.shields.io/nuget/v/Steeltoe.Common.svg?style=flat)](https://www.nuget.org/profiles/SteeltoeOSS)&nbsp;[![Stack Overflow](https://img.shields.io/badge/stack%20overflow-steeltoe-orange.svg)](http://stackoverflow.com/questions/tagged/steeltoe)

## Why Steeltoe?

[Steeltoe](https://steeltoe.io/) provides building blocks for development of .NET applications that integrate with [Spring](https://spring.io/) and [Spring Boot](https://spring.io/projects/spring-boot) environments, as well as [Cloud Foundry](https://www.cloudfoundry.org/) and [Kubernetes](https://kubernetes.io/) with first-party support for [Tanzu](https://tanzu.vmware.com/tanzu).

Key features include:

- External (optionally encrypted) configuration using [Spring Cloud Config Server](https://docs.spring.io/spring-cloud-config/docs/current/reference/html/)
- Service discovery with [Netflix Eureka](https://spring.io/projects/spring-cloud-netflix) and [HashiCorp Consul](https://www.consul.io/)
- Management endpoints (compatible with [actuators](https://docs.spring.io/spring-boot/docs/current/reference/html/actuator.html)), providing system info (such as versions, configuration, service container contents, mapped routes and HTTP traffic), heap/thread dumps, health checks, exporting metrics to [Prometheus](https://prometheus.io/), and changing log levels at runtime.
- Connectivity to databases (such as [SQL Server](https://www.microsoft.com/sql-server)/[Azure SQL](https://azure.microsoft.com/products/azure-sql), [Cosmos DB](https://azure.microsoft.com/products/cosmos-db/), [MongoDB](https://www.mongodb.com/), [Redis](https://redis.io/), [RabbitMQ](https://www.rabbitmq.com/), [PostgreSQL](https://www.postgresql.org/), and [MySQL](https://www.mysql.com/)), including support for [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- Single sign-on, JWT and Certificate auth with [Cloud Foundry](https://www.cloudfoundry.org/)

## Getting Started

In addition to the [documentation site](https://docs.steeltoe.io), we have built several tools to help you get started:

- [Steeltoe Initializr](https://start.steeltoe.io) - Pick and choose what type of application you would like to build and let us generate the initial project for you
  - The Initializr uses [.NET templates](https://github.com/SteeltoeOSS/NetCoreToolTemplates) that can also be used from the `dotnet` CLI and inside of Visual Studio
- [Steeltoe Samples](https://github.com/SteeltoeOSS/Samples) - Here we have working samples for trying out features and to use as code references

## Contributing

For more information on contributing to the project or other project information, please see the [Steeltoe Wiki](https://github.com/SteeltoeOSS/Steeltoe/wiki).

## Feedback and Support

For community support, we recommend [Steeltoe OSS Slack](https://slack.steeltoe.io) or [StackOverflow](https://stackoverflow.com/questions/tagged/steeltoe)

For production support, we recommend you reach out to [Broadcom Support](https://support.broadcom.com/).

For other questions or feedback, [open an issue](https://github.com/SteeltoeOSS/Steeltoe/issues/new/choose).

## Pre-release packages

Whether you are working with the team on validating a bugfix or just want to try the latest version available, you can use the Steeltoe development feed by adding a reference in your nuget.config file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="Steeltoe-dev" value="https://pkgs.dev.azure.com/dotnet/Steeltoe/_packaging/dev/nuget/v3/index.json" />
    <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
```

### Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).
