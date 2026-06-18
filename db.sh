#!/bin/bash

if [ "$1" = "-h" ] || [ "$1" = "--help" ]; then
  echo ""
  echo "EF Core Migration Helper"
  echo ""
  echo "Usage:"
  echo "  ./db.sh migrate <service> <MigrationName>"
  echo "  ./db.sh update <service>"
  echo "  ./db.sh remove <service>"
  echo ""
  echo "Services:"
  echo "  catalog"
  echo "  order"
  echo ""
  echo "Examples:"
  echo "  ./db.sh migrate catalog InitialCreate"
  echo "  ./db.sh update catalog"
  echo "  ./db.sh remove catalog"
  echo ""
  echo "  ./db.sh migrate order InitialCreate"
  echo "  ./db.sh update order"
  echo "  ./db.sh remove order"
  echo ""
  exit 0
fi

COMMAND=$1
SERVICE=$2
NAME=$3

if [ -z "$COMMAND" ] || [ -z "$SERVICE" ]; then
  echo "Usage:"
  echo "  ./db.sh migrate catalog MigrationName"
  echo "  ./db.sh update catalog"
  echo "  ./db.sh remove catalog"
  exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet is not installed."
  exit 1
fi

PROJECT=""
STARTUP=""
CONTEXT=""

case $SERVICE in
  catalog)
    PROJECT="Services/Catalog/Catalog.Infrastructure"
    STARTUP="Services/Catalog/Catalog.API"
    CONTEXT="Catalog.Infrastructure.Persistence.CatalogDbContext"
    ;;
  order)
    PROJECT="Services/Order/Order.Infrastructure"
    STARTUP="Services/Order/Order.API"
    CONTEXT="Order.Infrastructure.Persistence.OrderDbContext"
    ;;
  *)
    echo "Unknown service: $SERVICE"
    exit 1
    ;;
esac

if [ "$COMMAND" = "migrate" ]; then
  if [ -z "$NAME" ]; then
    echo "Migration name is required."
    echo "Example: ./db.sh migrate catalog AddBookDiscount"
    exit 1
  fi

  echo "Creating migration '$NAME' for service '$SERVICE'..."
  dotnet ef migrations add "$NAME" \
    --project "$PROJECT" \
    --startup-project "$STARTUP" \
    --context "$CONTEXT" \
    --output-dir Persistence/Migrations

elif [ "$COMMAND" = "update" ]; then
  echo "Updating database for service '$SERVICE'..."
  dotnet ef database update \
    --project "$PROJECT" \
    --startup-project "$STARTUP" \
    --context "$CONTEXT"

elif [ "$COMMAND" = "remove" ]; then
  echo "Removing last migration for service '$SERVICE'..."
  dotnet ef migrations remove \
    --project "$PROJECT" \
    --startup-project "$STARTUP" \
    --context "$CONTEXT"

else
  echo "Unknown command: $COMMAND"
  echo "Supported commands: migrate, update, remove"
  exit 1
fi
