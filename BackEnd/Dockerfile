FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 433

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["BackEnd/BackEnd.csproj", "BackEnd/"]
RUN dotnet restore "BackEnd/BackEnd.csproj"
COPY . .
WORKDIR "/src/BackEnd"
RUN dotnet build "BackEnd.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BackEnd.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackEnd.dll"]
