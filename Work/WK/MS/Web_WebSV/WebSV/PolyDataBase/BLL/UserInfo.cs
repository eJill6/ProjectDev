namespace SLPolyGame.Web.BLL
{
    /// <summary>
    /// UserInfo
    /// </summary>
    public class UserInfo
    {
        private readonly SLPolyGame.Web.DAL.UserInfo dal = new SLPolyGame.Web.DAL.UserInfo();

        public UserInfo()
        {
        }

        #region Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            return dal.Exists(UserID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.UserInfo model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SLPolyGame.Web.Model.UserInfo GetModel(int UserID)
        {
            return dal.GetModel(UserID);
        }

        #endregion Method
    }
}