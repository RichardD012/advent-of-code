# Advent of Code

This project is for the [advent of code](https://adventofcode.com/) challenge. This project allows support for multiple years and auto-determining the day and year from the current day.

## Setup

Sign in on https://adventofcode.com/ and grab the cookie that is used for the site. Place this in an `.env` file and run the project for the day you want by overriding the day and/or year overrides.

### Requirements

- For Day 13 you need to populate the .env file with a Google Cloud Platform Vision API Service Key Location: https://console.cloud.google.com/apis/credentials. If no file is found the output image will be saved as Day13.png
