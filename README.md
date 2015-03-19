Moviepicker
===========

This app will suggest movies for you to watch. It takes into account your history to determine what movie you might like based on certain metrics. 

General concept mockups:

![UI Mockup](http://i.imgur.com/QWsAFsX.png)


The project in its current state consists of a backend (ASP.NET Web Api 2) and frontend (Android). The API will be hosted on azure.

## Solution layout

This section will describe what each project in the solution aims to do.

### Database

Provides interaction with the database and exposes repositories to other projects.

### DataService

Provides scripts that will fill the database with relevant data and keep it updated.

### Models

Storage for the centrally used models which are used by all relevant projects.

### Tests

Unit- and functional tests that test API calls from start to finish, url routing, database interaction, etc.

### TMDbWrapper

Wrapper around the most important aspects of the TMDb API which will seed the database with data.

### WebApi

REST API to expose an interface to the frontend application(s).

## TODO

Right now the small existing codebase will be rewritten. However before that happens, I'll first create a system that downloads all relevant data into a database and create my own slimmed down wrapper around the API.

## Links

Trello board: https://trello.com/b/y3iXYUk2/moviepicker

