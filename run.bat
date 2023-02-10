@echo off
docker run -d -p 27017:27017 --name test-mongo mongo:latest

cd src\DoctorAppoinmentApi
docker build -t doctor_appointment -f Dockerfile .
docker run doctor_appointment
