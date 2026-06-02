# sql-distributed-systems-project

ASP.NET Core 8 web app with two SQL Server databases. Workflow: develop locally with Docker Compose, then deploy to Kubernetes.

## Prerequisites

- Docker Desktop
- kubectl (Kubernetes only)
- .NET 8 SDK (local dev only)


## Setup 

All sensitive values live in `mywebapp/.env`. 

Template:

```
# mywebapp/.env
SA_PASSWORD=your_sa_password
APPDB_CONNECTION_STRING=Server=sqlserver1,1433;Database=AppDb;User Id=sa;Password=your_sa_password;TrustServerCertificate=True;
LOGDB_CONNECTION_STRING=Server=sqlserver2,1433;Database=LogDb;User Id=sa;Password=your_sa_password;TrustServerCertificate=True;
K8S_APPDB_CONNECTION_STRING=Server=host.docker.internal,1533;Database=AppDb;User Id=sa;Password=your_sa_password;TrustServerCertificate=True;
K8S_LOGDB_CONNECTION_STRING=Server=host.docker.internal,1534;Database=LogDb;User Id=sa;Password=your_sa_password;TrustServerCertificate=True;
```



## Docker Compose

Runs two SQL Server containers + web app. 

```bash
cd mywebapp
docker-compose up --build
```


Web app -> http://localhost:8000

HTTPS   -> https://localhost:8001

To stop:

```bash
docker-compose down
```

## Deploy to Kubernetes



Build the image (must be done before deploying, since `imagePullPolicy: Never`):

```bash
docker build -t mywebapp-webapp:latest .
```

Create the Kubernetes secret from your `.env` file (only needed once, or when secrets change):

```bash
kubectl create secret generic app-secrets --from-env-file=mywebapp/.env
```

To update secrets later without recreating:

```bash
kubectl create secret generic app-secrets --from-env-file=mywebapp/.env --dry-run=client -o yaml | kubectl apply -f -
```

Deploy using:

```bash
kubectl apply -f mywebapp/k8s-deployment.yaml
```

App is exposed on **NodePort 30080**: http://localhost:30080

Teardown using:

```bash
kubectl delete -f mywebapp/k8s-deployment.yaml
kubectl delete secret app-secrets
```



