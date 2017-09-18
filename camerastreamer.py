import picamera
import time

class CameraStreamer:
    """A wrapper class to stream video to a client"""
    
    #constructor
    def __init__(self, h_resolution = 640, v_resolution = 480, framerate = 24, length = 60):
        #set the class attributes
        self.h_resolution = h_resolution
        self.v_resolution = v_resolution
        self.framerate = framerate
        self.length = length
        

    def stream_video(self, connection):
        #create the camera object using context manager protocol
        with picamera.PiCamera() as camera:
            #set the resolution and frame rate
            camera.resolution = (self.h_resolution, self.v_resolution)
            camera.framerate = self.framerate
            #allow the camera to warm up
            time.sleep(2)
            #start recording and send output to connection
            camera.start_recording(connection, format='h264')
            camera.wait_recording(self.length)
            camera.stop_recording()
               
    
