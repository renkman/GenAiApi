# Gen AI DotNet API

[![.NET](https://github.com/renkman/GenAiApi/actions/workflows/ci-pipeline.yml/badge.svg)](https://github.com/renkman/GenAiApi/actions/workflows/ci-pipeline.yml)
[![codecov](https://codecov.io/gh/renkman/GenAiApi/graph/badge.svg?token=JCPHCBQ5W8)](https://codecov.io/gh/renkman/GenAiApi)

Simple API for [Ollama](https://ollama.com/) experiments.

Runs with the [Gemma3:b4 model](https://ai.google.dev/gemma/docs/core?hl=de).

The docker compose file contains configurations the ollama docker container and the API container. The API has a Swagger Open API site.

## Use cases

### Chat
The HTTP-POST route `/chat` just sends a message with the selected role to the Ollama API and returns its response.

### Image
The HTTP GET route `/image` captures a current image from a live camera in Hamburg and lets the AI model explain what the time of day and weather are like.
