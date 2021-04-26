# Middleware

## Target

ASP.Net Core.

## What

A collection of small middleware for ASP.Net Core appliations.

### Content

- MeasureSuccessfulEndpointRequestsMiddleware
    - A starting point for monitoring requests. In the snippet given a custom implementation of prometheus-net is used to count all successful GET requests.