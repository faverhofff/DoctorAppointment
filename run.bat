@echo off
cd src\DoctorAppoinmentApi
docker build -t doctor_appointment -f Dockerfile .
docker run doctor_appointment
