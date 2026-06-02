FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/InterviewTTBApi.API/InterviewTTBApi.API.csproj",             "src/InterviewTTBApi.API/"]
COPY ["src/InterviewTTBApi.Application/InterviewTTBApi.Application.csproj", "src/InterviewTTBApi.Application/"]
COPY ["src/InterviewTTBApi.Infrastructure/InterviewTTBApi.Infrastructure.csproj", "src/InterviewTTBApi.Infrastructure/"]
COPY ["src/InterviewTTBApi.Domain/InterviewTTBApi.Domain.csproj",        "src/InterviewTTBApi.Domain/"]

RUN dotnet restore "src/InterviewTTBApi.API/InterviewTTBApi.API.csproj"

COPY . .

RUN dotnet publish "src/InterviewTTBApi.API/InterviewTTBApi.API.csproj" \
    -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InterviewTTBApi.API.dll"]
