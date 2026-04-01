# Set the WSL distribution and project path
$wslDistro = "Ubuntu"
$projectPath = "/home/ahmedgondal/mssql-EFCore-Deep-Dive"

# Execute commands inside WSL
wsl -d $wslDistro bash -c "cd $projectPath && docker compose up -d && exec bash"