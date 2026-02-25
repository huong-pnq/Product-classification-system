import os
from ultralytics import YOLO
import cv2
import numpy as np
from pymodbus.client import ModbusTcpClient 
import base64

client = ModbusTcpClient('192.168.5.3', port=502)
connection_status = client.connect()
# Check if the connection was successful
if connection_status:
    print('Successfully connected to PLC')
else:
    print('Failed to connect to PLC')
# Proceed with the rest of your code if connected
if connection_status:
    # Your existing code here
    pass
else:
    print("Connection to PLC failed. Exiting.")
    client.close()

# Dùng số 0 để lấy hình ảnh từ webcam mặc định
cap = cv2.VideoCapture(1)

# Kiểm tra xem có đọc được hình ảnh từ webcam hay không
ret, frame = cap.read()

# Lấy kích thước khung hình
H, W, _ = frame.shape

# Đường dẫn tới mô hình YOLO
model_path = os.path.join('D:/object-detection/runs/detect/train4/weights/best.pt')

# Tải mô hình YOLO
model = YOLO(model_path)  # tải mô hình đã huấn luyện

threshold = 0.90  # Ngưỡng phát hiện đối tượng

# Định nghĩa các màu sắc cho từng lớp
colors = {
    0: (255, 0, 0),  # Màu xanh dương cho class_id = 0
    1: (255, 0, 0),  # Màu xanh dương cho class_id = 1
    2: (255, 0, 0),  # Màu xanh dương cho class_id = 2
    3: (0, 0, 255),  # Màu đỏ cho class_id = 3
}

while True:
    ret, frame = cap.read()  # Đọc từng khung hình từ webcam
    if not ret:
        break

    # Phát hiện đối tượng trong khung hình
    results = model(frame)[0]

    # Vẽ khung và tên đối tượng nếu phát hiện
    for result in results.boxes.data.tolist():
        x1, y1, x2, y2, score, class_id = result
        
        # Gửi giá trị lên PLC dựa trên class_id
        if int(class_id) == 0:  # Class 0
            client.write_register(0, 1)  # Gửi giá trị 1 tới thanh ghi 0
        elif int(class_id) == 1:  # Class 1
            client.write_register(0, 2)  # Gửi giá trị 2 tới thanh ghi 0
        elif int(class_id) == 2:  # Class 2
            client.write_register(0, 3)  # Gửi giá trị 3 tới thanh ghi 0
        elif int(class_id) == 3:  # Class 3
            client.write_register(0, 4)  # Gửi giá trị 4 tới thanh ghi 0
        
        if  score > threshold:
            print('ID', score)
            color = colors.get(int(class_id), (255, 255, 255))  # Màu trắng nếu không tìm thấy
            cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), color, 4)
            cv2.putText(frame, results.names[int(class_id)].upper(), (int(x1), int(y1 - 10)),
                        cv2.FONT_HERSHEY_SIMPLEX, 1.3, color, 3, cv2.LINE_AA)
        
        else:
            print('ID', score)
            cv2.rectangle(frame, (int(x1), int(y1)), (int(x2), int(y2)), (0, 0,  255), 4)
            cv2.putText(frame, 'UNKNOWN', (int(x1), int(y1 - 10)),
                        cv2.FONT_HERSHEY_SIMPLEX, 1.3, (0, 0,  255), 3, cv2.LINE_AA)
            client.write_register(1, 1)
        

    # Hiển thị khung hình với các đối tượng được phát hiện
    #cv2.imshow('Webcam Object Detection', frame)

    # Nhấn phím 'esc' để thoát
    if cv2.waitKey(1) == 27:
        break
    # Encode image as base64 string
    retval, buffer = cv2.imencode('.jpg', frame)
    image_base64 = base64.b64encode(buffer).decode("utf-8")

    # Send frame data to C# process using print (modify if needed)
    print(f"FRAME:{image_base64}")  # Send marker and base64 data

# Giải phóng tài nguyên
cap.release()
cv2.destroyAllWindows()
