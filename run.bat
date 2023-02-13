@echo off
docker run -d -p 27017:27017 mongo:latest

cd src
docker build -t doctor_appointment -f Dockerfile .
docker run -p 80:80 doctor_appointment 
