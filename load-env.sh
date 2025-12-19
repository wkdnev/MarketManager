#!/bin/bash

# Load environment variables from .env file
# Usage: source load-env.sh

if [ -f .env ]; then
    echo "Loading environment variables from .env file..."
    export $(cat .env | grep -v '^#' | grep -v '^$' | xargs)
    echo "✅ Environment variables loaded successfully"
    echo ""
    echo "Configured variables:"
    echo "  EBAY_CLIENT_ID: ${EBAY_CLIENT_ID:+configured}"
    echo "  EBAY_CLIENT_SECRET: ${EBAY_CLIENT_SECRET:+configured}"
    echo "  EBAY_REDIRECT_URI: $EBAY_REDIRECT_URI"
    echo "  EBAY_ENVIRONMENT: $EBAY_ENVIRONMENT"
    echo "  EBAY_REFRESH_TOKEN: ${EBAY_REFRESH_TOKEN:+configured}"
else
    echo "⚠️  .env file not found"
    echo "Copy .env.example to .env and configure your eBay credentials:"
    echo "  cp .env.example .env"
    exit 1
fi
