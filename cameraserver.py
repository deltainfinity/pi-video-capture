import socket, errno
from camerastreamer import CameraStreamer

while True:
    #set up the listener on socket 12357
    server_socket = socket.socket()
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server_socket.bind(('0.0.0.0', 12357))
    server_socket.listen(0)

    #accept a single connection and use it as a file-like object
    connection = server_socket.accept()[0].makefile('wb')
    #create a CameraStreamer object and stream video on connection
    cs = CameraStreamer(640,480,24,60)
    try:
        cs.stream_video(connection)
        connection.close()
    except socket.error, e:
        if e.errno != 32 or e.errno != 104:
            raise
        else:
            pass
    finally:
        server_socket.close()
        
