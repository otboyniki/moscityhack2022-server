#!/bin/bash

n=0

print_usage() {
  printf "Usage: ..."
}

while getopts 'n:v' flag; do
  case "${flag}" in
    n) n=$(($OPTARG - 1)) ;;
    *) print_usage
       exit 1 ;;
  esac
done

last_migration=$(ls ../Migrations | ggrep -Po '^\d.*(?<!Designer\.cs)$' | tail -n $((2 + $n)) | head -n 1 | sed 's_\.cs$__g')
./update.sh "$last_migration"

for ((i = 0; i <= n; i++))
do
  ./remove.sh
done