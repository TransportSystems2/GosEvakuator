FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build

RUN curl -sL https://deb.nodesource.com/setup_11.x | bash -
RUN apt-get install -y nodejs

# install bower
RUN npm install --global bower

WORKDIR /src
COPY src/GosEvakuator/GosEvakuator.csproj src/GosEvakuator/
RUN dotnet restore src/GosEvakuator/GosEvakuator.csproj
COPY . .
WORKDIR /src/src/GosEvakuator
RUN dotnet build GosEvakuator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GosEvakuator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GosEvakuator.dll"]