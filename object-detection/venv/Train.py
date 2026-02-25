from ultralytics import YOLO
import os

# Load a model
model = YOLO("yolov8n.pt") # build a new model from scratch

# #Use the model
results = model.train(data="D:/object-detection/venv/config.yaml", epochs=500) # train the model
