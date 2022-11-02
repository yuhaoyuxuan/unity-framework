using UnityEngine;
using UnityEngine.UI;
using Slf;
using System.Net.Sockets;
using System.IO;
using System;
using System.Net;

namespace Slf
{
    /// <summary>
    /// socket状态
    /// </summary>
    public enum SocketStatus
    {
        None = 0,           //默认值
        CONNECTING,         //连接中
        CONNECTED,          //已连接
        FAILED,             //连接失败
        CLOSE               //断开连接
    }

    //==========================
    // - Author:      slf         
    // - Date:        2021/07/28 11:05:27	
    // - Description: 套接字
    //==========================
    public class SocketClient
    {
        private Socket socket;
        /// <summary>
        /// 实例名字
        /// </summary>
        public string name;
        /// <summary>
        /// 消息头长度
        /// </summary>
        public int headLength = 8;
        private const int BUFFER_SIZE = 1024;
        private byte[] buffer = new byte[BUFFER_SIZE];
        //缓存收到的数据流
        private MemoryStream receiveBuffer;
        //获取到的消息体长度\id
        private int msgLength = -1;
        private int msgId = -1;

        private SocketStatus status = SocketStatus.None;
        /// <summary>
        /// 连接成功 
        /// </summary>
        public Action onOpen;
        /// <summary>
        /// 连接关闭 
        /// </summary>
        public Action onClose;
        /// <summary>
        /// 连接失败 
        /// </summary>
        public Action onError;
        /// <summary>
        /// 收到消息 
        /// </summary>
        public Action<int, byte[]> onMessage;

        public SocketClient(string name,int headLength = 8)
        {
            this.name = name;
            this.headLength = headLength;
        }

        //是否连接
        public bool isConnected
        {
            get
            {
                return status == SocketStatus.CONNECTED && socket != null && socket.Connected;
            }
        }

        //重置数据
        private void resetData()
        {
            Debug.LogError(name+"=重置socket数据");
            msgLength = -1;
            msgId = -1;
            if (receiveBuffer != null)
            {
                receiveBuffer.Dispose();
                receiveBuffer = null;
            }

            if (socket != null)
            {
                if (socket.Connected)
                {
                    socket.Close();
                }
                socket = null;
            }
        }

        /// <summary>
        /// 主动关闭连接
        /// </summary>
        public void disConnect()
        {
            setStatus(SocketStatus.CLOSE, "手动关闭连接");
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="ipStr"></param>
        /// <param name="port"></param>
        public void connect(string ipStr, int port)
        {
            if (status == SocketStatus.CONNECTING || status == SocketStatus.CONNECTED)
            {
                Debug.LogError("重复连接socket");
                return;
            }
            setStatus(SocketStatus.CONNECTING, "connect" + ipStr + ":" + port);
            IPAddress ip = IPAddress.Parse(ipStr);
            IPEndPoint ipe = new IPEndPoint(ip, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            receiveBuffer = new MemoryStream();
            socket.BeginConnect(ip, port, new AsyncCallback(connectCallback), socket);
        }

        //连接回调
        private void connectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                setStatus(SocketStatus.CONNECTED ,"success");
                receive(client);
            }
            catch (Exception e)
            {
                setStatus(SocketStatus.FAILED, e.ToString());
            }
        }

        //发送消息
        public void sendMsg(byte[] bytes)
        {
            if (isConnected)
            {
                try
                {
                    socket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(sendCallback), socket);
                }
                catch (Exception e)
                {
                    setStatus(SocketStatus.CLOSE, e.ToString());
                }
            }
        }

        //发送成功
        private void sendCallback(IAsyncResult ar)
        {
            int bytesSent = socket.EndSend(ar);
            //Debug.Log("Socket sendComplete"+bytesSent);
        }

        //开始接受数据
        private void receive(Socket client)
        {
            try
            {
                client.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(receiveCallabck), client);
            }
            catch (Exception e)
            {
                Debug.LogError("接收消息异常==" + e.ToString());
            }
        }

        //收到数据
        private void receiveCallabck(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int read = client.EndReceive(ar);
            byte[] bodyBuffer;  //一条完整的消息体
            if (read <= 0)  //远端断开连接
            {
                setStatus(SocketStatus.CLOSE, "空数据 主动断开连接");
                return;
            }

            //把收到的数据写入缓存 内存流
            receiveBuffer.Position = receiveBuffer.Length;
            receiveBuffer.Write(buffer, 0, read);
            receiveBuffer.Position = 0;

            BinaryReader br = new BinaryReader(receiveBuffer);
            //获取消息头  body长度
            if (msgLength == -1 && receiveBuffer.Length >= headLength)
            {
                msgId = br.ReadInt32();
                msgLength = br.ReadInt32();
            }

            //获取内存流剩余长度
            int remaining = (int)(receiveBuffer.Length - receiveBuffer.Position);
            //循环检测  如果剩余内存流长度 大于等于 消息长度
            while (msgLength >= 0 && remaining >= msgLength)
            {
                //服务器 返回空消息体
                if (msgLength == 0)
                {
                    bodyBuffer = new byte[0];
                }
                else
                {
                    bodyBuffer = br.ReadBytes(msgLength);
                }
                onReceiveMessage(bodyBuffer);

                //获取内存流剩余长度
                remaining = (int)(receiveBuffer.Length - receiveBuffer.Position);
                if (remaining >= headLength)
                {
                    msgId = br.ReadInt32();
                    msgLength = br.ReadInt32();
                }
                else
                {
                    msgLength = -1;
                }
            }

            //获取内存流剩余长度
            remaining = (int)(receiveBuffer.Length - receiveBuffer.Position);

            //粘包、半包情况  把收到的内存流 有效字节的位置移到0
            if (remaining > 0)
            {
                MemoryStream temp = new MemoryStream();
                receiveBuffer.CopyTo(temp);
                receiveBuffer.Dispose();
                receiveBuffer = temp;
            }

            receiveBuffer.Position = 0;
            receiveBuffer.SetLength(remaining);
            receive(client);
        }

        //收到完整消息 上抛数据
        private void onReceiveMessage(byte[] bytes)
        {
            if (onMessage != null)
            {
                onMessage(msgId, bytes);
            }
        }


        //设置状态
        private void setStatus(SocketStatus type, string str = "")
        {
            status = type;
            Debug.LogError(status + "_______" + str);
            switch (type)
            {
                case SocketStatus.CONNECTING:
                    break;
                case SocketStatus.CONNECTED:
                    if (onOpen != null)
                    {
                        onOpen();
                    }
                    break;
                case SocketStatus.FAILED:
                    receiveBuffer = null;
                    socket.Close();
                    socket = null;
                    if (onError != null)
                    {
                        onError();
                    }
                    break;
                case SocketStatus.CLOSE:
                    resetData();
                    if (onClose != null)
                    {
                        onClose();
                    }
                    break;
            }
        }
    }
}