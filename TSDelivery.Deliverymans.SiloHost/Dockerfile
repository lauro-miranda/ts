FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
EXPOSE 80
EXPOSE 443
EXPOSE 11111
EXPOSE 8888

COPY . .

RUN dotnet restore

FROM build AS publish
RUN dotnet publish -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TSDelivery.Deliverymans.SiloHost.dll"]