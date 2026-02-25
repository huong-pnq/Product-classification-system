# Product Classification System

This repository contains the source code and project files for an automated **Product Classification System**. The system integrates computer vision (YOLOv8) for product inspection, a Siemens PLC for core automation logic, an Arduino-based Modbus TCP servo controller for physical sorting, and a custom C# SCADA application for monitoring and control.

## 🏗️ System Architecture

The project is divided into four main components:

### 1. Object Detection (`/object-detection` & `/data-train-yolo`)
* **Role**: Uses a camera to capture images of products (e.g., bottle caps) and classifies them in real-time.
* **Technologies**: Python, OpenCV, Ultralytics YOLOv8.
* **Contents**:
    * `data-train-yolo/`: Custom dataset containing training images (`anhnap.jpg`) and YOLO format labels.
    * `object-detection/`: Python scripts (`Train.py` for model training and `YOLOnapchai.py` for real-time inference).
    * Contains the pre-trained weights (`best.pt`, `last.pt` inside `runs/detect/train4/weights/`).

### 2. SCADA Application (`/SCADA`)
* **Role**: A Human-Machine Interface (HMI) for operators to monitor the system, view the live camera feed, track statistics, and control the process.
* **Technologies**: C# .NET Windows Forms.
* **Key Libraries**:
    * `S7.Net`: To communicate directly with the Siemens PLC.
    * `AForge.Video`: To handle direct video streaming from connected webcams.
* **Contents**: Visual Studio solution (`do_an_scada.sln`) with forms for User Login and Main Dashboard (`Form1.cs`).

### 3. PLC Control (`/PLC`)
* **Role**: The central "brain" of the physical automation process. It handles sensor inputs, interacts with the SCADA system, and dictates sorting actions.
* **Technologies**: Siemens TIA Portal V18.
* **Contents**: The TIA Portal project file (`PLC.ap18`) and associated system configurations.

### 4. Servo Controller (`/arduinoMB_servo`)
* **Role**: Actuates a servo motor to physically sort/reject products on the conveyor belt based on commands received over the network.
* **Technologies**: C++, Arduino, PlatformIO.
* **Key Libraries**: `ModbusTCP` to communicate with the PLC or SCADA system as a Modbus slave/server.
* **Contents**: PlatformIO project (`platformio.ini`) and source code (`main.cpp`).

---

## 🚀 Prerequisites

To work with or run the full system, you will need the following software installed:

* **Python 3.8+**: For running the YOLOv8 object detection scripts.
    * Packages: `ultralytics`, `opencv-python`, `socket`
* **Visual Studio 2019/2022**: For compiling and running the C# SCADA application.
* **Siemens TIA Portal V18**: For opening and compiling the PLC project.
* **VS Code with PlatformIO Extension**: (or Arduino IDE) for compiling and flashing the Arduino code.

---

## ⚙️ Setup and Execution

### 1. Object Detection (Vision System)
1. Navigate to the `object-detection/venv/` directory.
2. Install the required Python packages:
   ```bash
   pip install ultralytics opencv-python
