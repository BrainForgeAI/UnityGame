# Unity Setup Guide

## Prerequisites

- Unity installed on your system (make sure its the Student Plan Version)
- Editor Version: 2022.3.35f1

## Getting Started

1. Clone / Pull the latest changes from the repository.

2. Open Unity Hub.

3. Click "Add project" (NOT "New project").

4. Select the "Brainforge Unity Game" folder as the project directory.

   - **Important:** Do not select the "unity-game-implementation" folder, as Unity won't detect the necessary files.

5. If prompted, choose the "2D Universal" environment.

## Exploring the Project

To begin working on the game:

1. Navigate to `Assets -> 2D RPG KIT -> Scenes -> Demo`.
2. Choose a map section to start with.

**Note:** There are two "Scene" folders. Use the one from the 2D RPG KIT asset, not the vanilla Unity folder.

To get Gemini to prompt questions, you'll need to clone another repository and follow the rules there, as that aspect is on another repository: https://github.com/BrainForgeAI/GeminiApp

# Python Setup Guide

## Getting started

1. Clone the project.

2. (Recommended) Create and activate a [virtual environment](https://docs.python.org/3/tutorial/venv.html)

3. Install dependencies by using `pip3 install -r requirements.txt`

4. Set up your [API key](#setting-up-an-api-key)

5. Set up [pre-commit linting](#linting)

## Setting up an API key

1. Get an [API key](https://aistudio.google.com/app/apikey)

2. Create a file called `.env` in the root of the project directory.

3. Create a variable `API_KEY=your_api_key` in the `.env` file.

## Linting

1. Run `pre-commit install` in the root of your project directory.

2. (Optional) Run pre-commit hooks manually `pre-commit run --all-files`.
