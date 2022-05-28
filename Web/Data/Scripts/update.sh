#!/bin/bash

if [ -z "$1" ]
then
  ./common.sh "database update";
else
  ./common.sh "database update $1";
fi