using JxMsgEntities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RabbitMqUtilCore.Base
{
    public abstract class BaseRabbitmqManager
    {
        private static readonly decimal s_taskExpirationMilliseconds = 10 * 60 * 1000;

        private static readonly decimal s_defaultMessageExpirationMilliseconds = 5 * 1000;

        private readonly ConnectionFactory _connectionFactory = new ConnectionFactory();

        private IConnection _connection;//一个客户端维护一个长链接

        private IModel _channel = null;//发送通道

        private static readonly ConcurrentDictionary<string, object> s_lockMap = new ConcurrentDictionary<string, object>();

        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        protected abstract void DoErrorHandle(Exception ex);

        public BaseRabbitmqManager(string hostName, int port, string userName, string password) : this(hostName, port, userName, password, ConnectionFactory.DefaultVHost)
        {
        }

        public BaseRabbitmqManager(string hostName, int port, string userName, string password, string virtualHost)
        {
            _connectionFactory.HostName = hostName;
            _connectionFactory.Port = port;
            _connectionFactory.UserName = userName;
            _connectionFactory.Password = password;

            if (!string.IsNullOrWhiteSpace(virtualHost))
            {
                _connectionFactory.VirtualHost = virtualHost;
            }

            _connectionFactory.ClientProvidedName = $"{hostName}:{port}:{virtualHost}:{GetHashCode()}";
            s_lockMap.TryAdd(_connectionFactory.ClientProvidedName, new object());
            _connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(60);
            _connectionFactory.RequestedConnectionTimeout = TimeSpan.FromSeconds(3);

            _timer.Interval = 5000;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        public string ClientProvidedName => _connectionFactory.ClientProvidedName;

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckConnection();
        }

        #region 连接管理

        private SendResult CheckConnection()
        {
            s_lockMap.TryGetValue(ClientProvidedName, out object objLock);

            lock (objLock)
            {
                if (_connection != null && _connection.IsOpen)
                {
                    //確認正常, 結束
                    return new SendResult() { IsSuccess = true };
                }

                //如果連線沒連上就重新建立
                try
                {
                    try
                    {
                        if (_connection != null)
                        {
                            _connection.Close();
                            _connection.Dispose();
                        }
                    }
                    catch(Exception ex)
                    {
                        DoErrorHandle(ex);
                    }

                    try
                    {
                        _connection = _connectionFactory.CreateConnection();
                    }
                    catch (Exception ex)
                    {
                        DoErrorHandle(ex);
                    }

                    if (_connection != null && _connection.IsOpen)
                    {
                        if (_channel != null)
                        {
                            try
                            {
                                _channel.Close();
                                _channel.Dispose();
                            }
                            catch (Exception ex)
                            {
                                DoErrorHandle(ex);
                            }
                        }
                        try
                        {
                            _channel = _connection.CreateModel();
                        }
                        catch (Exception ex)
                        {
                            DoErrorHandle(ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DoErrorHandle(ex);
                }

                if (_connection == null ||
                    !_connection.IsOpen ||
                    _channel == null ||
                    !_channel.IsOpen)
                {
                    return new SendResult()
                    {
                        IsSuccess = false,
                        Message = "建立连线失败"
                    };
                }

                return new SendResult() { IsSuccess = true };
            }
        }

        public IConnection GetConnection(bool isCheckConnection = true)
        {
            if (isCheckConnection)
            {
                CheckConnection();
            }

            return _connection;
        }

        public bool IsOpen => _connection != null && _connection.IsOpen;

        public IModel GetChannel()
        {
            CheckConnection();

            return _channel;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.IsOpen)
            {
                try
                {
                    _connection.Close();
                    _connection.Dispose();
                }
                catch (Exception ex)
                {
                    DoErrorHandle(ex);
                }
            }
        }

        #endregion 连接管理

        #region 发送消息

        public SendResult SendMessage(MessageEntity messageEntity) => SendMessage(messageEntity, s_defaultMessageExpirationMilliseconds);

        /// <summary>
        /// 注意：如果点到点 sendtype = 'direct' 交换机为amq.direct  路由键wcfrqserver（因为是先发给WCF里的RQ监听的）
        /// 如果是广播sendtype =‘fanout’ 交换机为amq.fanout，路由键为空
        /// </summary>
        /// <param name="messageEntity"></param>
        public SendResult SendMessage(MessageEntity messageEntity, decimal messageExpirationMilliseconds)
        {
            if (messageEntity == null)
            {
                return new SendResult()
                {
                    IsSuccess = false,
                    Message = "没有资料内容"
                };
            }

            SendResult sendResult = CheckConnection();

            if (!sendResult.IsSuccess)
            {
                return sendResult;
            }

            IBasicProperties properties = _channel.CreateBasicProperties();
            properties.Expiration = messageExpirationMilliseconds.ToString();//消息過期, 如果exchange有被bind到queue,可以避免訊息在queue中停留過久造成記憶體佔用

            string infos = JsonConvert.SerializeObject(messageEntity);
            byte[] bytes = Encoding.UTF8.GetBytes(infos);
            _channel.BasicPublish(messageEntity.SendExchange, messageEntity.SendRoutKey, properties, bytes);

            return sendResult;
        }

        public SendResult Enqueue<T>(string queueName, T model, decimal? taskExpirationMillisecond = null)
        {
            if (model == null)
            {
                return new SendResult()
                {
                    IsSuccess = false,
                    Message = "没有资料内容"
                };
            }

            if (!taskExpirationMillisecond.HasValue)
            {
                taskExpirationMillisecond = s_taskExpirationMilliseconds;
            }

            SendResult sendResult = CheckConnection();

            if (!sendResult.IsSuccess)
            {
                return sendResult;
            }

            IBasicProperties basicProperties = _channel.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.Expiration = taskExpirationMillisecond.ToString();

            string jsonString = JsonConvert.SerializeObject(model);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);
            _channel.BasicPublish(string.Empty, queueName, basicProperties, body); //開始傳遞

            return sendResult;
        }

        //public void SendMessageNoExpir(object obj)
        //{
        //    if (obj != null)
        //    {
        //        try
        //        {
        //            MessageEntity entity = obj as MessageEntity;
        //            this.checkconnection();
        //            if (this._connection != null && this._connection.IsOpen)
        //            {
        //                if (this._channel != null && this._channel.IsOpen)
        //                {
        //                    IBasicProperties properties = this._channel.CreateBasicProperties();
        //                    //properties.Expiration = "5000";//消息过期
        //                    string infos = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
        //                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(infos);
        //                    this._channel.BasicPublish(entity.SendExchange, entity.SendRoutKey, properties, bytes);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}

        #endregion 发送消息
    }


}