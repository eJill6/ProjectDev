using JxMsgEntities;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqUtil
{
    public class CommonRabbitmqManager
    {
        private static ConnectionFactory s_connectionFactory = new ConnectionFactory();
        private IConnection _connection;//一个客户端维护一个长链接
        private IModel _sendMessageChannel = null;//发送通道
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();

        public CommonRabbitmqManager(string hostName, int port, string userName, string password)
        {
            s_connectionFactory.HostName = hostName;
            s_connectionFactory.Port = port;
            s_connectionFactory.UserName = userName;
            s_connectionFactory.Password = password;
            s_connectionFactory.RequestedHeartbeat = 60;
            s_connectionFactory.RequestedConnectionTimeout = 3000;
            //timer_HeartBeat.Interval = TimeSpan.FromSeconds(5);
            //timer_HeartBeat.Tick += new EventHandler(timer_HeartBeat_Tick);
            //timer_HeartBeat.Start();
            _timer.Interval = 5000;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.checkconnection();
        }
        void timer_HeartBeat_Tick(object sender, EventArgs e)
        {
            this.checkconnection();
        }

        #region 连接管理
        private void checkconnection()
        {
            if (this._connection == null || (this._connection != null && !this._connection.IsOpen))
            {
                try
                {
                    try
                    {
                        if (_connection != null)
                        {
                            this._connection.Close();
                            this._connection.Dispose();
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        this._connection = s_connectionFactory.CreateConnection();
                    }
                    catch (Exception)
                    {
                    }
                    if (this._connection != null && this._connection.IsOpen)
                    {
                        if (_sendMessageChannel != null)
                        {
                            try
                            {
                                _sendMessageChannel.Close();
                                _sendMessageChannel.Dispose();
                            }
                            catch
                            {
                            }
                        }
                        try
                        {
                            _sendMessageChannel = _connection.CreateModel();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public IConnection GetConnection()
        {
            checkconnection();
            return _connection;
        }

        public void closeConnection()
        {
            if (this._connection != null && this._connection.IsOpen)
            {
                try
                {
                    this._connection.Close();
                    this._connection.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 注意：如果点到点 sendtype = 'direct' 交换机为amq.direct  路由键wcfrqserver（因为是先发给WCF里的RQ监听的）
        /// 如果是广播sendtype =‘fanout’ 交换机为amq.fanout，路由键为空 
        /// </summary>
        /// <param name="obj"></param>
        public void SendMessage(object obj)
        {
            if (obj != null)
            {
                try
                {
                    MessageEntity entity = obj as MessageEntity;
                    this.checkconnection();
                    if (this._connection != null && this._connection.IsOpen)
                    {
                        if (this._sendMessageChannel != null && this._sendMessageChannel.IsOpen)
                        {

                            IBasicProperties properties = this._sendMessageChannel.CreateBasicProperties();
                            properties.Expiration = "5000";//消息过期
                            string infos = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(infos);
                            this._sendMessageChannel.BasicPublish(entity.SendExchange, entity.SendRoutKey, properties, bytes);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        public void SendMessageNoExpir(object obj)
        {
            if (obj != null)
            {
                try
                {
                    MessageEntity entity = obj as MessageEntity;
                    this.checkconnection();
                    if (this._connection != null && this._connection.IsOpen)
                    {
                        if (this._sendMessageChannel != null && this._sendMessageChannel.IsOpen)
                        {
                            IBasicProperties properties = this._sendMessageChannel.CreateBasicProperties();
                            //properties.Expiration = "5000";//消息过期
                            string infos = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(infos);
                            this._sendMessageChannel.BasicPublish(entity.SendExchange, entity.SendRoutKey, properties, bytes);
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
        #endregion
    }
}
