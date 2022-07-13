@echo off
echo "Unzipping the IRTV.zip...
timeout 3
tar -xf IRTV.zip
echo "Restarting IRTV..."
timeout 3
start %~dp0InfiniteRuntimeTagViewer.exe