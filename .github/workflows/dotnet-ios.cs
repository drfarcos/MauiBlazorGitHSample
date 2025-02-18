﻿name: Build .NET MAUI iOS App

on:
  push:
    branches: [ "master" ]
  pull_request:
    branches: [ "master" ]

jobs:
  build:

    runs-on: macos-latest  # Используем macOS, так как требуется для сборки iOS-приложений

    steps:
    - uses: actions/checkout@v4
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x
    - name: Install .NET MAUI
      run: dotnet workload install maui 
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build ${{ github.workspace }}/MauiBlazorGitHSample.csproj -c Release -f net9.0-ios --no-restore
    - name: Publish
      run: dotnet publish ${{ github.workspace }}/MauiBlazorGitHSample.csproj -c Release -f net9.0-ios --output ${{ github.workspace }}/publish

    - name: Create .app
      run: |
        cd ${{ github.workspace }}/publish
        mkdir -p Payload
        mv *.app Payload/
        zip -r MauiBlazorGitHSample.app.zip Payload/

    - name: Upload .app
      uses: actions/upload-artifact@v4
      with:
        name: MauiBlazorGitHSample.app.zip
        path: ${{ github.workspace }}/publish/MauiBlazorGitHSample.app.zip

