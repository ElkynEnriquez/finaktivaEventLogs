# finaktivaEventLogs
Crear Red de docker
docker network create microservicesnetwork

RabbitMq
docker run -d --hostname finaktiva-rabbit --name finaktiva-rabbitmq rabbitmq:3 
RabbitManagement
docker run -d --hostname finaktiva-rabbit-web --name finaktiva-rabbitmq-web rabbitmq:management

Conectar a reddecontainers
docker network connect microservicesnetwork [containerId] 
docker network connect microservicesnetwork [containerId]

Crear container SqlServer
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Fk!1234*" -e "MSSQL_PID=Evaluation" -p 1434:1433 -d mcr.microsoft.com/mssql/server:2022-latest
docker network connect microservicesnetwork [containerId]