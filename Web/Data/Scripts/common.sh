#!/bin/bash

light_cyan='\033[1;36m'
nc='\033[0m'

command="dotnet ef -v $1 --project ../../ -c DataContext"
echo -e "${light_cyan}${command}${nc}"
$command