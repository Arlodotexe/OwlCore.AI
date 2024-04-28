# OwlCore.AI [![Version](https://img.shields.io/nuget/v/OwlCore.AI.svg)](https://www.nuget.org/packages/OwlCore.AI)

Tools and helpers for building AI and ML applications.

## Featuring:
- Menus: Build firmware-inspired menus and have a large language model infer the option to pick given a user request. Only uses 2-4 tokens per menu inference. 
- IModelInference: a basic inference abstraction for hot-swapping inference APIs. Flexible enough for multimodal inference such as LlamaSharp, OpenAI, LMStudio, Whisper, etc.

## Install

Published releases are available on [NuGet](https://www.nuget.org/packages/OwlCore.AI). To install, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console).

    PM> Install-Package OwlCore.AI
    
Or using [dotnet](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet)

    > dotnet add package OwlCore.AI

## Usage

```cs
var test = new Thing();
```

## Financing

We accept donations [here](https://github.com/sponsors/Arlodotexe) and [here](https://www.patreon.com/arlodotexe), and we do not have any active bug bounties.

## Versioning

Version numbering follows the Semantic versioning approach. However, if the major version is `0`, the code is considered alpha and breaking changes may occur as a minor update.

## License

All OwlCore code is licensed under the MIT License. OwlCore is licensed under the MIT License. See the [LICENSE](./src/LICENSE.txt) file for more details.
