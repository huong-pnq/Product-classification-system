from ultralytics import YOLO
import os

# Load a model
model = YOLO("yolov8n.pt") # build a new model from scratch

# #Use the model
results = model.train(data="D:/object-detection/venv/config.yaml", epochs=500) # train the model


# model_path = os.path.join('D:/object-detection/venv/runs/detect/train3/weights/best.pt')

# # Load mô hình YOLO đã huấn luyện
# model = YOLO(model_path) 
# results = model.predict(source='D:/Downloads/anhnap1/anh7.jpg', save=True)