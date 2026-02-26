# Product Classification System

A comprehensive automated system designed for classifying products (specifically identifying and sorting bottle caps) using a combination of Computer Vision, Programmable Logic Controllers (PLC), Arduino-based sorting mechanisms, and a centralized SCADA system.

## 📑 Table of Contents
- [Project Overview](#project-overview)
- [System Architecture](#system-architecture)
- [Directory Structure](#directory-structure)
- [Technologies Used](#technologies-used)
- [Setup & Installation](#setup--installation)

## 🔍 Project Overview
This project implements a full-stack industrial automation solution to classify products on a conveyor system. It uses an AI-powered vision system to detect and classify objects (bottle caps), communicates the results to a PLC and Arduino for physical sorting using servo motors, and provides real-time monitoring through a custom-built SCADA interface.

## 🏗️ System Architecture
The system consists of four main modules working in tandem:
1.  **Computer Vision (AI):** Uses a custom-trained YOLOv8 model to detect and classify products via a camera feed.
2.  **SCADA System:** A C# Windows Forms application that monitors the camera feed, interfaces with the AI model, logs data, and communicates with the PLC.
3.  **Industrial Control (PLC):** Handles the main logic of the industrial process, receiving commands from the SCADA system.
4.  **Actuation (Arduino):** Listens to Modbus TCP commands to control a servo motor that physically sorts the classified products into appropriate bins.

## 📂 Directory Structure

* **`PLC/`**: Contains the Siemens TIA Portal V18 project files (`.ap18`) and HMI configurations for the industrial control logic.
* **`SCADA/`**: The C# .NET solution (`do_an_scada.sln`) containing the Graphical User Interface for system monitoring. It utilizes `S7.Net` for PLC communication and `AForge.Video` for capturing the camera stream.
* **`object-detection/`**: Python environment containing the YOLOv8 model configuration.
    * `Train.py`: Script used to train the YOLO model.
    * `YOLOnapchai.py`: Inference script for real-time bottle cap detection.
    * `yolov8n.pt` & `runs/`: Pre-trained weights and training performance graphs.
* **`data-train-yolo/`**: The dataset used to train the YOLO model, containing annotated images (`.jpg` and `.txt` label files) of bottle caps.
* **`arduinoMB_servo/`**: A PlatformIO project containing the C++ code (`src/main.cpp`) for the Arduino. It implements a Modbus TCP server to receive sorting signals and controls a servo motor accordingly.

## 🛠️ Technologies Used
* **Machine Learning / AI:** Python, Ultralytics YOLOv8, OpenCV
* **SCADA / Desktop App:** C# .NET Framework, Windows Forms, S7.Net, AForge.NET
* **Hardware / Embedded:** Arduino, PlatformIO, Modbus TCP, Servo Motors
* **Industrial Automation:** Siemens TIA Portal V18 (Step 7 / WinCC)

## 🚀 Setup & Installation

### 1. Object Detection (Python)
1. Navigate to the `object-detection` directory.
2. Ensure you have Python 3.8+ installed.
3. Install the required libraries:
   ```bash
   pip install ultralytics opencv-python
