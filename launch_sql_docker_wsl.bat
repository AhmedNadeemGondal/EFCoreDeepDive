@echo off
start pwsh -NoExit -Command "wsl -d Ubuntu bash -c 'cd /home/ahmedgondal/mssql-EFCore-Deep-Dive && docker compose up -d && exec bash'"
